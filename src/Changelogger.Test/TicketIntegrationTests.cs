using Changelogger.Shared.Ticketing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Changelogger.Test
{
    [TestClass]
    public class TicketIntegrationTests
    {
        [TestMethod]
        public void Get_DefaultTicketIntegration_ShouldReturnDefaultIntegration()
        {
            var integration = TicketingFactory.GetTicketingIntegrator();
            Assert.IsNotNull(integration);
            Assert.AreEqual(typeof(DefaultTicketIntegration).FullName, integration.GetType().FullName);
        }

        [TestMethod]
        public void Get_UndefinedTicketIntegration_ShouldReturnNull()
        {
            var integration = TicketingFactory.GetTicketingIntegrator("SuperDefactSystem");
            Assert.IsNull(integration);
        }
    }
}
