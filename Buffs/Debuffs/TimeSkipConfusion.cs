using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class TimeSkipConfusion : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Time has skipped?");
            Description.SetDefault("What you just saw was your future self.");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity = Vector2.Zero;
            npc.AddBuff(BuffID.Confused, 2);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity = Vector2.Zero;
            player.AddBuff(BuffID.Confused, 2);
        }
    }
}