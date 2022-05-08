using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public static class InternetService
    {
        public static bool IsConnectedToInternet
        {
            get => SendPingRequest();
        }

        private static bool SendPingRequest()
        {
            string host = "google.com";
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
    }
}
