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
            {
                Sprite = new(sprite.Path, sprite.pivot, sprite.size, sprite.offset);
                Sprite.OnChanged += (object sender) => OnChanged.Invoke(this, new()
                {
                    renderer = this
                });
            }
            else if (spriteArray != null)
            {
                spriteArray = new(spriteArray.spritePath, spriteArray.pivot, spriteArray.rows, spriteArray.columns);

                foreach (var sprite in spriteArray.Sprites)
                {
                    sprite.OnChanged += (object sender) => OnChanged.Invoke(this, new()
                    {
                        renderer = this
                    });
                }

                Sprite = spriteArray.Sprites[0];
            }
        }

        /// <summary>
        /// When this SpriteRenderer changes or is removed, this will get called.
        /// </summary>
        public delegate void SpriteRendererChanged(object sender, SpriteRendererChangedEventArgs e);
        /// <summary>
        /// If this SpriteRenderer has changed or removed, this will get called.
        /// </summary>
        public event SpriteRendererChanged OnChanged;

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
            OnChanged.Invoke(this, new()
            {
                renderer = this
            });
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Event arguments for Sprite batch changed.
    /// </summary>
    public class SpriteRendererChangedEventArgs
    {
        /// <summary>
        /// Renderer changed. If null renderer was removed.
        /// </summary>
        public SpriteRenderer renderer;
    }
}
