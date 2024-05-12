using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using MarvelTerrariaUniverse.Content.Projectiles;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace MarvelTerrariaUniverse.Content.Items.Weapons
{
    public class YakaArrow : ModItem
    {
 
        public override void SetDefaults()
        {

            Item.damage = 60;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 42;
            Item.useTime = 60;
            Item.useAnimation = 60;
			Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = 80000;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<YakaArrowProj>();
            Item.useTurn = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.shootSpeed = 5f;
        }
	}
}


       
