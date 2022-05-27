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
    }
}
