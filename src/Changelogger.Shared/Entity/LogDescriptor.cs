using System;
using System.ComponentModel;

namespace Changelogger.Shared.Entity
{
    public class LogDescriptor
    {
        public string Tag { get;  set; }
        public string Hash { get; set; }
        public string Message { get; set; }
    }
}
