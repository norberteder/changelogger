using System.Configuration;

namespace Changelogger.Shared.Ticketing.Configuration
{
    public class TicketingTypeSection : ConfigurationSection
    {
        public static TicketingTypeSection GetConfig()
        {
            return ConfigurationManager.GetSection("TicketingTypeSection") as TicketingTypeSection ?? new TicketingTypeSection();
        }

        [ConfigurationProperty("types")]
        public TypeDefinitions TypeDefinitions
        {
            get
            {
                return this["types"] as TypeDefinitions ?? new TypeDefinitions();
            }
        }
    }
}
