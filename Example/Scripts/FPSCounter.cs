using Clever2D.Engine;
using Clever2D.Core;
using Clever2D.UI;

namespace Example
{
	public class FPSCounter : CleverScript
	{
		private Text text;

		protected override void Start()
		{
			text = gameObject.GetComponent<Text>();
		}

		protected override void FixedUpdate()
		{
			text.text = Clever.Fps + " fps";
		}
	}
}
