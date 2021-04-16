using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int CommentterId { get; set; }
        public string CommentDetail { get; set; }
        public int LogBookId { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public virtual IndustrialSupervisor IndustrialSupervisor { get; set; }
        public virtual LogBook LogBook { get; set; }

    }
}
