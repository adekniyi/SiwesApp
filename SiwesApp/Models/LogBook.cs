using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Models
{
    public class LogBook
    {
        public int LogBookId { get; set; }
        public int StudentId { get; set; }
        public DateTimeOffset Day { get; set; }
        public String Time { get; set; }
        public string Description { get; set; }
        public virtual Student Student { get; set; }
        public virtual List<Comment> Comment { get; set; }
        public virtual List<Grade> Grade { get; set; }
    }
}
