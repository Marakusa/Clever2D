using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Clever2D.Engine
{
	/// <summary>
	/// Collider manages colliding with other Colliders.
	/// </summary>
	public class Collider : Component
	{
		/// <summary>
		/// Size of this collider.
		/// </summary>
		[JsonProperty]
		public Vector2 size;
		/// <summary>
		/// Offset of this collider.
		/// </summary>
		[JsonProperty]
		public Vector2 offset;

		internal override void Initialize() { }

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
