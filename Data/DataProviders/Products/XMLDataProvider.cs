using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UsersLibrary;
using UsersLibrary.Services;

namespace Data.DataProviders.Products
{
    public class XMLDataProvider : IDataProvider
    {
        private readonly string _filePath = "{0}.xml";

        public void Save(User user, Service service)
        {
            CryptedService cryptedService = user.EncryptService(service);

            XDocument xdoc = new XDocument();
            XElement root = new XElement("services");
            xdoc.Add(root);

            if (File.Exists(string.Format(_filePath, user.HashEmail)))
            {
                xdoc = XDocument.Load(string.Format(_filePath, user.HashEmail));
                root = xdoc.Element("services");
            }

            if (root != null)
                root.Add(GetXElement(cryptedService));

            xdoc.Save(string.Format(_filePath, user.HashEmail));
        }
        public void Save(User user, IEnumerable<Service> services)
        {
            XDocument xdoc = new XDocument();
            XElement root = new XElement("services");
            xdoc.Add(root);

            if (File.Exists(string.Format(_filePath, user.HashEmail)))
            {
                xdoc = XDocument.Load(string.Format(_filePath, user.HashEmail));
                root = xdoc.Element("services");
            }

            foreach (Service service in services)
            {
                CryptedService cryptedService = user.EncryptService(service);

                if (root != null)
                    root.Add(GetXElement(cryptedService));
            }

            xdoc.Save(string.Format(_filePath, user.HashEmail));
        }

        private XElement GetXElement(CryptedService cryptedService)
        {
            XElement serviceElement = new XElement("service");
            XAttribute serviceIdAttribute = new XAttribute("id", $"{cryptedService.Id}");
            XElement serviceNameAttribute = new XElement("name", $"{cryptedService.Name}");
            XElement serviceLoginAttribute = new XElement("login", $"{cryptedService.Login}");
            XElement servicePasswordAttribute = new XElement("password", $"{cryptedService.Password}");

            serviceElement.Add(serviceIdAttribute, serviceNameAttribute, serviceLoginAttribute, servicePasswordAttribute);
            return serviceElement;
        }

        public void Delete(User user, Service service)
        {
            if (!File.Exists(string.Format(_filePath, user.HashEmail))) return;

            XDocument xdoc = XDocument.Load(string.Format(_filePath, user.HashEmail));
            XElement root = xdoc.Element("services");

            if (root != null)
            {
                var serviceElement = root.Elements("service").FirstOrDefault(p => p.Attribute("id")?.Value == service.Id.ToString());

                if (serviceElement != null)
                {
                    serviceElement.Remove();
                    xdoc.Save(string.Format(_filePath, user.HashEmail));
                }
            }
        }
        public void Delete(User user, int id)
        {
            if (!File.Exists(string.Format(_filePath, user.HashEmail))) return;

            XDocument xdoc = XDocument.Load(string.Format(_filePath, user.HashEmail));
            XElement root = xdoc.Element("services");

            if (root != null)
            {
                var serviceElement = root.Elements("service").First(p => p.Attribute("id")?.Value == id.ToString());

                if (serviceElement != null)
                {
                    serviceElement.Remove();
                    xdoc.Save(string.Format(_filePath, user.HashEmail));
                }
            }
        }

        public void Update(User user, int id, Service service)
        {
            if(!File.Exists(string.Format(_filePath, user.HashEmail))) return;

            CryptedService cryptedService = user.EncryptService(service);

            XDocument xdoc = XDocument.Load(string.Format(_filePath, user.HashEmail));
            XElement root = xdoc.Element("services");

            XElement findService = root.Elements().First(e => e.Attribute("id").Value == id.ToString());

            if (findService != null)
            {
                findService.Element("name").Value = cryptedService.Name;
                findService.Element("login").Value = cryptedService.Login;
                findService.Element("password").Value = cryptedService.Password;
            }

            xdoc.Save(string.Format(_filePath, user.HashEmail));
        }
        public void Update(User user, Service startService, Service finishService) => Update(user, startService.Id, finishService);

        public List<Service> Load(User user)
        {
            if (!File.Exists(string.Format(_filePath, user.HashEmail))) return new List<Service>();

            XDocument xdoc = XDocument.Load(string.Format(_filePath, user.HashEmail));
            XElement root = xdoc.Element("services");

            if (root != null)
                return root.Elements("service")
                .Select(c => user.DecryptService(
                    new CryptedService(c.Element("name").Value, c.Element("login").Value, c.Element("password").Value)
                    { Id = Convert.ToInt32(c.Attribute("id").Value) })).ToList();

            return new List<Service>();
        }

        public void Clear(User user)
        {
            if (!File.Exists(string.Format(_filePath, user.HashEmail))) return;

            XDocument xdoc = XDocument.Load(string.Format(_filePath, user.HashEmail));
            XElement root = xdoc.Element("services");
            if (root != null)
                root.RemoveAll();

            xdoc.Save(string.Format(_filePath, user.HashEmail));
        }

        public void DeleteFile(User user)
        {
            if (!File.Exists(string.Format(_filePath, user.HashEmail))) return;

            File.Delete(string.Format(_filePath, user.HashEmail));
        }

        public DateTime GetLastModifyTime(User user)
        {
            if (!File.Exists(string.Format(_filePath, user.HashEmail))) return DateTime.MinValue;

            return File.GetLastWriteTime(string.Format(_filePath, user.HashEmail));
        }
    }
}
