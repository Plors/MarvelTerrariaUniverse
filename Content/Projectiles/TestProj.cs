using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Utilities;
using MarvelTerrariaUniverse;

namespace MarvelTerrariaUniverse.Content.Projectiles
{
	public class TestProj : ModProjectile
	{

		public override void SetDefaults()
		{
			{
				Projectile.width = 30;
				Projectile.height = 30;
				Projectile.aiStyle = 0; 
				Projectile.friendly = true;
				Projectile.minion = true;
				Projectile.hostile = false;
				Projectile.usesLocalNPCImmunity = true;
				Projectile.localNPCHitCooldown = 25;
				Projectile.penetrate = -1;
				Projectile.tileCollide = false;
				Projectile.ignoreWater = true;


			}
		}
		Vector2 direction2;

		public override void AI()
		{
			// Check if the projectile is currently following an NPC
			if (Projectile.ai[0] == 1f && Projectile.ai[1] != -1f)
			{
				// Set ai[1] to a different value to trigger the texture change in PreDraw()
				Projectile.ai[1] = 1f;
			}
			else
			{
				// Reset ai[1] to its original value
				Projectile.ai[1] = 0f;
			}

			Projectile.damage = 34;

			Player player = Main.player[Projectile.owner];

			#region Active check
			// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
			if (player.dead || !player.active)
			{

			}
			{
				Projectile.timeLeft = 2;
			}
			#endregion

			#region General behavior
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 148f; // Go up 48 coordinates (three tiles from the center of the player)

			// If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
			// The index is projectile.minionPos
			float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -player.direction;
			idlePosition.X += minionPositionOffsetX; // Go behind the player

			// All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

			// Teleport to player if distance is too big
			Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
			float distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
			{
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
				// and then set netUpdate to true
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.04f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
				{
					if (Projectile.position.X < other.position.X) Projectile.velocity.X -= overlapVelocity;
					else Projectile.velocity.X += overlapVelocity;

					if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
					else Projectile.velocity.Y += overlapVelocity;
				}
			}
            #endregion

            #region Find target
            // Starting search distance
            float distanceFromTarget = 700f;
            Vector2 targetCenter = Projectile.Center;
            bool foundTarget = false;

            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);
                bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                bool closeThroughWall = between < 60f;

                if (between < 1200f && (lineOfSight || closeThroughWall))
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }
            if (!foundTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && npc.active)
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 60f;
                        if (((closest || !foundTarget) && inRange) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
				}
			}

			// friendly needs to be set to true so the minion can deal contact damage
			// friendly needs to be set to false so it doesn't damage things like target dummies while idling
			// Both things depend on if it has a target or not, so it's just one assignment here
			// You don't need this assignment if your minion is shooting things instead of dealing contact damage
			Projectile.friendly = foundTarget;
			#endregion

			#region Movement

			// Default movement parameters (here for attacking)
			float speed = 40f;
			float inertia = 25f;

			if (foundTarget)
			{
				// Minion has a target: attack (here, fly towards the enemy)
				if (distanceFromTarget > 10f)
				{
					// The immediate range around the target (so it doesn't latch onto it when close)
					Vector2 direction = targetCenter - Projectile.Center;
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
				}
			}
			else
			{
				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 40f)
				{
					// Speed up the minion if it's away from the player
					speed = 28f;
					inertia = 30f;
				}
				else
				{
					// Slow down the minion if closer to the player
					speed = 28f;
					inertia = 30f;
				}
				if (distanceToIdlePosition > 40f)
				{
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (Projectile.velocity == Vector2.Zero)
				{
					// If there is a case where it's not moving at all, give it a little "poke"
					Projectile.velocity.X = -0.05f;
					Projectile.velocity.Y = -0.05f;
				}

				#endregion

				#region Animation and visuals
				// So it will lean slightly towards the direction it's moving
				Projectile.rotation = Projectile.velocity.X * 0.05f;


			}


			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() * 0.4f);
			#endregion
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
        public override bool PreDraw(ref Color lightColor)
        {


			Texture2D texture = ModContent.Request<Texture2D>("MarvelTerrariaUniverse/Content/Projectiles/Segment").Value;
			Texture2D lastTexture = ModContent.Request<Texture2D>("MarvelTerrariaUniverse/Content/Projectiles/Claw").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 lastDrawOrigin = new Vector2(lastTexture.Width * 0.5f, lastTexture.Height * 0.5f);
			Player player = Main.player[Projectile.owner];
			Vector2 playerCenter = player.Center;
			Vector2 dynamicScaling = new Vector2(80, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
			float moreScaling = 1.15f - 0.25f * Math.Abs(dynamicScaling.X) / 80f;
			Vector2 p0 = playerCenter;
			Vector2 p1 = playerCenter - player.velocity.RotatedBy(MathHelper.ToRadians(270 + dynamicScaling.X)) * 13.5f * moreScaling;
			Vector2 p2 = Projectile.Center - Projectile.velocity * 18f * moreScaling;
			Vector2 p3 = Projectile.Center;
			int segments = 60;
			for (int i = 0; i < segments; i++)
			{
				float t = i / (float)segments;
				Vector2 drawPos2 = MarvelTerrariaUniverse.CalculateBezierPoint(t, p0, p1, p2, p3);
				t = (i + 1) / (float)segments;
				Vector2 drawPosNext = MarvelTerrariaUniverse.CalculateBezierPoint(t, p0, p1, p2, p3);
				float rotation = (drawPos2 - drawPosNext).ToRotation();
				lightColor = Lighting.GetColor((int)drawPos2.X / 16, (int)(drawPos2.Y / 16));
				Texture2D currentTexture = texture;
				if (i == segments - 1) // Check if it's the last segment
				{
					currentTexture = lastTexture;
					drawOrigin = lastDrawOrigin;
				}
				Main.spriteBatch.Draw(currentTexture, drawPos2 - Main.screenPosition, null, Projectile.GetAlpha(lightColor), rotation - MathHelper.ToRadians(90), drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
