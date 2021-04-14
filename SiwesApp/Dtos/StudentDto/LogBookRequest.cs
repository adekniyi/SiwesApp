using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.StudentDto
{
    public class LogBookRequest
    {
        public DateTimeOffset Day { get; set; }
        public String Time { get; set; }
        public string Description { get; set; }
    }
}
