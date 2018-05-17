
namespace Novell.Samples
{


    class GetInternalIp
    {


        public static string GetLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType _type)
        {
            System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            var ipgp = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();

            string output = "";
            foreach (System.Net.NetworkInformation.NetworkInterface item in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                var interfaceProperties = item.GetIPProperties();

                // if (!string.Equals(interfaceProperties.DnsSuffix, ipgp.DomainName, System.StringComparison.InvariantCultureIgnoreCase))
                // continue;

                if (interfaceProperties.DnsAddresses == null || interfaceProperties.DnsAddresses.Count < 1)
                    continue;

                if (interfaceProperties.DnsAddresses[0].AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                    continue;

                if (item.NetworkInterfaceType == _type && item.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                {
                    foreach (System.Net.NetworkInformation.UnicastIPAddressInformation ip in interfaceProperties.UnicastAddresses)
                    {
                        if (!ip.IsDnsEligible)
                            continue;

                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }

        public static string GetLocalIPAddress()
        {
            System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new System.Exception("No network adapters with an IPv4 address in the system!");
        }


        public static void ReliableIP()
        {
            string localIP;
            using (System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, 0))
            {
                //socket.Connect("8.8.8.8", 65530);

                // FROM DNS
                socket.Connect("192.168.115.2", 65530);

                System.Net.IPEndPoint endPoint = socket.LocalEndPoint as System.Net.IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
        }


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            ReliableIP();
            GetLocalIPAddress();
            GetLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet);

        }


    }


}
