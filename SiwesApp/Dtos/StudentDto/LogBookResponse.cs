using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Dtos.StudentDto
{
    public class LogBookResponse
    {
        public int LogBookId { get; set; }
        public DateTimeOffset Day { get; set; }
        public String Time { get; set; }
        public string Description { get; set; }
    }
}
