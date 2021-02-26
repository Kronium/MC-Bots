using OQ.MineBot.GUI.Protocol.Movement.Maps;
using OQ.MineBot.PluginBase.Base.Plugin.Tasks;
using OQ.MineBot.PluginBase.Movement.Maps;
using OQ.MineBot.PluginBase.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypixelFarmingNetherwart
{
    class MyTask : ITask, ITickListener
    {
        private static MapOptions MO = new MapOptions()
        {
            AdditionalWeights = new MapOptionWeights()
            {
                JumpGap = 25,
                JumpUp = 25
            }
        };
        private static LocationBlacklistCollection BLACKLIST = LocationBlacklistCollection.CreateGlobal(3,
            60000, 1);
        
        private const int GROWN_NETHERWART_METADATA = 3;
        private const int NETHERWART_ID = 115;
        private const int NETHERWART_SEED_ID = 372;

        public override bool Exec()
        {
            return !Context.Player.IsDead() &&
                !Context.Player.State.Eating && !Context.Player.State.EatRequestQueued &&
                !Inventory.IsFull() && Context.Containers.GetOpenWindow() == null;
        }

        public async Task OnTick()
        {
            var block = await Context.World.FindClosest(128, 32,
                NETHERWART_ID, OQ.MineBot.PluginBase.Classes.World.CpuMode.Medium_Usage,
                consideredBlock => consideredBlock.GetMetadata() == GROWN_NETHERWART_METADATA
                && !BLACKLIST.IsBlocked(Context, consideredBlock.GetLocation()));

            if (block == null)
                return;
            if ((await block.MoveTo(MO).Task).Result == OQ.MineBot.PluginBase.Movement.Events.MoveResultType.Completed)
            {
                var mineAction = await block.Dig();
                await mineAction.DigTask;
            }
            else
            {
                BLACKLIST.AddToBlockCounter(Context, block.GetLocation());
            }
        }
    }
}
