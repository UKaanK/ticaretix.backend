using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticaretix.Infrastructure.Configuration
{
    public class RabbitMQClientConfiguration
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public List<string> Hostnames { get; set; }
    }
}
