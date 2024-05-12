using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MarvelTerrariaUniverse;
using MarvelTerrariaUniverse.Content.Projectiles;
using System.Collections.Generic;
using Humanizer;

namespace MarvelTerrariaUniverse.Content.Items.Accessories;


    public class InhibitorChip : ModItem
    {
        private List<Projectile> projectiles = new List<Projectile>();

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 25;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

         public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("TestProj").Type] < 4)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<TestProj>(), 20, 3f, player.whoAmI);
				
            }
        }
    }
	