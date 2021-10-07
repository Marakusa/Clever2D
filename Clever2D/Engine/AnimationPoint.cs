namespace Clever2D.Engine
{
	/// <summary>
	/// The point of an animations timeline.
	/// </summary>
	public class AnimationPoint
	{
		/// <summary>
		/// The point of time in the animations timeline in milliseconds.
		/// </summary>
		public int time;
		/// <summary>
		/// The index of a sprite in a sprite array.
		/// </summary>
		public int spriteIndex;

		/// <summary>
		/// The point of an animations timeline.
		/// </summary>
		public AnimationPoint(int time, int spriteIndex)
		{
			this.time = time;
			this.spriteIndex = spriteIndex;
		}
	}
}
