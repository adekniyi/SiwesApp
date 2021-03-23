using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.All
{
    public class ToRespond
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object ObjectValue { get; set; }
    }
}
