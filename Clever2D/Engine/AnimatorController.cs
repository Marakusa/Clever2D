using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;
using Clever2D.Core;
using Newtonsoft.Json;

namespace Clever2D.Engine
{
	public class AnimatorController : Component
	{
		private string animatorPath = "";
		private SpriteRenderer spriteRenderer;

		internal override void Initialize()
		{
			if (gameObject.GetComponent<SpriteRenderer>() != null)
			{
				spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			
				Animator animator = new();

				List<Animator> items;
					
				using (StreamReader r = new StreamReader(animatorPath))
				{
					string json = r.ReadToEnd();
					items = JsonConvert.DeserializeObject<List<Animator>>(json);
				}
				
				if (items.Count > 0)
				{
					Thread thread = new(() =>
					{
						int nextFrameIndex = 0;
						int animationKeyCount = items[0].animations[0].timeline.Length;
						bool frameDone = true;

						while (!Clever.Quit)
						{
							if (frameDone)
							{
								frameDone = false;

								int i = nextFrameIndex;
								nextFrameIndex = nextFrameIndex + 1 >= animationKeyCount ? 0 : nextFrameIndex + 1;

								int interval = items[0].animations[0].timeline[nextFrameIndex].time - items[0].animations[0].timeline[i].time;
								interval = interval < 0 ? 1 : interval;
								
								System.Timers.Timer timer = new() { Interval = interval };
								timer.Elapsed += (object sender, ElapsedEventArgs e) =>
								{
									timer.Stop();
									spriteRenderer.Sprite = spriteRenderer.spriteArray.Sprites[items[0].animations[0].timeline[nextFrameIndex].spriteIndex];
									frameDone = true;
								};
								timer.Start();
							}
						}
					});
					thread.Start();
				}
			}
		}
		
		public AnimatorController(string path)
		{
			animatorPath = Clever.executableDirectory + "/assets/" + path;
		}
	}
}
