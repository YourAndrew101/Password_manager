using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace Services
{
    internal class Color
    {
        public string hex { get; set; }
        public string type { get; set; }
        public int brightness { get; set; }
    }

    internal class Font
    {
        public string name { get; set; }
        public string type { get; set; }
        public string origin { get; set; }
        public string originId { get; set; }
        public List<object> weights { get; set; }
    }

    internal class Format
    {
        public string src { get; set; }
        public string background { get; set; }
        public string format { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int size { get; set; }
    }

    internal class Image
    {
        public string type { get; set; }
        public List<Format> formats { get; set; }
    }

    internal class Link
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    internal class Logo
    {
        public string type { get; set; }
        public object theme { get; set; }
        public List<Format> formats { get; set; }
    }

    internal class Root
    {
        public string name { get; set; }
        public string domain { get; set; }
        public bool claimed { get; set; }
        public string description { get; set; }
        public List<Link> links { get; set; }
        public List<Logo> logos { get; set; }
        public List<Color> colors { get; set; }
        public List<Font> fonts { get; set; }
        public List<Image> images { get; set; }
    }
    public class LogoService
    {
        private static bool IsConnectedToInternet()
        {
            string host = "8.8.8.8";
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }

            return false;
        }
        private static void MakeCopy(string resourceName)
        {
            File.Copy($"../../MainWindow/LogosServices/defaultLogo.png", $"../../MainWindow/LogosServices/{resourceName}.png");
        }

        public static string GetLogo(string resourceName)
        {
            string path = $"../../MainWindow/LogosServices/{resourceName}.png";
            if (!File.Exists(path))
            {
                if (!IsConnectedToInternet()) return $"../../MainWindow/LogosServices/defaultLogo.png";
                XmlDocument xml = new XmlDocument();
                xml.Load("../../../Services/APIList.xml");
                XmlElement root = xml.DocumentElement;
                string key = root.FirstChild.InnerText;
                string uri = $"https://api.brandfetch.io/v2/brands/{resourceName}";
                WebResponse response = null;
                WebRequest request = request = WebRequest.Create(uri);

                request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {key}");

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

                    if (errorResponse.StatusCode == HttpStatusCode.BadRequest || errorResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        MakeCopy(resourceName);
                        return path;
                    }

                }
                StreamReader streamReader = response != null ? new StreamReader(response.GetResponseStream()) : throw new Exception();
                string json = streamReader.ReadToEnd();
                streamReader.Close();
                response.Close();

                Root data = JsonConvert.DeserializeObject<Root>(json);
                if (!data.logos.Contains(data.logos.FirstOrDefault(e => e.type == "icon")))
                {
                    MakeCopy(resourceName);
                    return path;
                }
                string logoUri = data.logos.FirstOrDefault(e => e.type == "icon").formats[0].src;
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(new Uri(logoUri), path);
                }
            }


            return path;
        }

    }
}
