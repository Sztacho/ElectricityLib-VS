using Vintagestory.API.Common;
using Vintagestory.API.Server;
using ElectricityLib.Core;

namespace ElectricityLib
{
    public class ElectricityLibModSystem : ModSystem
    {
        /// <summary>
        /// Singleton dostępny dla innych modów.
        /// </summary>
        public static ElectricityNetwork GlobalNetwork { get; private set; }

        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.Logger.Notification("[ElectricityLib] Library loaded successfully.");
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);

            GlobalNetwork = new ElectricityNetwork();

            api.Logger.Notification("[ElectricityLib] Server-side initialized. Core library ready.");
        }

        public override double ExecuteOrder()
        {
            return 0.1;
        }
    }
}