using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TODO.Entities
{
    public class TODOTask
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Category { get; set; }
        public DateTime StartDate { get; set; }
        public  Nullable<DateTime> EndDate { get; set; }
        public bool IsAutoClose { get; set; }
    }
}
