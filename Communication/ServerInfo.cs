using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    class ServerInfo
    {
        public string Host { get; set; }
        public int PortChat { get; set; }
        public int PortClient { get; set; }
        public List<string> Clients;
    }
}
