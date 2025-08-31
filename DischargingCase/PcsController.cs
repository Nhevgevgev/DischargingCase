namespace DischargingCase;

/// <summary>
/// Represents a controller for managing the operation of a battery energy storage system.
/// </summary>
/// <remarks>The <see cref="PcsController"/> class provides functionality to control the battery's active power 
/// output based on the state of charge, site load, and battery temperature. It also includes mechanisms  for safety
/// overrides to prevent unsafe operations.</remarks>
public class PcsController
{
    /// <summary>
    /// Battery's active power in kilowatt.
    /// </summary>
    public double ActivePower { get; private set; }

    private bool safetyOverride;

    /// <summary>
    /// Runs the controller logic to determine the battery's active power output.
    /// </summary>
    /// <param name="soc">Battery's state of charge in percent.</param>
    /// <param name="siteLoad">Site load in kilowatt.</param>
    /// <param name="batteryTemperature">Battery's temperature in degree Celsius.</param>
    public void Run(double soc, double siteLoad, double batteryTemperature)
    {
        Console.WriteLine($"[INFO] Run - SoC: {soc}%, Site Load: {siteLoad} kW, Battery Temperature: {batteryTemperature}°C.");

        if (safetyOverride)
        {
            Console.WriteLine("[WARN] Skipping dispatch: Discharge not permitted (Safety override active)\n");
            safetyOverride = false;
            return;
        }

        if (soc > 20.0 && siteLoad > 0)
        {
            Dispatch(siteLoad);
        }
        else
        {
            Console.WriteLine("[INFO] Dispatch not required (SoC or load too low)\n");
        }
    }

    private void Dispatch(double power)
    {
        ActivePower = power;
        Console.WriteLine($"[DISPATCH] Battery discharging at {power:F1} kW\n");
    }

    public void SendSafetySignal()
    {
        safetyOverride = true;
        Console.WriteLine($"[WARN] Battery temperature more than 40°C \n");
    }
}