using System.Collections.Generic;
using System.IO;

namespace Clever2D.Engine
{
	public class CollisionManager
	{
		private Dictionary<int, Collider> collisionAreas = new();
		private float areaSize = 1f;
	}
}
