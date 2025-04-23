namespace ElectricityLib.Api;

public class ElectricityPacket
{
    public EnergyType Type { get; }
    public float Voltage { get; set; }
    public float Amperage { get; set; }
    public float Frequency { get; set; }

    public ElectricityPacket(EnergyType type, float voltage, float amperage, float frequency)
    {
        Type = type;
        Voltage = voltage;
        Amperage = amperage;
        Frequency = frequency;
    }

    public float PowerWatts => Voltage * Amperage;
}