using Clever2D.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;

namespace Clever2D.Engine
{
	/// <summary>
	/// Animator is the base of an GameObjects sprite animations.
	/// </summary>
	public class Animator
	{
		/// <summary>
		/// Animators animations.
		/// </summary>
		[JsonProperty]
		private Animation[] animations;
		/// <summary>
		/// Animators conditions.
		/// </summary>
		[JsonProperty]
		private AnimationCondition[] conditions;

		/// <summary>
		/// Currently active animation.
		/// </summary>
		private int activeAnimation;

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
				if (animations.Length <= 0)
					continue;

				if (lastAnimation != activeAnimation)
				{
					lastAnimation = activeAnimation;
					nextFrameIndex = 0;
					animationKeyCount = animations[activeAnimation].timeline.Length;
					frameDone = true;
				}

				if (!frameDone)
					continue;

				frameDone = false;

				var i = nextFrameIndex;
				nextFrameIndex = nextFrameIndex + 1 >= animationKeyCount ? 0 : nextFrameIndex + 1;

				if (activeAnimation >= animations.Length)
					activeAnimation = 0;
						
				if (nextFrameIndex >= animations[activeAnimation].timeline.Length)
					nextFrameIndex = 0;
				if (i >= animations[activeAnimation].timeline.Length)
					i = 0;
						
				var interval = animations[activeAnimation].timeline[nextFrameIndex].time - animations[activeAnimation].timeline[i].time;
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

		internal void SetCondition(string name, object value)
        {
			foreach (var condition in conditions)
			{
				if (name == condition.name)
				{
					var ct = condition.value.GetType();
					var vt = value.GetType();

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

							var conditionsList = conditions.ToList();

							if (animations.Length > lastAnimation || lastAnimation < 0)
							{
								lastAnimation = 0;
							}

							foreach (AnimationTransition transition in animations[lastAnimation].transitions)
							{
								bool meetsConditions = true;

								foreach (AnimationTransitionCondition con in transition.conditions)
								{
									var c = conditionsList.Find(f => f.name == con.name);
									if (c == null)
										continue;

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
