using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.Content.Buffs;
using MarvelTerrariaUniverse.Content.Items.Armor.IronMan;
using MarvelTerrariaUniverse.Content.Projectiles.Arsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
public class ArsenalRocketBoots : ArsenalItem
{
    public override void SetDefaults()
    {
        base.SetDefaults();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);
        IronManPlayer IMplayer = player.GetModPlayer<IronManPlayer>();
        if (IMplayer.Mark == 1)
        {
            player.rocketBoots = 2;
            player.rocketTimeMax = 5;
        }
    }


}

