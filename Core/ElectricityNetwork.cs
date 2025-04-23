using System;
using System.Collections.Generic;
using ElectricityLib.Api;

namespace ElectricityLib.Core;

public class ElectricityNetwork
    {
        private readonly HashSet<IElectricNode> _nodes = new();
        private bool _dirty = true;

        public void AddNode(IElectricNode node)
        {
            _nodes.Add(node);
            _dirty = true;
        }

        public void RemoveNode(IElectricNode node)
        {
            _nodes.Remove(node);
            _dirty = true;
        }

        public void UpdateNetwork()
        {
            if (!_dirty) return;
            _dirty = false;
        }

        public void PropagateElectricity()
        {
            UpdateNetwork();

            foreach (var node in _nodes)
            {
                if (node is not IElectricGenerator generator) continue;
                var packet = generator.GenerateElectricity();
                DistributePower(generator, packet);
            }
        }

        private void DistributePower(IElectricNode source, ElectricityPacket output)
        {
            var queue = new Queue<PropagationEntry>();
            var visited = new HashSet<IElectricNode>();

            queue.Enqueue(new PropagationEntry(source, output));
            visited.Add(source);

            while (queue.Count > 0)
            {
                var entry = queue.Dequeue();
                var currentNode = entry.Node;
                var currentPacket = entry.Packet;

                foreach (var neighbor in currentNode.GetConnectedNodes())
                {
                    if (!visited.Add(neighbor)) continue;

                    var nextPacket = currentPacket;

                    switch (neighbor)
                    {
                        case IElectricConductor conductor:
                        {
                            var voltageDrop = currentPacket.Amperage * conductor.ResistanceOhms;
                            var newVoltage = Math.Max(0, currentPacket.Voltage - voltageDrop);
                            nextPacket = new ElectricityPacket(
                                currentPacket.Type,
                                newVoltage,
                                currentPacket.Amperage,
                                currentPacket.Frequency
                            );

                            var overload = currentPacket.Amperage - conductor.MaxAmperage;
                            if (overload > 0)
                            {
                                conductor.CurrentTemperature += overload * overload * 0.1f;
                                if (conductor.CurrentTemperature >= conductor.ThermalCapacity)
                                {
                                    conductor.OnOverload();
                                    continue;
                                }
                            }
                            else
                            {
                                conductor.CurrentTemperature -= conductor.CurrentTemperature * conductor.CoolingRate;
                                if (conductor.CurrentTemperature < 0)
                                    conductor.CurrentTemperature = 0;
                            }

                            break;
                        }
                        case IElectricConsumer consumer when consumer.CanAccept(nextPacket):
                            consumer.OnEnergyReceived(nextPacket);
                            break;
                    }

                    queue.Enqueue(new PropagationEntry(neighbor, nextPacket));
                }
            }
        }

        private class PropagationEntry
        {
            public IElectricNode Node { get; }
            public ElectricityPacket Packet { get; }

            public PropagationEntry(IElectricNode node, ElectricityPacket packet)
            {
                Node = node;
                Packet = packet;
            }
        }
    }