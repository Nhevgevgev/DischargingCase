// See https://aka.ms/new-console-template for more information

using DischargingCase;

var controller = new PcsController();
var random = new Random();
const double namePlateCapacityWattSeconds = 3_6000_000d;
var wattSecondsInBattery = namePlateCapacityWattSeconds; // Start with a full battery of 10kWh
var startTemperature = 35d;

Console.WriteLine("Controller restarted...");
Console.WriteLine("Press Ctrl+C to stop.\n");

while (true)
{
    wattSecondsInBattery -= controller.ActivePower;
    var soc = wattSecondsInBattery / namePlateCapacityWattSeconds * 100.0;
    var siteLoad = Math.Round(random.NextDouble() * 15); // Random site load between 0 and 15 kW
    
    startTemperature += random.NextDouble() * 2 - 1; // Random temperature fluctuation between -1 and +1 degree
    
    if (startTemperature > 40)
    {
        controller.SendSafetySignal();
    }

    controller.Run(soc, siteLoad, startTemperature);

    Thread.Sleep(1000); // 1-second tick
}