using Dapper;
using Nest;
using System;
using System.Data.SqlClient;
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
            using (var connection = new SqlConnection("Server=.;Database=jobcentre_production;Integrated Security=SSPI;MultipleActiveResultSets=true"))
            {
                int offset = 0;
                int next = 50000;
                while (true)
                {
                    var events = connection.Query<JobEvent>(@"
                        SELECT e.[StatID] as [Id]
                                ,e.[StatDate] as [EventAt]
                                ,e.[StatTypeID]
                                ,e.[EmployerID]
	                            ,ep.[CompanyName]
                                ,e.[JobId]
	                            ,j.[JobTitle]
                                ,e.[Source2] as [Source]
                                ,e.[Medium]
                                ,e.[Campaign]
                        FROM [dbo].[z_Stat_JobEvent] as e
	                        LEFT JOIN [dbo].[EmployerProfileDB39] as ep on ep.EmployerID = e.EmployerID
	                        LEFT JOIN  [dbo].[JobDB39] as j on j.JobID = e.JobId
                        WHERE e.[StatTypeID] IN (7,10,18,22) AND e.[StatDate] >= @after
                        ORDER BY e.[StatID]
                        OFFSET @offset ROWS
                        FETCH NEXT @next ROWS ONLY
                    ", new { after = "2019-01-01", offset, next });

                    if (events.Count() < 1)
                        break;

                    var client = new ElasticClient(new Uri("http://127.0.0.1:9221"));
                    var response = client.IndexMany(events, "jobevents");

                    offset = offset + next;
                }
            }
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
        public DateTime EventAt { get; set; }
        public int StatTypeId { get; set; }
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string Source { get; set; }
        public string Medium { get; set; }
        public string Campaign { get; set; }
    }
}
