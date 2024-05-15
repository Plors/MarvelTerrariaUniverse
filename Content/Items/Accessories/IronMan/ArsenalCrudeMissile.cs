using MarvelTerrariaUniverse.Common.Players;
using MarvelTerrariaUniverse.Content.Buffs;
using MarvelTerrariaUniverse.Content.Projectiles.Arsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Items.Accessories.IronMan;
public class ArsenalCrudeMissile : ArsenalItem
{
    int cd = 0;
    bool offCD = true;
    private IronManPlayer IMplayer;

    public override void UpdateArsenal(Player player)
    {
        IMplayer = player.GetModPlayer<IronManPlayer>();

        base.UpdateArsenal(player);

        // disables if player in water
        if (player.HasBuff(ModContent.BuffType<Waterlogged>()) || IMplayer.CurrentArmorMode == IronManPlayer.ArmorMode.Build)
        {
            return;
        }

        // calculations for direction and speed
        Vector2 mousePos = Main.MouseWorld;
        Vector2 relativeMousePos = mousePos - player.Center;
        relativeMousePos = Vector2.Normalize(relativeMousePos) * 10;

        if (offCD)
        {
            IMplayer.ArmRotation = true;

            //adjust dmg in the projectile spawn here to balance
            Projectile.NewProjectile(Terraria.Entity.GetSource_None(), player.Center, relativeMousePos, ModContent.ProjectileType<CrudeMissile>(), 30, 5);
            offCD = false;
        }
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        // 10 second cooldown
        base.UpdateAccessory(player, hideVisual);
        if (!offCD)
        {
            cd++;
            if (cd > 600)
            {
                offCD = true;
                cd = 0;
            }
        }
    }

}
