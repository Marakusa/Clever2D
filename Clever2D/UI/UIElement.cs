using Clever2D.Engine;
using Newtonsoft.Json;

namespace Clever2D.UI
{
	/// <summary>
	/// Base class for all UI elements.
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public abstract class UIElement : Component
	{
		/// <summary>
		/// Alignment of this UI element on screen. (0,0) is top-left and (1,1) is bottom-right.
		/// </summary>
		[JsonProperty]
		public Vector2 screenAlign = Vector2.Zero;
		/// <summary>
		/// Pivot of this UI element. (0,0) is top-left and (1,1) is bottom-right.
		/// </summary>
		[JsonProperty]
		public Vector2 pivot = Vector2.Zero;

		internal abstract void Render();
	}
}
