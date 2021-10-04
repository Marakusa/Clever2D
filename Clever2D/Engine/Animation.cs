namespace Clever2D.Engine
{
	public class Animation
	{
		public string name;
		public AnimationPoint[] timeline;
	}

	public class AnimationPoint
	{
		public int time;
		public int spriteIndex;

		public AnimationPoint(int time, int spriteIndex)
		{
			this.time = time;
			this.spriteIndex = spriteIndex;
		}
	}
}
