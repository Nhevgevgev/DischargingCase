namespace DischargingCase;

/// <summary>
/// Represents a controller for managing the operation of a battery energy storage system.
/// </summary>
/// <remarks>The <see cref="PcsController"/> class provides functionality to control the battery's active power 
/// output based on the state of charge, site load. It also includes mechanisms  for safety
/// overrides to prevent unsafe operations.</remarks>
public class PcsController
{
    /// <summary>
    /// Battery's active power to dispatch in kilowatt.
    /// </summary>
    public double ActivePower { get; private set; }

    /// <summary>
    /// Indicates whether the safety override mode is enabled.
    /// </summary>
    private bool safetyOverride;

    /// <summary>
    /// Runs the controller logic to determine the battery's active power output.
    /// </summary>
    /// <param name="soc">Battery's state of charge in percent.</param>
    /// <param name="siteLoad">Site load in kilowatt.</param>
    /// <param name="batteryTemperature">Battery's temperature in degrees Celsius.</param>
    public void Run(double soc, double siteLoad, double batteryTemperature)
    {
        // Added the Battery Temperature information to the log message for better context.
        Console.WriteLine($"[INFO] Run - SoC: {soc}%, Site Load: {siteLoad} kW, Battery Temperature: {batteryTemperature:F1}°C.");

        if (safetyOverride)
        {
            Console.WriteLine("[WARN] Skipping dispatch: Discharge not permitted (Safety override active)\n");
            // Removed the call of Dispatch(0) to avoid unnecessary log messages and improve application's efficiency.
            // Disable the Safety Override Mode every time after being enabled once.
            safetyOverride = false;
            return;
        }

        // Changed Site Load limit from 1.0 to 0 in order to dispatch also for 1 kW load.
        if (soc > 20.0 && siteLoad > 0)
        {
            Dispatch(siteLoad);
        }
        else
        {
            // Removed the call of Dispatch(0) to avoid unnecessary log messages and improve application's efficiency.
            Console.WriteLine("[INFO] Dispatch not required (SoC or load too low)\n");
        }
    }

    private void Dispatch(double power)
    {
        ActivePower = power;
        Console.WriteLine($"[DISPATCH] Battery discharging at {power:F1} kW\n");
    }

    /// <summary>
    /// Activates the safety override mode and logs a warning message indicating that the battery temperature exceeds the
    /// safe threshold.
    /// </summary>
    /// <remarks>This method sets the safety override to <see langword="true"/> and outputs a warning message
    /// to the console.  The warning message includes the current state of the safety override and indicates that the
    /// battery temperature  has exceeded 40°C. This method is intended to be used in scenarios where immediate action
    /// is required to address  unsafe battery conditions.</remarks>
    public void SendSafetySignal()
    {
        safetyOverride = true;
        // Added the warning message about the battery temperature information and Safety Override Mode status for better context.
        Console.WriteLine($"[WARN] Battery's temperature is more than 40°C. Safety Override Mode active: {safetyOverride} \n");
    }
}