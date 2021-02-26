using OQ.MineBot.GUI.Protocol.Movement.Maps;
using OQ.MineBot.PluginBase.Base.Plugin.Tasks;
using OQ.MineBot.PluginBase.Movement.Maps;
using OQ.MineBot.PluginBase.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypixelWheatFarming
{
    class FarmingWheat : ITask, ITickListener
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
            600000, 1);

        private const int GROWN_WHEAT_METADATA = 7;
        private const int WHEAT_ID = 59;
        private const int SEED_ID = 295;

        public override bool Exec()
        {
            return !Context.Player.IsDead() &&
                !Context.Player.State.Eating && !Context.Player.State.EatRequestQueued &&
                !Inventory.IsFull() && Context.Containers.GetOpenWindow() == null;
        }

        public async Task OnTick()
        {
            var block = await Context.World.FindClosest(256, 32,
                WHEAT_ID, OQ.MineBot.PluginBase.Classes.World.CpuMode.Medium_Usage,
                consideredBlock => consideredBlock.GetMetadata() == GROWN_WHEAT_METADATA
                && !BLACKLIST.IsBlocked(Context, consideredBlock.GetLocation()));

            if (block == null)
                return;
            if ((await block.MoveTo(MO).Task).Result == OQ.MineBot.PluginBase.Movement.Events.MoveResultType.Completed)
            {
                var mineAction = await block.Dig();
                await mineAction.DigTask;

                if (await Inventory.Select(SEED_ID))
                    await block.PlaceAt();
            }
            else
            {
                BLACKLIST.AddToBlockCounter(Context, block.GetLocation());
            }
        }
    }
}
