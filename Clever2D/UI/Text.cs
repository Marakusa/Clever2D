using System;
using Clever2D.Core;
using Clever2D.Engine;
using SDL2;

namespace Clever2D.UI
{
	/// <summary>
	/// Text element for UI.
	/// </summary>
	public class Text : UIElement
	{
		/// <summary>
		/// Text element text.
		/// </summary>
		public string text = "";
		/// <summary>
		/// Text element text style.
		/// </summary>
		public FontStyle fontStyle = FontStyle.Normal;
		/// <summary>
		/// Text element text font size.
		/// </summary>
		public int size = 12;
		/// <summary>
		/// Text element text color.
		/// </summary>
		public SDL.SDL_Color color = new SDL.SDL_Color() { r = 255, g = 255, b = 255, a = 255 };
		/// <summary>
		/// Text element text font.
		/// </summary>
		private IntPtr sans = IntPtr.Zero;

		/// <summary>
		/// Is element placed on the Screen or in the Scene.
		/// </summary>
		public bool worldSpace = false;

		internal override void Initialize()
		{
			if (Clever.Fonts.Length > 0)
			{
				sans = Clever.Fonts[0];
			}
		}

		internal override void Render()
		{
			switch (fontStyle)
			{
				case FontStyle.Normal:
					SDL_ttf.TTF_SetFontStyle(sans, SDL_ttf.TTF_STYLE_NORMAL);
					break;
				case FontStyle.Italic:
					SDL_ttf.TTF_SetFontStyle(sans, SDL_ttf.TTF_STYLE_ITALIC);
					break;
				case FontStyle.Bold:
					SDL_ttf.TTF_SetFontStyle(sans, SDL_ttf.TTF_STYLE_BOLD);
					break;
				case FontStyle.Underline:
					SDL_ttf.TTF_SetFontStyle(sans, SDL_ttf.TTF_STYLE_UNDERLINE);
					break;
				case FontStyle.Strikethrough:
					SDL_ttf.TTF_SetFontStyle(sans, SDL_ttf.TTF_STYLE_STRIKETHROUGH);
					break;
			}

			IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(sans, text, color);

			IntPtr fpsText = SDL.SDL_CreateTextureFromSurface(Clever.Renderer, surfaceMessage);

			SDL.SDL_FreeSurface(surfaceMessage);

			surfaceMessage = IntPtr.Zero;

			float x = 0;
			float y = 0;
			float w = 0;
			float h = 0;
			
			SDL.SDL_Rect fpsRect;
			if (!worldSpace)
			{
				w = text.Length * (18f * (size / 28f));
				h = size;

				float width = Clever.Size.Width;
				float height = Clever.Size.Height;
				x = width * screenAlign.x - w * pivot.x;
				y = height * screenAlign.y - h * pivot.y;
			}
			else
			{
				if (Camera.MainCamera != null)
				{
					Transform cameraTransform = Camera.MainCamera.transform;

					float scale = (Clever.Size.Height / 600f) * 2f;
                    
					float cameraOffsetX = scale * cameraTransform.Position.x;
					float cameraOffsetY = scale * -cameraTransform.Position.y;

					w = text.Length * (18f * (size / 28f)) * scale * transform.Scale.x;
					h = size * scale * transform.Scale.y;

					if (gameObject.parent != null)
					{
						x = transform.Position.x * scale * transform.Scale.x - cameraOffsetX;
						y = -transform.Position.y * scale * transform.Scale.y - cameraOffsetY;
						x += gameObject.parent.transform.Position.x * scale + Clever.Size.Width / 2f;
						y -= gameObject.parent.transform.Position.y * scale - Clever.Size.Height / 2f;
						
						x -= w * pivot.x;
						y -= h * pivot.y;
						
						w *= transform.Scale.x;
						h *= transform.Scale.y;
					}
					else
					{
						x = transform.Position.x * scale * transform.Scale.x - cameraOffsetX;
						y = -transform.Position.y * scale * transform.Scale.y - cameraOffsetY;
					}
				}
			}

			fpsRect.x = (int)Math.Round(x);
			fpsRect.y = (int)Math.Round(y);
			fpsRect.w = (int)Math.Round(w);
			fpsRect.h = (int)Math.Round(h);
			
			SDL.SDL_RenderCopy(Clever.Renderer, fpsText, IntPtr.Zero, ref fpsRect);
			SDL.SDL_DestroyTexture(fpsText);
		}

		/// <summary>
		/// Text element for UI.
		/// </summary>
		public Text(int size)
		{
			this.text = "";
			this.size = size;
			this.color = new SDL.SDL_Color()
			{
				r = 255, 
				g = 255, 
				b = 255, 
				a = 255
			};
		}
		/// <summary>
		/// Text element for UI.
		/// </summary>
		public Text(string text, int size)
		{
			this.text = text;
			this.size = size;
			this.color = new SDL.SDL_Color()
			{
				r = 255, 
				g = 255, 
				b = 255, 
				a = 255
			};
		}
		/// <summary>
		/// Text element for UI.
		/// </summary>
		public Text(string text, int size, SDL.SDL_Color color)
		{
			this.text = text;
			this.size = size;
			this.color = color;
		}
		/// <summary>
		/// Text element for UI.
		/// </summary>
		public Text(SDL.SDL_Color color)
		{
			this.text = "";
			this.size = 12;
			this.color = color;
		}

		/// <summary>
		/// Disposes and destroys this Component.
		/// </summary>
		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

	/// <summary>
	/// Font styles for UI elements.
	/// </summary>
	public enum FontStyle
	{
		/// <summary>
		/// Normal font style.
		/// </summary>
		Normal, 
		/// <summary>
		/// Italic font style.
		/// </summary>
		Italic, 
		/// <summary>
		/// Bold font style.
		/// </summary>
		Bold, 
		/// <summary>
		/// Underline font style.
		/// </summary>
		Underline, 
		/// <summary>
		/// Strikethrough font style.
		/// </summary>
		Strikethrough
	}
}
