using ElectricityLib.Api;

namespace ElectricityLib.Events;

public static class ElectricityEvents
{
    public delegate void ConductorOverloadHandler(IElectricConductor conductor);
    public delegate void PacketPropagatedHandler(IElectricNode node, ElectricityPacket packet);
}