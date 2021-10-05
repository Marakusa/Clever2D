using Clever2D.Engine;
using Clever2D.Core;
using Clever2D.UI;

namespace Example
{
	public class FPSCounter : CleverScript
	{
		private Text text;

		public override void Start()
		{
			text = gameObject.GetComponent<Text>();
		}

		public override void FixedUpdate()
		{
			text.text = Clever.FPS + " fps";
		}
	}
}
