using OQ.MineBot.PluginBase.Base;
using OQ.MineBot.PluginBase.Base.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypixelWheatFarming
{
    [Plugin(1, "Hypixel Wheat Farming", "Farms wheat on hypixel skyblock")]
    public class Class1 : IStartPlugin
    {
        public override void OnLoad(int version, int subversion, int buildversion)
        {
            //Register Tasks
            RegisterTask(new FarmingWheat());
            RegisterTask(new StoreInClosestChest());
            //Register Settings
            this.Setting.Add(new NumberSetting("Farming Radius", "How far the bot should look in order to farm.", 10, 0, 20));
        }
    }
}
