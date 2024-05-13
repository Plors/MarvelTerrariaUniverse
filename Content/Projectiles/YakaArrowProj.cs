using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MarvelTerrariaUniverse.Content.Projectiles
{
	public class YakaArrowProj : ModProjectile
	{
		

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 960;
			ProjectileID.Sets.TrailCacheLength[Type] = 60;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
	
			Projectile.aiStyle = 9;
			Projectile.alpha = 0;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 12;
			Projectile.Opacity = 0.5f;
			Projectile.penetrate = 10;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.width = 32;
			Projectile.height = 32;
		
      }

		public override void Kill(int timeLeft)
		{
			for (int d = 0; d < Projectile.oldPos.Length; d++)
			{
				Dust TrailDust = Dust.NewDustDirect(Projectile.oldPos[d], Projectile.width, Projectile.height, 182, 0f, 0f, 0);
				TrailDust.fadeIn = Main.rand.NextFloat() * 1f;
				TrailDust.noGravity = true;
				TrailDust.noLightEmittence = (TrailDust.noLight = true);
				TrailDust.scale = Main.rand.NextFloat();
			}
		}
public override bool OnTileCollide(Vector2 velocity1)
	{
		if ((double)base.Projectile.velocity.Y != (double)velocity1.Y || (double)base.Projectile.velocity.X != (double)velocity1.X)
		{
			if ((double)base.Projectile.velocity.X != (double)velocity1.X)
			{
				base.Projectile.velocity.X = 0f - velocity1.X;
			}
			if ((double)base.Projectile.velocity.Y != (double)velocity1.Y)
			{
				base.Projectile.velocity.Y = 0f - velocity1.Y;
			}
		}
		return false;
	}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.ai[0] == -1f)
			{
				Projectile.ai[1] = -1f;
				Projectile.netUpdate = true;
			}
		}
		

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>("MarvelTerrariaUniverse/Content/Projectiles/YakaArrowProj").Value;
			Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int startY = frameHeight * Projectile.frame;
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			 Color color = Color.White;
			float LerpValue = Utils.GetLerpValue(0f, 8f, Projectile.velocity.Length(), clamped: true);
			float rotation = Projectile.rotation * LerpValue - (float)Math.PI / 2f * LerpValue;
			Vector2 origin = sourceRectangle.Size() / 2f;
			Vector2 scale = Vector2.One * Projectile.scale;
			scale.X *= MathHelper.Lerp(1f, 0.8f, LerpValue);
			SpriteEffects effects = SpriteEffects.None;
			float LerpValueScale = Utils.GetLerpValue(0f, 6f, Projectile.localAI[0], clamped: true);
			Vector2 scale2 = new Vector2(LerpValueScale) * scale;
			Vector2 SpinningPoint = new Vector2(4f * scale2.X, 0f).RotatedBy(rotation);

			for (float i = 0f; i < 1f; i += 0.25f)
			{
				Vector2 position2 = position + SpinningPoint.RotatedBy(i * ((float)Math.PI * 2f));
				Main.EntitySpriteDraw(texture, position2, sourceRectangle, color * 0.75f, rotation, origin, scale2, effects, 0);
			}
			Main.EntitySpriteDraw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, 0);

			default(YakaArrowDrawer).Draw(Projectile);
			
			return false;
		}
		
		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.2f, 0.8f, 0.4f));
		}
	}
}