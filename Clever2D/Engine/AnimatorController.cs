using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Clever2D.Engine
{
	/// <summary>
	/// The controller of the animators animations.
	/// </summary>
	public class AnimatorController : Component
	{
		private string _animatorPath;
		
		/// <summary>
		/// The animator assigned to this controller.
		/// </summary>
		public Animator animator;
		
		private SpriteRenderer spriteRenderer;
		
		/// <summary>
		/// The controller of the animators animations.
		/// </summary>
		public AnimatorController(string path)
		{
			_animatorPath = Application.ExecutableDirectory + "/assets/" + path;
		}

		internal override void Initialize()
		{
			if (gameObject.GetComponent<SpriteRenderer>() != null)
			{
				spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			
				animator = new();

				List<Animator> items;
				
				using (StreamReader r = new StreamReader(_animatorPath))
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
		
		/// <summary>
		/// Set an animators bool conditions value.
		/// </summary>
		/// <param name="name">Animator condition name.</param>
		/// <param name="value">Condition value.</param>
		public void SetBool(string name, bool value)
		{
			if (isInitialized)
				animator.SetCondition(name, value);
		}
		/// <summary>
		/// Set an animators int conditions value.
		/// </summary>
		/// <param name="name">Animator condition name.</param>
		/// <param name="value">Condition value.</param>
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
