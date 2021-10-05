using Clever2D.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Clever2D.Engine
{
	public class Animator
	{
		public Animation[] animations;
		public AnimationCondition[] conditions;

		public int start;
		public int activeAnimation;

		private int lastAnimation = -1;

		internal void Start(SpriteRenderer renderer)
		{
			int nextFrameIndex = 0;
			int animationKeyCount = animations[activeAnimation].timeline.Length;
			bool frameDone = true;

			while (!Clever.Quit)
			{
				if (lastAnimation != activeAnimation)
                {
					lastAnimation = activeAnimation;
					nextFrameIndex = 0;
					animationKeyCount = animations[activeAnimation].timeline.Length;
					frameDone = true;
				}

				if (frameDone)
				{
					frameDone = false;

					int i = nextFrameIndex;
					nextFrameIndex = nextFrameIndex + 1 >= animationKeyCount ? 0 : nextFrameIndex + 1;

					int interval = animations[activeAnimation].timeline[nextFrameIndex].time - animations[activeAnimation].timeline[i].time;
					interval = interval <= 0 ? 1 : interval;

					System.Timers.Timer timer = new() { Interval = interval };
					timer.Elapsed += (object sender, ElapsedEventArgs e) =>
					{
						timer.Stop();
						renderer.Sprite = renderer.spriteArray.Sprites[animations[activeAnimation].timeline[nextFrameIndex].spriteIndex];
						frameDone = true;
					};
					timer.Start();
				}
			}
		}

		internal void SetCondition(string name, object value)
        {
			foreach (var condition in conditions)
			{
				if (name == condition.name)
				{
					Type ct = condition.value.GetType();
					Type vt = value.GetType();

					if (ct.Name.StartsWith("Int"))
						ct = typeof(int);
					if (vt.Name.StartsWith("Int"))
						vt = typeof(int);

					if (ct.Name.ToString() == vt.Name.ToString())
                    {
						if (!condition.value.Equals(value))
						{
							condition.value = value;

							List<AnimationCondition> conds = conditions.ToList();

							foreach (AnimationTransition transition in animations[lastAnimation].transitions)
							{
								bool meetsConditions = true;

								foreach (AnimationTransitionCondition con in transition.conditions)
								{
									AnimationCondition c = conds.Find(f => f.name == con.name);
									if (c != null)
									{
										if (c.value != con.value)
										{
											meetsConditions = false;
											break;
										}
										else
										{
											activeAnimation = transition.to;
										}
									}
								}

								if (meetsConditions)
									break;
							}
						}
					}
                    else
                    {
						Player.LogWarn($"Invalid value type for {condition.name} (Type of given value: {value.GetType()}, type of condition: {condition.value.GetType()})");
						break;
                    }
                }
            }
        }
	}

	public class AnimationCondition
	{
		public string name;
		public object value;
	}
}
