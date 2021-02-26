using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OQ.MineBot.PluginBase.Base;
using OQ.MineBot.PluginBase.Base.Plugin;

namespace HypixelFarmingNetherwart
{
    [Plugin(1, "Hypixel netherwart Farmer", "Farming netherwart for hypixel skyblock")]
    public class Class1 : IStartPlugin
    {
        public override void OnLoad(int version, int subversion, int buildversion)
        {
            this.Setting.Add(new NumberSetting("Look around", "How far around you want the bot to look for netherwart", 10, 0, 20));
        }
        public override void OnStart()
        {
            RegisterTask(new MyTask());
            RegisterTask(new StoreTask());
        }
    }



}
