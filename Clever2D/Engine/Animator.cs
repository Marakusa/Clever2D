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

		public int activeAnimation = 0;

		private int lastAnimation = -1;

		private Timer frameTimer = null;
		private	bool frameDone = true;
		private	int nextFrameIndex = 0;
		private int animationKeyCount = 0;

		internal void Start(SpriteRenderer renderer)
		{
			activeAnimation = 0;
			lastAnimation = -1;
			
			while (!Clever.Quit)
			{
				if (animations.Length > 0)
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

						if (activeAnimation >= animations.Length)
							activeAnimation = 0;
						
						if (nextFrameIndex >= animations[activeAnimation].timeline.Length)
							nextFrameIndex = 0;
						if (i >= animations[activeAnimation].timeline.Length)
							i = 0;
						
						int interval = animations[activeAnimation].timeline[nextFrameIndex].time - animations[activeAnimation].timeline[i].time;
						interval = interval <= 0 ? 1 : interval;

						if (frameTimer != null)
						{
							frameTimer.Stop();
							frameTimer.Close();
							frameTimer.Dispose();
						}

						frameTimer = new() { Interval = interval };
						frameTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
						{
							frameTimer.Stop();
							renderer.Sprite = renderer.spriteArray.Sprites[animations[activeAnimation].timeline[nextFrameIndex].spriteIndex];
							frameDone = true;
							frameTimer.Dispose();
						};

						frameTimer.Start();
					}
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
							frameDone = true;
							
							lastAnimation = -1;
							activeAnimation = 0;
							nextFrameIndex = 0;
							frameDone = true;
							
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
										if (c.value.ToString() != con.value.ToString())
										{
											meetsConditions = false;
										}
										else
										{
											activeAnimation = transition.to;
											break;
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
}
