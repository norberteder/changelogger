using System;
using System.ComponentModel;

namespace Changelogger.Shared.Entity
{
    public class LogDescriptor
    {
        public Version Tag { get;  set; }
        public string Hash { get; set; }
        public string Message { get; set; }
    }
}
