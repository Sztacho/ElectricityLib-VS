namespace ElectricityLib.Api;

public interface IElectricConductor : IElectricNode
{
    float ResistanceOhms { get; }
    float MaxAmperage { get; }
    float ThermalCapacity { get; }
    float CoolingRate { get; }
    float CurrentTemperature { get; set; }

    void OnOverload();
}