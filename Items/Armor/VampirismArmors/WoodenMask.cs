using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors
{
    [AutoloadEquip(EquipType.Head)]
    public class WoodenMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An wooden mask based off of a much more menacing mask. When worn, you gain superhuman abilities, but you burn in the sunlight.\nRight-click while holding the mask to open the Zombie Skill Tree.\nNote: Skill Points are obtained by killing new types of enemies multiple times.");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 44;
            Item.value = Item.buyPrice(silver: 5);
            Item.rare = ItemRarityID.Green;
            Item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(ModContent.BuffType<Zombie>(), 2);
        }

        public override void HoldItem(Player player)
        {
            if (Main.mouseRight && player.whoAmI == Main.myPlayer && !ZombieSkillTree.Visible)
                ZombieSkillTree.OpenZombieSkillTree();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 40)
                .AddIngredient(ItemID.GoldBar, 2)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Wood, 40)
                .AddIngredient(ItemID.PlatinumBar, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}