using System;

namespace Changelogger.Git.Entity
{
    public class VersionTagRange
    {
        public Version VersionFrom { get; set; }
        public Version VersionTo { get; set; }

        public bool FromToAreEqual
        {
            get
            {
                return VersionFrom == VersionTo;
            }
        }
    }
}
