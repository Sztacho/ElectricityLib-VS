using System.Collections.Generic;

namespace ElectricityLib.Api;

public interface IElectricNode
{
    List<IElectricNode> GetConnectedNodes();
    void OnEnergyReceived(ElectricityPacket packet);
}