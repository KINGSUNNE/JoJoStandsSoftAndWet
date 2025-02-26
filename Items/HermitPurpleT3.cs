using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HermitPurpleT3 : StandItemClass
    {
        public override int standSpeed => 30;
        public override int standType => 1;
        public override int standTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/HermitPurpleT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hermit Purple (Tier 3)");
            Tooltip.SetDefault("Left-click to use Hermit Purple as a whip and right-click to grab enemies and slowly crush them!\nSpecial: Overcharge Hermit Purple so that enemies that hurt you get blown away!\nPassive: Enemies are hurt when they hurt you and get inflicted Sunburn!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 157;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 1;
            mPlayer.hermitPurpleTier = standTier;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HermitPurpleT2>())
                .AddIngredient(ItemID.HallowedBar, 7)
                .AddIngredient(ItemID.Amethyst, 6)
                .AddIngredient(ItemID.ThornsPotion)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 5)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 3)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
