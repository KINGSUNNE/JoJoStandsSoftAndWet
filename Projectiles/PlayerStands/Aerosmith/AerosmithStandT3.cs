using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Aerosmith
{
    public class AerosmithStandT3 : StandClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/PlayerStands/Aerosmith/AerosmithStandT1"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 40;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.netImportant = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 0;
            Projectile.ignoreWater = true;
        }

        public override float shootSpeed => 12f;

        public override int projectileDamage => 63;
        public override int shootTime => 8;      //+2 every tier
        public override StandType standType => StandType.Ranged;
        public override string poseSoundName => "VolareVia";
        public override string spawnSoundName => "Aerosmith";

        private bool bombless = false;
        private bool fallingFromSpace = false;

        public override void AI()
        {
            SelectFrame();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            mPlayer.aerosmithWhoAmI = Projectile.whoAmI;
            newProjectileDamage = (int)(newProjectileDamage * MathHelper.Clamp(1f - (Projectile.Distance(player.Center) / (350f * 16f)), 0.5f, 1f));

            fallingFromSpace = Projectile.position.Y < (Main.worldSurface * 0.35) * 16f;
            if (fallingFromSpace)
            {
                Projectile.frameCounter = 0;
                Projectile.velocity.Y += 0.3f;
                Projectile.netUpdate = true;
            }
            Vector2 rota = Projectile.Center - Main.MouseWorld;
            Projectile.rotation = (-rota * Projectile.direction).ToRotation();
            bombless = player.HasBuff(ModContent.BuffType<AbilityCooldown>());
            Projectile.tileCollide = true;

            if (!mPlayer.standAutoMode)
            {
                Projectile.tileCollide = true;
                mPlayer.standRemoteMode = true;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);

                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !fallingFromSpace)
                {
                    float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        Projectile.velocity = Main.MouseWorld - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 9f;

                        Projectile.direction = 1;
                        if (Main.MouseWorld.X < Projectile.position.X - 5)
                            Projectile.direction = -1;

                        Projectile.spriteDirection = Projectile.direction;
                    }
                    else
                    {
                        Projectile.velocity *= 0.95f;
                    }
                }
                else
                {
                    Projectile.rotation = 0f;
                    if (!fallingFromSpace)
                        Projectile.velocity *= 0.95f;
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), newProjectileDamage, 3f, Projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                    }
                }
                if (SpecialKeyPressedNoCooldown() && !bombless && player.whoAmI == Main.myPlayer)
                {
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(10));
                    shootCount += newShootTime;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AerosmithBomb>(), 0, 3f, Projectile.owner, 314 * (float)mPlayer.standDamageBoosts);
                    Main.projectile[proj].netUpdate = true;
                }
            }
            if (mPlayer.standAutoMode)
            {
                Projectile.rotation = (Projectile.velocity * Projectile.direction).ToRotation();
                NPC target = FindNearestTarget(350f);
                if (target == null)
                {
                    if (Projectile.Distance(player.Center) < 80f)
                    {
                        if (Projectile.position.X >= player.position.X + 50f || WorldGen.SolidTile((int)(Projectile.position.X / 16) - 3, (int)(Projectile.position.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = -2f;
                            Projectile.spriteDirection = Projectile.direction = -1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.position.X < player.position.X - 50f || WorldGen.SolidTile((int)(Projectile.position.X / 16) + 3, (int)(Projectile.position.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = 2f;
                            Projectile.spriteDirection = Projectile.direction = 1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.position.Y > player.position.Y + 2f)
                        {
                            Projectile.velocity.Y = -2f;
                        }
                        if (Projectile.position.Y < player.position.Y - 2f)
                        {
                            Projectile.velocity.Y = 2f;
                        }
                        if (Projectile.position.Y < player.position.Y + 2f && Projectile.position.Y > player.position.Y - 2f)
                        {
                            Projectile.velocity.Y = 0f;
                            Projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 8f;
                    }
                }
                if (target != null)
                {
                    if (Projectile.Distance(target.Center) > 45f)
                    {
                        Projectile.velocity = target.position - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 8f;

                        Projectile.direction = 1;
                        if (Projectile.velocity.X < 0f)
                            Projectile.direction = -1;
                        Projectile.spriteDirection = Projectile.direction;
                        Projectile.netUpdate = true;
                    }
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            shootCount += newShootTime;
                            SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                            Vector2 shootVel = target.Center - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), newProjectileDamage, 3f, Projectile.owner);
                            Main.projectile[proj].netUpdate = true;
                        }
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(bombless);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            bombless = reader.ReadBoolean();
        }

        public void SelectFrame()
        {
            Projectile.frameCounter++;
            if (bombless)
            {
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frame += 1;
                    Projectile.frameCounter = 0;
                    if (Projectile.frame >= 4)
                        Projectile.frame = 2;
                }
                if (Projectile.frame <= 1)
                    Projectile.frame = 2;

            }
            else
            {
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frame += 1;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame >= 2)
                    Projectile.frame = 0;
            }
        }
    }
}