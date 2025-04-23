# ⚡ ElectricityLib - Power System Core for Vintage Story

![ElectricityLib](https://github.com/Sztacho/ElectricityLib-VS/blob/master/modicon.png)

**ElectricityLib** is an advanced library mod for [Vintage Story](https://www.vintagestory.at/), providing a flexible and efficient electricity system with support for:

- ✅ AC and DC power types
- 🔥 Resistance, voltage drop, and overload heat accumulation
- 🌡️ Overload protection with thermal capacity handling
- 🎯 Event system (Overload events, Packet propagation events)
- 🛠️ Debug tools and diagnostics
- 🧩 Example generator, conductor, and consumer implementations

> ⚠️ This is a **library mod (Core)**. It does not add blocks or content by itself.  
> It is designed to be used as a dependency for other mods that need power/electricity systems.

---

## 🚀 Features

- **Modular power system:**  
  Easily implement generators, wires, and machines by using provided interfaces.

- **Realistic overload logic:**  
  Overcurrent leads to heat buildup → overheating → possible destruction.

- **Events:**  
  Hook into network behavior (`OnConductorOverload`, `OnPacketPropagated`).

- **AC / DC support:**  
  Packet-based electricity with frequency handling for AC.

- **Helper tools:**  
  Electricity propagation, neighbor detection, diagnostics.

---

## 📦 Installation

1. Clone this repository or download the release.
2. Place the built DLL in your `Mods/` folder inside your Vintage Story installation.
3. Add this to your `modinfo.json` if you're using it as a dependency:
```json
"dependencies": [
  { "modid": "electricitylib", "version": "1.1.0" }
]
```

---

## 📐 API Overview

### Interfaces:
| Interface               | Purpose                                 |
|--------------------------|-----------------------------------------|
| `IElectricNode`          | Any element that participates in the network |
| `IElectricConductor`     | Wire/conductor that transmits power (with resistance, heat, max amps) |
| `IElectricGenerator`     | Generator that outputs electricity packets |
| `IElectricConsumer`      | Machine that consumes electricity |

---

## 💡 Example: Basic Generator and Lamp Setup

### Generator (`HandCrankGenerator`):
```csharp
public class ExampleHandCrankGenerator : BlockEntity, IElectricGenerator
{
    public ElectricityPacket GenerateElectricity()
    {
        return new ElectricityPacket(EnergyType.DC, 12f, 2f, 0);
    }

    public List<IElectricNode> GetConnectedNodes()
    {
        return ElectricityHelper.FindNeighbors(Api.World, Pos);
    }

    public void OnEnergyReceived(ElectricityPacket packet)
    {
        // Generators do not consume packets.
    }
}
```

---

### Consumer (`Electric Lamp`):
```csharp
public class ExampleElectricLamp : BlockEntity, IElectricConsumer
{
    private bool isPowered = false;

    public bool CanAccept(ElectricityPacket packet)
    {
        return packet.Voltage >= 5f;
    }

    public void OnEnergyReceived(ElectricityPacket packet)
    {
        isPowered = true;
        MarkDirty(true);
    }

    public override void OnGameTick(float dt)
    {
        base.OnGameTick(dt);
        isPowered = false; // Reset state each tick
    }

    public List<IElectricNode> GetConnectedNodes()
    {
        return ElectricityHelper.FindNeighbors(Api.World, Pos);
    }
}
```

---

## 🔥 Example: Custom Conductor with Overload Handling

```csharp
public class ExampleCopperWire : BlockEntity, IElectricConductor
{
    private float currentTemperature = 0f;

    public float ResistanceOhms => 0.05f;
    public float MaxAmperage => 5.0f;
    public float ThermalCapacity => 100f;
    public float CoolingRate => 0.05f;
    public float CurrentTemperature
    {
        get => currentTemperature;
        set => currentTemperature = value;
    }

    public void OnOverload()
    {
        Api.World.BlockAccessor.SetBlock(Api.World.GetBlock(new AssetLocation("yourmod", "burnedwire")).BlockId, Pos);
        Api.World.PlaySoundAt(new AssetLocation("sounds/electric_zap"), Pos.X, Pos.Y, Pos.Z);
    }

    public List<IElectricNode> GetConnectedNodes()
    {
        return ElectricityHelper.FindNeighbors(Api.World, Pos);
    }

    public void OnEnergyReceived(ElectricityPacket packet) { }
}
```

---

## 🛠️ Events Usage

You can hook into the network events:
```csharp
network.OnConductorOverload += (conductor) =>
{
    Logger.Notification($"Wire overload detected at conductor: {conductor}");
};

network.OnPacketPropagated += (node, packet) =>
{
    Logger.Debug($"Packet propagated to node: {node} with {packet.PowerWatts} W");
};
```

---

## 🧪 Diagnostics Example

You can build your own network diagnostics using the helper methods.  
**Coming soon: `NetworkDiagnostics.ValidateNetwork()`**

---

## 📜 License

MIT License.  
Feel free to use, modify, and contribute!

---

## 🤝 Contributing

1. Fork the project.
2. Create your feature branch (`git checkout -b feature/MyFeature`).
3. Commit your changes (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature/MyFeature`).
5. Create a new Pull Request.

---

## 🙏 Credits

ElectricityLib is developed with ❤️ by **Sztacho**  
Inspired by real-world electrical systems, and by modding communities of Minecraft and Vintage Story.