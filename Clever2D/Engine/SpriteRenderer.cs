using Gtk;

namespace Clever2D.Engine
{
    /// <summary>
    /// Component that manages the Sprite wanted to be rendered in a GameObject.
    /// </summary>
    public class SpriteRenderer : Component
    {
        /// <summary>
        /// The assigned Sprite to this SpriteRenderer.
        /// </summary>
        private Sprite sprite;
        /// <summary>
        /// Returns the assigned Sprite to this SpriteRenderer.
        /// </summary>
        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                sprite = value;
            }
        }
        /// <summary>
        /// Returns the assigned Sprites image of this SpriteRenderer.
        /// </summary>
        public Image Image
        {
            get
            {
                return sprite.image;
            }
        }

        /// <summary>
        /// Component that manages the Sprite wanted to be rendered in a GameObject.
        /// </summary>
        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
        }
    }
}
