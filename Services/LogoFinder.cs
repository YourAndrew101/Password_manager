using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
   public class LogoFinder
    {
        private static void GetLogoFromAPI(string resourceName)
        {
            string uri = $"https://api.brandfetch.io/v2/brands/{resourceName}";
            WebRequest request = WebRequest.Create(uri);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer EdPUEywZxj74PT9Mt/Rumbp+QtYkL6gH4k5H2K+cHXM=");
            WebResponse response = request.GetResponse();

            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string json = streamReader.ReadToEnd();
            streamReader.Close();
            response.Close();

            Root data = JsonConvert.DeserializeObject<Root>(json);
            string logoUri = data.logos.Find(e => e.type == "icon").formats[0].src;

            string path= $"../../MainWindow/Assets/{resourceName}.png";
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(new Uri(logoUri), path);
            }   
        }
        public static string GetLogo(string resourceName)
        {
            string path= $"../../MainWindow/Assets/{resourceName}.png";
            if (!File.Exists(path))
            {
                try
                {
                    GetLogoFromAPI(resourceName);                    
                }
                catch (Exception)
                {
                    File.Copy($"../../MainWindow/Assets/defaultLogo.png", $"../../MainWindow/Assets/{resourceName}.png");
                }              
            }
            return path; 
        }
    }
}
