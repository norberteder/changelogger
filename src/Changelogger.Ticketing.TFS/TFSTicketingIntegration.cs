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
        private readonly string query = "SELECT * FROM WorkItems Where [System.ID] = {0}";

        private WorkItemStore Store { get; set; }
        private TfsTeamProjectCollection Collection { get; set; }
        private string Link { get; set; }
        private string IdPattern { get; set; }
        private string TitleFormat { get; set; } // e.g. "[{id}] {title}"
        private List<string> fields = new List<string>();

        public TFSTicketingIntegration()
        {
            Init();
        }

        public IEnumerable<TicketDescriptor> CombineLogsWithTicketing(List<LogDescriptor> logs)
        {
            List<TicketDescriptor> descriptors = new List<TicketDescriptor>();

            Regex regex = new Regex(IdPattern, RegexOptions.IgnoreCase);
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
                                var title = TitleFormat;

                                if (fields.Count > 0)
                                {
                                    foreach(var field in fields)
                                    {
                                        title = title.Replace("{" + field + "}", foundItem[field].ToString());
                                    }
                                }
                                else
                                {
                                    title = foundItem.Title;
                                }

                                string link = Link + id;
                                descriptors.Add(new TicketDescriptor() {Id = id.ToString(), Version = log.Tag, Title = title, Description = foundItem.Description, Link = link});
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

            Link = ConfigurationManager.AppSettings.Get("Link");
            IdPattern = ConfigurationManager.AppSettings.Get("IdPattern");
            TitleFormat = ConfigurationManager.AppSettings.Get("TitleFormat");

            ExtractFields();

            Collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(collection));

            Store = Collection.GetService<WorkItemStore>();
        }

        private void ExtractFields()
        {
            Regex regex = new Regex("{\\w*}", RegexOptions.IgnoreCase);
            var matches = regex.Matches(TitleFormat);
            for (int i = 0; i < matches.Count; i++)
            {
                fields.Add(matches[i].Value.Replace("{", "").Replace("}", ""));
            }
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