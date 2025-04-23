using System.Collections.Generic;
using ElectricityLib.Api;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace ElectricityLib.Util;

public static class ElectricityHelper
{
    public static List<IElectricNode> FindNeighbors(IWorldAccessor world, BlockPos pos)
    {
        var neighbors = new List<IElectricNode>();
        var directions = BlockFacing.ALLFACES;

        foreach (var face in directions)
        {
            var be = world.BlockAccessor.GetBlockEntity(pos.AddCopy(face));
            if (be is IElectricNode electricNode)
            {
                neighbors.Add(electricNode);
            }
        }

        return neighbors;
    }
}