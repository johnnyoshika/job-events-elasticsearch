using JobEventsElasticsearch.Data;
using Nest;
using System;
using System.Linq;

namespace JobEventsElasticsearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Insert();
            Console.WriteLine("Hello World!");
        }

        static void Insert()
        {
            using (var context = new DataContext())
            {
                var start = new DateTime(2019, 1, 1);
                var events = context.ZStatJobEvent
                    .Where(e => e.StatDate >= start)
                    .Select(e => new JobEvent
                    {
                        Id = e.StatId,
                        At = e.StatDate,
                        StatTypeId = e.StatTypeId,
                        JobId = e.JobId,
                        EmployerId = e.EmployerId,
                        Source = e.Source2,
                        Medium = e.Medium,
                        Campaign = e.Campaign
                    }).ToArray();
            }

            var client = new ElasticClient(new Uri("http://127.0.0.1:9221"));
            var response = client.IndexMany(new[]
            {
                new Widget { Id = 1, Name = "Car" },
                new Widget { Id = 2, Name = "Plane" }
            }, "widgets");
        }
    }

    class Widget
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class JobEvent
    {
        public long Id { get; set; }
        public DateTime At { get; set; }
        public int StatTypeId { get; set; }
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public string Source { get; set; }
        public string Medium { get; set; }
        public string Campaign { get; set; }
    }
}
