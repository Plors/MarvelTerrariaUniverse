﻿using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.Content.Buffs;
using MarvelTerrariaUniverse.Content.Projectiles.Arsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
public class ArsenalRepulsor : ArsenalItem
{
    int cd = 0;
    bool offCD = true;
    IronManPlayer IMplayer;
    public override void UpdateArsenal(Player player)
    {
        base.UpdateArsenal(player);

        IMplayer = player.GetModPlayer<IronManPlayer>();
        if (IMplayer.Mark <= 5 && ((IMplayer.CurrentSuitState == IronManPlayer.SuitState.Flying) || (IMplayer.CurrentSuitState == IronManPlayer.SuitState.Hovering)))
        {
            return;
        }

        if (player.HasBuff(ModContent.BuffType<Waterlogged>()) || IMplayer.CurrentArmorMode == IronManPlayer.ArmorMode.Build)
        {
            return;
        }
        Vector2 mousePos = Main.MouseWorld;
        Vector2 relativeMousePos = mousePos - player.Center;
        relativeMousePos = Vector2.Normalize(relativeMousePos) * 10;
        Vector2 shootDirection = (Main.MouseWorld - (player.Center)).SafeNormalize(Vector2.UnitX * player.direction);

        if (offCD)
        {
            //adjust dmg in the projectile spawn here to balance
            Projectile.NewProjectile(Terraria.Entity.GetSource_None(), player.Center, shootDirection * 6f, ModContent.ProjectileType<Repulsor>(), 50, 10);
            offCD = false;
        }
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);
        if (!offCD)
        {
            cd++;
            if (cd >= 70) // 1.17 seconds
            {
                offCD = true;
                cd = 0;
            }
        }
    }
}
