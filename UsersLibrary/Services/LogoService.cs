using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace Services
{
    internal class Color
    {
        internal string hex { get; set; }
        internal string type { get; set; }
        internal int brightness { get; set; }
    }

    internal class Font
    {
        internal string name { get; set; }
        internal string type { get; set; }
        internal string origin { get; set; }
        internal string originId { get; set; }
        internal List<object> weights { get; set; }
    }

    internal class Format
    {
        internal string src { get; set; }
        internal string background { get; set; }
        internal string format { get; set; }
        internal int height { get; set; }
        internal int width { get; set; }
        internal int size { get; set; }
    }

    internal class Image
    {
        internal string type { get; set; }
        internal List<Format> formats { get; set; }
    }

    internal class Link
    {
        internal string name { get; set; }
        internal string url { get; set; }
    }

    internal class Logo
    {
        internal string type { get; set; }
        internal object theme { get; set; }
        internal List<Format> formats { get; set; }
    }

    internal class Root
    {
        internal string name { get; set; }
        internal string domain { get; set; }
        internal bool claimed { get; set; }
        internal string description { get; set; }
        internal List<Link> links { get; set; }
        internal List<Logo> logos { get; set; }
        internal List<Color> colors { get; set; }
        internal List<Font> fonts { get; set; }
        internal List<Image> images { get; set; }
    }
    public class LogoService
    {
        public static string GetLogo(string resourceName)
        {
            string path = $"../../MainWindow/LogosServices/{resourceName}.png";
            if (!File.Exists(path))
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load("../../../Services/APIList.xml");
                    XmlElement root = xml.DocumentElement;

                    string key = root.FirstChild.InnerText;
                    string uri = $"https://api.brandfetch.io/v2/brands/{resourceName}";

                    WebResponse response;
                    WebRequest request;
                    try
                    {
                        request = WebRequest.Create(uri);
                        request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {key}");
                        response = null;
                    }
                    catch (WebException) { return $"../../MainWindow/LogosServices/defaultLogo.png"; }

                    try
                    {
                        response = request.GetResponse();
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse errorResponse = (HttpWebResponse)we.Response;

                        if (errorResponse.StatusCode == HttpStatusCode.Forbidden)
                        {
                            response.Close();
                            root.RemoveChild(root.FirstChild);
                            xml.Save("../../../Services/APIList.xml");
                            return GetLogo(resourceName);
                        }

                    }
                    StreamReader streamReader = response != null ? new StreamReader(response.GetResponseStream()) : throw new Exception();
                    string json = streamReader.ReadToEnd();
                    streamReader.Close();
                    response.Close();

                    Root data = JsonConvert.DeserializeObject<Root>(json);
                    if (!data.logos.Contains(data.logos.FirstOrDefault(e => e.type == "icon")))
                    {
                        throw new Exception();
                    }
                    string logoUri = data.logos.FirstOrDefault(e => e.type == "icon").formats[0].src;
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(new Uri(logoUri), path);
                    }
                }
                catch (Exception)
                {
                    if(!File.Exists($"../../MainWindow/LogosServices/{resourceName}.png"))
                        File.Copy($"../../MainWindow/LogosServices/defaultLogo.png", $"../../MainWindow/LogosServices/{resourceName}.png");
                }
            }
            return path;
        }

    }
}
