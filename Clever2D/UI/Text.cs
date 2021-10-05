using System;
using Clever2D.Core;
using Clever2D.Engine;
using SDL2;

namespace Clever2D.UI
{
	public class Text : UIElement
	{
		public string text = "";
		public int size = 12;
		public SDL.SDL_Color color = new SDL.SDL_Color() { r = 255, g = 255, b = 255, a = 255 };
		private IntPtr sans = IntPtr.Zero;

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
			IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(sans, text, color);

			IntPtr fpsText = SDL.SDL_CreateTextureFromSurface(Clever.Renderer, surfaceMessage);

			SDL.SDL_FreeSurface(surfaceMessage);

			surfaceMessage = IntPtr.Zero;

			SDL.SDL_Rect fpsRect;
			if (!worldSpace)
			{
				fpsRect.x = 0;
				fpsRect.y = 0;
				fpsRect.w = text.Length * (int)Math.Round(18f * (size / 28f));
				fpsRect.h = size;
			}
			else
			{
				float scale = (Clever.Size.Height / 600f) * 2f;
				
				fpsRect.w = (int)Math.Round(text.Length * (int)Math.Round(18f * (size / 28f)) * scale * (int)Math.Round(transform.scale.x));
				fpsRect.h = (int)Math.Round(size * scale * (int)Math.Round(transform.scale.y));

				if (gameObject.parent != null)
				{
					fpsRect.x = (int)Math.Round(transform.position.x * scale * transform.scale.x) + (int)Math.Round(gameObject.parent.transform.position.x * scale * gameObject.parent.transform.scale.x);
					fpsRect.y = (int)Math.Round(-transform.position.y * scale * transform.scale.y) + (int)Math.Round(-gameObject.parent.transform.position.y * scale * gameObject.parent.transform.scale.y);
					fpsRect.w *= (int)Math.Round(transform.scale.x);
					fpsRect.h *= (int)Math.Round(transform.scale.y);
				}
				else
				{
					fpsRect.x = (int)Math.Round(transform.position.x * scale * transform.scale.x);
					fpsRect.y = (int)Math.Round(-transform.position.y * scale * transform.scale.y);
				}
			}

			SDL.SDL_RenderCopy(Clever.Renderer, fpsText, IntPtr.Zero, ref fpsRect);
			SDL.SDL_DestroyTexture(fpsText);
		}

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
		public Text(string text, int size, SDL.SDL_Color color)
		{
			this.text = text;
			this.size = size;
			this.color = color;
		}
		public Text(SDL.SDL_Color color)
		{
			this.text = "";
			this.size = 12;
			this.color = color;
		}
	}
}
