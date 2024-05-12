using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using MarvelTerrariaUniverse.Common.Net;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.IO;
using Terraria.GameContent;
using Terraria.Utilities;
using Terraria.Graphics;
using System.Linq;
using Terraria.UI;
using Terraria.GameContent.UI;

namespace MarvelTerrariaUniverse;
public class MarvelTerrariaUniverse : Mod
{
    public static MarvelTerrariaUniverse Instance => ModContent.GetInstance<MarvelTerrariaUniverse>();
    public enum Transformation
    {
        None = 0,
        IronMan = 1
    }

    public static Dictionary<string, string> CategorizedModKeybinds = new(); // unused for now... will be used when lolxd and i figure out the fuckery behind the keybind menu soontm

    public const int IRONMANSUITS = 7;
    public const int EXTRALOADOUTS = 1;

    public static readonly Color[,] CustomLoadoutColors = new Color[EXTRALOADOUTS, 3] {
        { new(186, 12, 47), new(155, 17, 30), new(143, 7, 15) }
    };
public static Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float u = 1 - t;
			float tt = t * t;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * t;
			Vector2 p = uuu * p0; //first term
			p += 3 * uu * t * p1; //second term
			p += 3 * u * tt * p2; //third term
			p += ttt * p3; //fourth term
			return p;
		}

	public static Vector2 GetDrawPosition(Vector2 position, Vector2 origin, int width, int height, int texWidth, int texHeight, int framecount, float scale, bool drawCentered = false)
	{
		Vector2 vector = new Vector2((int)Main.screenPosition.X, (int)Main.screenPosition.Y);
		if (drawCentered)
		{
			Vector2 vector2 = new Vector2(texWidth / 2, texHeight / framecount / 2);
			return position + new Vector2((float)width * 0.5f, (float)height * 0.5f) - vector2 * scale + origin * scale - vector;
		}
		return position - vector + new Vector2((float)width * 0.5f, height) - new Vector2((float)texWidth * scale / 2f, (float)texHeight * scale / (float)framecount) + origin * scale + new Vector2(0f, 5f);
	}
public override void Load()
		{
			//GameShaders.Misc["MagicMissile"];
			//GameShaders.Misc["FlameLash"];
			//GameShaders.Misc["RainbowRod"];

			GameShaders.Misc["FlamelashLikeMagicMissile"] = new MiscShaderData(Main.PixelShaderRef, "MagicMissile").UseProjectionMatrix(doUse: true);
			GameShaders.Misc["FlamelashLikeMagicMissile"].UseImage0("Images/Extra_" + (short)195);
			GameShaders.Misc["FlamelashLikeMagicMissile"].UseImage1("Images/Extra_" + (short)189);
			GameShaders.Misc["FlamelashLikeMagicMissile"].UseImage2("Images/Extra_" + (short)190);

			GameShaders.Misc["SharpMagicMissile"] = new MiscShaderData(Main.PixelShaderRef, "MagicMissile").UseProjectionMatrix(doUse: true);
			GameShaders.Misc["SharpMagicMissile"].UseImage0("Images/Extra_" + (short)195);
			GameShaders.Misc["SharpMagicMissile"].UseImage1("Images/Extra_" + (short)197);
			GameShaders.Misc["SharpMagicMissile"].UseImage2("Images/Extra_" + (short)190);
		}
	
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        MTUNetMessages.HandlePacket(reader, whoAmI);
    }
}