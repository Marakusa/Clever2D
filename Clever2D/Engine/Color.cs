using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using SDL2;

namespace Clever2D.Engine
{
	/// <summary>
	/// Color class makes SDL color usage easier.
	/// </summary>
	public class Color
	{
		/// <summary>
		/// Red value of this color.
		/// </summary>
		public int r;
		/// <summary>
		/// Green value of this color.
		/// </summary>
		public int g;
		/// <summary>
		/// Blue value of this color.
		/// </summary>
		public int b;
		/// <summary>
		/// Alpha value of this color.
		/// </summary>
		public int a;
		
		/// <summary>
		/// Color class makes SDL color usage easier.
		/// </summary>
		public Color()
		{
			this.r = 0;
			this.g = 0;
			this.b = 0;
			this.a = 0;
		}

		/// <summary>
		/// Color class makes SDL color usage easier.
		/// </summary>
		/// <param name="r">Red value of this color.</param>
		/// <param name="g">Green value of this color.</param>
		/// <param name="b">Blue value of this color.</param>
		/// <param name="a">Alpha value of this color.</param>
		public Color(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		
		/// <summary>
		/// Returns this Color as SDL_Color.
		/// </summary>
		public SDL.SDL_Color SdlColor
		{
			get
			{
				SDL.SDL_Color c = new()
				{
					r = BitConverter.GetBytes(this.r)[0],
					g = BitConverter.GetBytes(this.g)[0],
					b = BitConverter.GetBytes(this.b)[0],
					a = BitConverter.GetBytes(this.a)[0]
				};
			
				return c;
			}
		}

		/// <summary>
		/// Returns this Color as a hash code.
		/// </summary>
		public override int GetHashCode()
		{
			return ($"{r},{g},{b},{a}").GetHashCode();
		}
	}
}
