using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttachmentDumper
{
    public class ConfigurationSet
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] FileExtensions { get; set; }
        public string OutputDirectory { get; set; }
    }
}
