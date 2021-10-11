using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SixLabors.ImageSharp;

namespace Clever2D.Engine
{
	/// <summary>
	/// 2D array of Sprites.
	/// </summary>
	public class SpriteArray
	{
		/// <summary>
		/// Path to the source of the image of this Sprite.
		/// </summary>
		[JsonProperty]
		public readonly string spritePath; 
		/// <summary>
		/// Sprite pivot (scales from 0 to 1 per dimension).
		/// </summary>
		[JsonProperty]
		public readonly Vector2 pivot;
		/// <summary>
		/// SpriteArrays row count.
		/// </summary>
		[JsonProperty]
		public readonly int rows;
		/// <summary>
		/// SpriteArrays column count.
		/// </summary>
		[JsonProperty]
		public readonly int columns;
		
		/// <summary>
		/// Returns this Sprite array.
		/// </summary>
		public Sprite[] Sprites
		{
			get;
			private set;
		} = Array.Empty<Sprite>();

		[JsonConstructor]
		private SpriteArray()
		{
			if (spritePath != null && pivot != null)
				CreateArray();
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
			this.spritePath = spritePath;
			this.pivot = pivot;
			this.rows = rows;
			this.columns = columns;
			CreateArray();
		}

		private void CreateArray()
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
						
						sprite.size = new Vector2Int((int)Math.Floor(splitWidth), (int)Math.Floor(splitHeight));
						sprite.offset = new Vector2(splitWidth * x, splitHeight * y);
						
						sprites.Add(sprite);
					}
				}
				
				Sprites = sprites.ToArray();
			}
			else
			{
				Player.LogError($"File doesn't exist in {path}");
			}
		}
	}
}
