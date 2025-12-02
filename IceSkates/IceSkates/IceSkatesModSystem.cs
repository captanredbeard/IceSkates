using IceSkates.Config;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace IceSkates
{
    public class IceSkatesModSystem : ModSystem
    {
        public string ModId => Mod.Info.ModID;
        public ICoreAPI Api { get; private set; }
        public IceSkatesConfig Config { get; private set; }
        public static IceSkatesModSystem Instance { get; private set; }

        public ILogger Logger => Mod.Logger;
        private FileWatcher _fileWatcher;

        internal ICoreClientAPI capi;
        

        public override void Start(ICoreAPI api) {
            Instance = this;
            Api = api;
            
            

            
            ReloadConfig(api);
        }
    
        public override void StartServerSide(ICoreServerAPI api)
        {
            Mod.Logger.Notification("Hello from template mod server side: " + Lang.Get("iceskates:hello"));
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            Mod.Logger.Notification(Lang.Get("iceskates:hello"));
        }


        private void AddEntityBehaviors(Entity entity)
        {
            if (entity is not EntityPlayer) return;
            //if (entity.HasBehavior<EntityBehaviorDrag>()) return;
           // entity.AddBehavior(new EntityBehaviorDrag(entity));
        }

        private void Event_LevelFinalize()
        {
            // Hook server events for damage and fatigue
            //var ebs = capi.World.Player.Entity.GetBehavior<EntityBehaviorDrag>();
            if (ebs != null) ebs.OnFatigued += (ftg, ftgSource) => defenseSystem.HandleFatigued(capi.World.Player, ftg, ftgSource);
            capi.Logger.VerboseDebug("Done item defense stats");
        }

        public void ReloadConfig(ICoreAPI api)
        {
            (_fileWatcher ??= new FileWatcher()).Queued = true;

            try
            {
                var _config = api.LoadModConfig<IceSkatesConfig>($"{ModId}.json");
                if (_config == null)
                {
                    Mod.Logger.Warning("Missing config! Using default.");
                    Config = new IceSkatesConfig();
                    //Config = api.Assets.Get(new AssetLocation("bloodshed:config/default.json")).ToObject<BloodshedConfig>();
                    api.StoreModConfig(Config, $"{ModId}.json");
                }
                else
                {
                    Config = _config;
                }
            }
            catch (Exception ex)
            {
                Mod.Logger.Error($"Could not load {ModId} config!");
                Mod.Logger.Error(ex);
            }

            api.Event.RegisterCallback(_ => _fileWatcher.Queued = false, 100);
        }
       
    }
}
