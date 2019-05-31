using System;
using System.Collections.Generic;

namespace JobEventsElasticsearch.Data
{
    public partial class JobDb39
    {
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public string JobTitle { get; set; }
        public int StatusId { get; set; }

        public virtual EmployerProfileDb39 Employer { get; set; }
    }
}
