namespace ElectricityLib.Api;

public interface IElectricConsumer : IElectricNode
{
    bool CanAccept(ElectricityPacket packet);
}