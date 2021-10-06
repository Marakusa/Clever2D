using System;
using System.Collections.Generic;
using System.IO;
using Clever2D.Core;
using SDL2;
using SixLabors.ImageSharp;

namespace Clever2D.Engine
{
	/// <summary>
	/// 2D array of Sprites.
	/// </summary>
	public class SpriteArray
	{
		/// <summary>
		/// Sprite array.
		/// </summary>
		private Sprite[] sprites = new Sprite[0];
		/// <summary>
		/// Returns this Sprite array.
		/// </summary>
		public Sprite[] Sprites
		{
			get
			{
				return sprites;
			}
		}

		/// <summary>
		/// 2D array of Sprites.
		/// </summary>
		/// <param name="spritePath">Target Sprite asset path.</param>
		/// <param name="pivot">Split sprites pivot point.</param>
		/// <param name="rows">Split Sprite rows count.</param>
		/// <param name="columns">Split Sprite columns count.</param>
		public SpriteArray(string spritePath, Vector2 pivot, int rows, int columns)
		{
			string path = Clever.executableDirectory + "/assets/" + spritePath;
			
			if (File.Exists(path))
			{
				Image image = Image.Load(path);
				int width = image.Width;
				int height = image.Height;
				
				List<Sprite> sprites = new();

				for (int y = 0; y < rows; y++)
				{
					for (int x = 0; x < columns; x++)
					{
						int splitWidth = (int)Math.Floor((double)width / (double)columns);
						int splitHeight = (int)Math.Floor((double)height / (double)rows);
						
						Sprite sprite = new(
							spritePath, 
							pivot, 
							new Vector2Int(
								splitWidth, 
								splitHeight
							), 
							new Vector2(
								(float)splitWidth * (float)x, 
								(float)splitHeight * (float)y
							)
						);
						sprites.Add(sprite);
					}
				}
				
				this.sprites = sprites.ToArray();
			}
			else
			{
				Player.LogError("File doesn't exist in " + path);
			}
		}
	}
}
