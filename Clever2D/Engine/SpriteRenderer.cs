using System;
using Newtonsoft.Json;

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
        [JsonProperty]
        public Sprite sprite;
        /// <summary>
        /// The assigned SpriteArray to this SpriteRenderer.
        /// </summary>
        [JsonProperty]
        public SpriteArray spriteArray;
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

        internal override void Initialize()
        {
            if (sprite != null)
                Sprite = new(sprite.Path, sprite.pivot, sprite.size, sprite.offset);
            else if (spriteArray != null)
            {
                this.spriteArray = new(spriteArray.spritePath, spriteArray.pivot, spriteArray.rows, spriteArray.columns);
                Sprite = this.spriteArray.Sprites[0];
            }
        }

        [JsonConstructor]
        private SpriteRenderer() { }

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
