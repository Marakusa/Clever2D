using System;

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
        /// The assigned SpriteArray to this SpriteRenderer.
        /// </summary>
        internal SpriteArray spriteArray;
        /// <summary>
        /// Returns the assigned image to this SpriteRenderer.
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
        /// Component that manages the Sprite wanted to be rendered in a GameObject.
        /// </summary>
        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
        }

        /// <summary>
        /// Component that manages the Sprite wanted to be rendered in a GameObject.
        /// </summary>
        public SpriteRenderer(SpriteArray spriteArray)
        {
            this.spriteArray = spriteArray;
            Sprite = this.spriteArray.Sprites[0];
        }

        internal override void Initialize()
        {

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
