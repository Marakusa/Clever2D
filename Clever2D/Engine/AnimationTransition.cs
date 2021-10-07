namespace Clever2D.Engine
{
	/// <summary>
	/// Animation transition from animation to animation.
	/// </summary>
	public class AnimationTransition
	{
		/// <summary>
		/// Transition conditions.
		/// </summary>
		public AnimationTransitionCondition[] conditions;
		/// <summary>
		/// ID of the animation to transition.
		/// </summary>
		public int to;
	}
}
