using System.Configuration;

namespace Changelogger.Shared.Ticketing.Configuration
{
    public class TypeDefinition : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key => this["key"] as string;
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value => this["value"] as string;
    }
}
