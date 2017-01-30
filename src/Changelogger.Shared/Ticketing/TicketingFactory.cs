using System;
using Changelogger.Shared.Ticketing.Configuration;

namespace Changelogger.Shared.Ticketing
{
    public static class TicketingFactory
    {
        public static ITicketingIntegrator GetTicketingIntegrator(string ticketingSystem = null)
        {
            if (!string.IsNullOrWhiteSpace(ticketingSystem))
            {
                var config = TicketingTypeSection.GetConfig();
                var definition = config.TypeDefinitions[ticketingSystem.ToLowerInvariant()];

                if (definition == null)
                    return null;

                var typeToLoad = Type.GetType(definition.Value);
                return Activator.CreateInstance(typeToLoad) as ITicketingIntegrator;
            }
            return new DefaultTicketIntegration();
        }
    }
}
