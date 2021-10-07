namespace Clever2D.Engine
{
	/// <summary>
	/// The animation is used to play back animations.
	/// </summary>
	public class Animation
	{
		/// <summary>
		/// Name of this animation.
		/// </summary>
		public string name;
		/// <summary>
		/// Timeline of all frames of this animation.
		/// </summary>
		public AnimationPoint[] timeline;
		/// <summary>
		/// All transitions connected from this animation.
		/// </summary>
		public AnimationTransition[] transitions;
	}
}
