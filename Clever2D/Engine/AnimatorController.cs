using System;
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
		/// <summary>
		/// Assigned animators file path.
		/// </summary>
		[JsonProperty]
		private string animatorPath;
		
		/// <summary>
		/// The animator assigned to this controller.
		/// </summary>
		public Animator animator;
		
		private SpriteRenderer spriteRenderer;
		
		private AnimatorController() { }
		
		/// <summary>
		/// The controller of the animators animations.
		/// </summary>
		public AnimatorController(string path)
		{
			animatorPath = path;
		}

		internal override void Initialize()
		{
			if (gameObject.GetComponent<SpriteRenderer>() != null)
			{
				spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			
				animator = new();

				string path = $"{Application.ExecutableDirectory}/assets/{animatorPath}";

				if (File.Exists(path))
				{
					string json = File.ReadAllText(path);
					animator = JsonConvert.DeserializeObject<Animator>(json);

					if (animator != null)
					{
						Thread thread = new(() =>
						{
							animator.Start(spriteRenderer);
						});
						thread.Start();
					}
				}
				else
				{
					Player.LogError($"File named {path} doesn't exist.");
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
