using System;
using System.Collections.Generic;

namespace JobEventsElasticsearch.Data
{
    public partial class EmployerProfileDb39
    {
        public EmployerProfileDb39()
        {
            JobDb39 = new HashSet<JobDb39>();
        }

        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public bool Suspended { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<JobDb39> JobDb39 { get; set; }
    }
}
