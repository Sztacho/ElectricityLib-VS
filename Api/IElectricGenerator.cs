namespace ElectricityLib.Api;

public interface IElectricGenerator : IElectricNode
{
    ElectricityPacket GenerateElectricity();
}