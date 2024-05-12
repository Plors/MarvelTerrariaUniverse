using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace MarvelTerrariaUniverse.Content.Projectiles;
public struct YakaArrowDrawer
{
	private static VertexStrip _vertexStrip = new VertexStrip();
	public void Draw(Projectile proj)
	{
		MiscShaderData miscShaderData = GameShaders.Misc["SharpMagicMissile"];
		miscShaderData.UseSaturation(-2.8f);
		miscShaderData.UseOpacity(4f);
		miscShaderData.Apply();
		YakaArrowDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
		YakaArrowDrawer._vertexStrip.DrawTrail();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
	private Color StripColors(float progressOnStrip)
{
    // Define colors as (R, G, B) tuples
    (int r1, int g1, int b1) = (255, 0, 0); // Red
    (int r2, int g2, int b2) = (255, 50, 50); // White

    // Calculate lerped color components
    int lerpedR = (int)(Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true) * r2 + (1 - Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * r1);
    int lerpedG = (int)(Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true) * g2 + (1 - Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * g1);
    int lerpedB = (int)(Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true) * b2 + (1 - Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * b1);

    // Calculate alpha
    float alpha = 0.5f / 2; // Assuming you want half of the original alpha

    // Return the resulting color
    return new Color(lerpedR, lerpedG, lerpedB, (int)(255 * alpha));
}
	private float StripWidth(float progressOnStrip)
	{
		float num = 1f;
		float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
		num *= 1f - (1f - lerpValue) * (1f - lerpValue);
		return MathHelper.Lerp(0f, 5f, num);
	}
}