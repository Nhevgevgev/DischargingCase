namespace DischargingCase;

public class PcsController
{
    public double ActivePower { get; private set; }
    
    private bool safetyOverride;

    public void Run(double soc, double siteLoad)
    {
        Console.WriteLine($"[INFO] Run - SoC: {soc}%, Site Load: {siteLoad} kW");

        if (safetyOverride)
        {
            Console.WriteLine("[WARN] Skipping dispatch: Discharge not permitted (Safety override active)\n");
            Dispatch(0);
            return;
        }

        if (soc > 20.0 && siteLoad > 1.0)
        {
            Dispatch(siteLoad);
        }
        else
        {
            Dispatch(0);
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
    }
}