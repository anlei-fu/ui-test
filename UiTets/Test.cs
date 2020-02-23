using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiTets
{
  public  class Test
    {
        public string Page { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get;  set; }

        [JsonIgnore]
        public string Rtf { get; set; }
    }
}
