﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Extensions
{
	public static class Texture2DExtensions
	{
		public static Color[,] ToColorArray(this Texture2D texture)
		{
			Color[] colors1D = new Color[texture.Width * texture.Height];
			texture.GetData(colors1D);

			Color[,] colors2D = new Color[texture.Width, texture.Height];
			for (int x = 0; x < texture.Width; x++)
			{
				for (int y = 0; y < texture.Height; y++)
				{
					colors2D[x, y] = colors1D[x + y * texture.Width];
				}
			}

			return colors2D;
		}
	}
}
