using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devops_windows
{
    public class IndexModel
    {
        public string Host { get; set; }
        public string OS { get; set; }
        public string Framework { get; set; }

        public string[] Quotes { get; set; }

    }
}