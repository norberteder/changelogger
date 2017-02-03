using System;

namespace Changelogger.Shared.Entity
{
    public class TicketDescriptor
    {
        public string Id { get; set; }
        public Version Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReleaseNotes { get; set; }
        public string State { get; set; }
        public string Link { get; set; }
    }
}
