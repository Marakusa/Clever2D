using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Clever2D.Core;
using Newtonsoft.Json;

namespace Clever2D.Engine
{
	public class AnimatorController : Component
	{
		private string animatorPath = "";
		private SpriteRenderer spriteRenderer;
		public Animator animator;

		public AnimatorController(string path)
		{
			animatorPath = Clever.executableDirectory + "/assets/" + path;
		}

		internal override void Initialize()
		{
			if (gameObject.GetComponent<SpriteRenderer>() != null)
			{
				spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			
				animator = new();

				List<Animator> items;
					
				using (StreamReader r = new StreamReader(animatorPath))
				{
					string json = r.ReadToEnd();
					items = JsonConvert.DeserializeObject<List<Animator>>(json);
				}
				
				if (items.Count > 0)
				{
					animator = items[0];

					Thread thread = new(() =>
					{
						animator.Start(spriteRenderer);
					});
					thread.Start();
				}
			}

			isInitialized = true;
		}
		
		public void SetBool(string name, bool value)
		{
			if (isInitialized)
				animator.SetCondition(name, value);
		}
		public void SetInt(string name, int value)
		{
			if (isInitialized)
				animator.SetCondition(name, value);
		}

		/// <summary>
		/// Disposes and destroys this Component.
		/// </summary>
		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
