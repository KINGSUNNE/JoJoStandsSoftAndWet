using Microsoft.Xna.Framework;
using Terraria;

namespace JoJoStands.Projectiles.PlayerStands.MagiciansRed
{
    public class MagiciansRedStandT2 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 38;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override float shootSpeed => 8f;
        public override int projectileDamage => 48;
        public override int shootTime => 18;
        public override int halfStandHeight => 35;
        public override int standOffset => 0;

        public int chanceToDebuff = 35;
        public int debuffDuration = 360;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (!attackFrames)
                StayBehind();
            else
                GoInFront();

            if (player.ownedProjectileCounts[mod.ProjectileType("RedBind")] == 0)
            {
                secondaryAbilityFrames = false;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && player.ownedProjectileCounts[mod.ProjectileType("RedBind")] == 0)
                {
                    attackFrames = true;
                    Main.mouseRight = false;
                    projectile.netUpdate = true;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("FireAnkh"), newProjectileDamage, 3f, Main.myPlayer, chanceToDebuff, debuffDuration);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        normalFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer && player.ownedProjectileCounts[mod.ProjectileType("RedBind")] == 0)
                {
                    secondaryAbilityFrames = true;
                    Main.mouseLeft = false;
                    projectile.netUpdate = true;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= 16f;
                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("RedBind"), newProjectileDamage, 3f, Main.myPlayer, projectile.whoAmI, debuffDuration - 60);
                    Main.projectile[proj].netUpdate = true;
                }
            }
            if (modPlayer.StandAutoMode)
            {
                NPC target = null;
                Vector2 targetPos = projectile.position;
                float targetDist = 350f;
                if (target == null)
                {
                    for (int k = 0; k < 200; k++)       //the targeting system
                    {
                        NPC npc = Main.npc[k];
                        if (npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < targetDist && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                            {
                                if (npc.boss)       //is gonna try to detect bosses over anything
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                                else        //if it fails to detect a boss, it'll detect the next best thing
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                            }
                        }
                    }
                }
                if (target != null)
                {
                    attackFrames = true;
                    normalFrames = false;
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    if (targetPos.X > projectile.position.X)
                    {
                        projectile.velocity.X = 4f;
                    }
                    if (targetPos.X < projectile.position.X)
                    {
                        projectile.velocity.X = -4f;
                    }
                    if (targetPos.Y > projectile.position.Y)
                    {
                        projectile.velocity.Y = 4f;
                    }
                    if (targetPos.Y < projectile.position.Y)
                    {
                        projectile.velocity.Y = -4f;
                    }
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = targetPos - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("FireAnkh"), newProjectileDamage, 3f, Main.myPlayer, chanceToDebuff, debuffDuration);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    normalFrames = true;
                    attackFrames = false;
                }
            }
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            standTexture = mod.GetTexture("Projectiles/PlayerStands/MagiciansRed/MagiciansRed_" + animationName);
            if (animationName == "Idle")
            {
                AnimationStates(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 2, newShootTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 1, 2, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 2, true);
            }
        }
    }
}