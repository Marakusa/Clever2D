using System;
using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp;

namespace Clever2D.Engine
{
	/// <summary>
	/// 2D array of Sprites.
	/// </summary>
	public class SpriteArray
	{
		/// <summary>
		/// Returns this Sprite array.
		/// </summary>
		public Sprite[] Sprites
		{
			get;
		} = Array.Empty<Sprite>();

		/// <summary>
		/// 2D array of Sprites.
		/// </summary>
		/// <param name="spritePath">Target Sprite asset path.</param>
		/// <param name="pivot">Split sprites pivot point.</param>
		/// <param name="rows">Split Sprite rows count.</param>
		/// <param name="columns">Split Sprite columns count.</param>
		public SpriteArray(string spritePath, Vector2 pivot, int rows, int columns)
		{
			var path = $"{Application.ExecutableDirectory}/assets/{spritePath}";
			
			if (File.Exists(path))
			{
				var spriteImage = Image.Load(path);
				var width = (float)spriteImage.Width;
				var height = (float)spriteImage.Height;
				
				List<Sprite> sprites = new();

				for (int y = 0; y < rows; y++)
				{
					for (int x = 0; x < columns; x++)
					{
						var splitWidth = width / columns;
						var splitHeight = height / rows;
						
						var sprite = new Sprite(
							spritePath, 
							pivot, 
							new(
								(int)Math.Floor(splitWidth), 
								(int)Math.Floor(splitHeight)
							), 
							new(
								splitWidth * x, 
								splitHeight * y
							)
						);
						sprites.Add(sprite);
					}
				}
				
				this.Sprites = sprites.ToArray();
			}
			else
			{
				Player.LogError($"File doesn't exist in {path}");
			}
		}
	}
}
