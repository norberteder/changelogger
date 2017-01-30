using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using Changelogger.Shared.Entity;
using Changelogger.Shared.Ticketing;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Changelogger.Ticketing.TFS
{
    public class TFSTicketingIntegration : ITicketingIntegrator
    {
        private readonly string idPattern = "^#[0-9]*"; // works for: #1234: test
        private readonly string query = "SELECT[System.ID], [System.Title], [System.ChangedDate], [System.CreatedDate], [Type], [Effort] FROM WorkItems Where [System.ID] = {0}";

        private WorkItemStore Store { get; set; }
        private TfsTeamProjectCollection Collection { get; set; }
        private Uri Link { get; set; }

        public TFSTicketingIntegration()
        {
            Init();
        }

        public IEnumerable<TicketDescriptor> CombineLogsWithTicketing(List<LogDescriptor> logs)
        {
            List<TicketDescriptor> descriptors = new List<TicketDescriptor>();

            Regex regex = new Regex(idPattern, RegexOptions.IgnoreCase);
            foreach (var log in logs)
            {
                var matches = regex.Matches(log.Message);
                if (matches.Count > 0)
                {
                    int id = 0;
                    if (Int32.TryParse(matches[0].Value.Replace("#", ""), out id))
                    {
                        var foundItem = GetWorkItem(id);
                        if (foundItem != null)
                        {
                            if (descriptors.All(item => item.Id != id.ToString()))
                            {
                                Uri link = new Uri(Link, "AutomationX/_workitems/edit/" + id);
                                descriptors.Add(new TicketDescriptor() {Id = id.ToString(), Version = log.Tag, Title = foundItem.Title, Description = foundItem.Description, Link = link});
                            }
                        }
                    }
                }
            }
            return descriptors;
        }

        private void Init()
        {
            var collection = ConfigurationManager.AppSettings.Get("Collection");
            Link = new Uri(collection);

            Collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(Link);

            Store = Collection.GetService<WorkItemStore>();
        }

        private WorkItem GetWorkItem(int id)
        {
            var idQuery = string.Format(query, id);
            var items = Store.Query(idQuery);

            if (items.Count > 0)
            {
                return items[0];
            }
            return null;
        }
    }
}