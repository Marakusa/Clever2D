using Clever2D.Engine;

namespace Clever2D.UI
{
	public abstract class UIElement : Component
	{
		/// <summary>
		/// Alignment of this UI element on screen. (0,0) is top-left and (1,1) is bottom-right.
		/// </summary>
		public Vector2 screenAlign = Vector2.zero;
		/// <summary>
		/// Pivot of this UI element. (0,0) is top-left and (1,1) is bottom-right.
		/// </summary>
		public Vector2 pivot = Vector2.zero;

		internal abstract void Render();
	}
}
