using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StoneFree
{
    public class StoneFreeStandT1 : StandClass
    {
        public override float maxDistance => 98f;
        public override int punchDamage => 16;
        public override int punchTime => 11;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 0f;
        public override string spawnSoundName => "Stone Free";
        public override StandType standType => StandType.Melee;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    StayBehind();
                }
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                PlayAnimation("Idle");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/StoneFree/StoneFree_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 60, true);
            }
        }
    }
}