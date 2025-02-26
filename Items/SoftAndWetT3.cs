using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SoftAndWetT3 : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "SoftAndWet";
        public override int standTier => 3;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soft and Wet (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a fast rate and right-click to create a Plunder Bubble!\nPassive: Bubble Generation\nSecond Special: Bubble Barrier!\nUsed in Stand Slot");
        }
        public override string Texture
        {
            get { return Mod.Name + "/Items/SoftAndWetT1"; }
        }
        public override void SetDefaults()
        {
            Item.damage = 63;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SoftAndWetT2>())
                .AddIngredient(ModContent.ItemType<WillToFight>(), 3)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 3)
                .AddIngredient(ItemID.BubbleMachine)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
