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
    public class Color
    {
        public string hex { get; set; }
        public string type { get; set; }
        public int brightness { get; set; }
    }

    public class Font
    {
        public string name { get; set; }
        public string type { get; set; }
        public string origin { get; set; }
        public string originId { get; set; }
        public List<object> weights { get; set; }
    }

    public class Format
    {
        public string src { get; set; }
        public string background { get; set; }
        public string format { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int size { get; set; }
    }

    public class Image
    {
        public string type { get; set; }
        public List<Format> formats { get; set; }
    }

    public class Link
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Logo
    {
        public string type { get; set; }
        public object theme { get; set; }
        public List<Format> formats { get; set; }
    }

    public class Root
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

        private static void GetLogoFromAPI(string resourceName)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("../../../Services/APIList.xml");
            XmlElement root = xml.DocumentElement;
            string key= root.FirstChild.InnerText;
            string uri = $"https://api.brandfetch.io/v2/brands/{resourceName}";
            WebRequest request = WebRequest.Create(uri);
            request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {key}");
            WebResponse response = null;
            
            try
            {
               response = request.GetResponse();
            }
            catch (WebException we)
            {
                HttpWebResponse errorResponse = (HttpWebResponse)we.Response;
               
                if (errorResponse.StatusCode==HttpStatusCode.Forbidden)
                {
                    root.RemoveChild(root.FirstChild);
                    xml.Save("../../../Services/APIList.xml");
                    GetLogoFromAPI(resourceName);
                }
                else { return; }
            }
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string json = streamReader.ReadToEnd();
            streamReader.Close();
            response.Close();

            Root data = JsonConvert.DeserializeObject<Root>(json);
            if (data.logos.Contains(data.logos.FirstOrDefault(e => e.type == "icon")))
            {
                string logoUri = data.logos.FirstOrDefault(e => e.type == "icon").formats[0].src;
                string path = $"../../MainWindow/LogosServices/{resourceName}.png";
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(new Uri(logoUri), path);
                }
            }
            else
            {
                throw new Exception();
            }
            
        }
        public static string GetLogo(string resourceName)
        {
            string path= $"../../MainWindow/LogosServices/{resourceName}.png";
            if (!File.Exists(path))
            {
                try
                {
                    GetLogoFromAPI(resourceName);                    
                }
                catch (Exception)
                {
                    File.Copy($"../../MainWindow/LogosServices/defaultLogo.png", $"../../MainWindow/LogosServices/{resourceName}.png");
                }              
            }
            return path; 
        }
        
    }
}
