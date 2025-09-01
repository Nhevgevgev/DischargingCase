namespace DischargingCase.Tests
{
    [TestClass]
    public sealed class PcsControllerTests
    {
        private PcsController _controller = null!;
        private const int SoCToDispatch = 50;
        private const int SoCNotToDispatch = 10;
        private const int SiteLoadToDispatch = 10;
        private const int SiteLoadToNotDispatch = 0;
        private const int TemperatureToLog = 36;
        private const int ActivePowerZero = 0;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new PcsController();
        }

        [TestMethod]
        public void Run_Dispatches_WhenSocAndLoadAndTemperatureAreSufficientAndSafetyOverride()
        {
            _controller.Run(SoCToDispatch, SiteLoadToDispatch, TemperatureToLog);

            Assert.AreEqual(SiteLoadToDispatch, _controller.ActivePower);
        }

        [TestMethod]
        public void Run_DoesNotDispatche_WhenSocIsBelow20PercentAndLoadAndTemperatureAreSufficient()
        {
            _controller.Run(SoCNotToDispatch, SiteLoadToDispatch, TemperatureToLog);

            Assert.AreEqual(ActivePowerZero, _controller.ActivePower);
        }

        [TestMethod]
        public void Run_DoesNotDispatche_WhenLoadIsZeroAndSocAndTemperatureAreSufficient()
        {
            _controller.Run(SoCToDispatch, SiteLoadToNotDispatch, TemperatureToLog);

            Assert.AreEqual(SiteLoadToNotDispatch, _controller.ActivePower);
        }
    }
}
