using System.Configuration;

namespace Changelogger.Shared.Ticketing.Configuration
{
    public class TypeDefinitions : ConfigurationElementCollection
    {
        public TypeDefinition this[int index] => BaseGet(index) as TypeDefinition;

        public new TypeDefinition this[string key] => BaseGet(key) as TypeDefinition;

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as TypeDefinition).Key;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeDefinition();
        }
    }
}
