using System;
using System.Collections.Generic;

namespace JobEventsElasticsearch.Data
{
    public partial class ZStatJobEvent
    {
        public long StatId { get; set; }
        public DateTime StatDate { get; set; }
        public int JobId { get; set; }
        public int StatTypeId { get; set; }
        public int EmployerId { get; set; }
        public int? AgentId { get; set; }
        public string IpAddress { get; set; }
        public int? PositionTypeId { get; set; }
        public int? JobseekerId { get; set; }
        public int? WebsiteId { get; set; }
        public long? SessionId2 { get; set; }
        public string Source2 { get; set; }
        public string Medium { get; set; }
        public string Campaign { get; set; }
    }
}
