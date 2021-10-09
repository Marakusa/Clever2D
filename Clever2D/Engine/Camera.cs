using System;
using Clever2D.Core;
using Clever2D.UI;
using SDL2;

namespace Clever2D.Engine
{
	/// <summary>
	/// Camera renders the loaded Scene.
	/// </summary>
	public class Camera : CleverScript
	{
		/// <summary>
		/// Returns the global main camera.
		/// </summary>
		public static Camera MainCamera
		{
			get;
			private set;
		}

		/// <summary>
		/// Camera renders the loaded Scene.
		/// </summary>
		public Camera()
        {
	        if (Camera.MainCamera == null) MainCamera = this;
        }

		/// <summary>
		/// Update is called on every frame.
		/// </summary>
		protected override void Update()
		{
			if (Camera.MainCamera == null) MainCamera = this;
			
			IntPtr renderer = Clever.Renderer;
			
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(renderer);

            Scene loadedScene = SceneManager.LoadedScene;
            
            if (loadedScene is { IsInitialized: true })
            {
                var instances = loadedScene.SpawnedGameObjects;

                if (instances.Count > 0)
                {
	                // TODO: Rendering order

	                var zones = OcclusionManager.GetNearestAreas(transform.position);

	                foreach (var zone in zones)
	                {
		                foreach (SpriteRenderer spriteRenderer in zone.renderers)
		                {
			                if (spriteRenderer != null && spriteRenderer.Sprite != null)
			                {
				                float distance = Vector.Distance(spriteRenderer.transform.position, transform.position);
				                if (distance < OcclusionManager.areaSize)
				                {
					                float scale = (Clever.Size.Height / 600f) * 2f;

					                float cameraOffsetX = scale * transform.position.x - Clever.Size.Width / 2f;
					                float cameraOffsetY = scale * -transform.position.y - Clever.Size.Height / 2f;

					                float scaledX = spriteRenderer.transform.position.x * scale;
					                float scaledY = -spriteRenderer.transform.position.y * scale;
					                float posX = scaledX * spriteRenderer.transform.position.x - cameraOffsetX;
					                float posY = -spriteRenderer.transform.position.y * scale * spriteRenderer.transform.position.y - cameraOffsetY;

					                float x, y, w, h;

					                SDL.SDL_Rect tRect;
					                w = spriteRenderer.Sprite.rect.w * scale * spriteRenderer.transform.scale.x;
					                h = spriteRenderer.Sprite.rect.h * scale * spriteRenderer.transform.scale.y;

					                if (spriteRenderer.gameObject.parent != null)
					                {
						                x = posX + spriteRenderer.gameObject.parent.transform.position.x * scale * spriteRenderer.gameObject.parent.transform.scale.x +
							                scale * spriteRenderer.transform.position.x - cameraOffsetX;
						                y = posY - spriteRenderer.gameObject.parent.transform.position.y * scale * spriteRenderer.gameObject.parent.transform.scale.y +
							                scale * -spriteRenderer.transform.position.y - cameraOffsetY;
						                w *= spriteRenderer.Sprite.rect.w * spriteRenderer.transform.scale.x;
						                h *= spriteRenderer.Sprite.rect.h * spriteRenderer.transform.scale.y;
					                }
					                else
					                {
						                x = scaledX * spriteRenderer.transform.scale.x - cameraOffsetX;
						                y = scaledY * spriteRenderer.transform.scale.y - cameraOffsetY;
					                }

					                float pivotOffsetX = spriteRenderer.Sprite.rect.w * spriteRenderer.Sprite.pivot.x * scale;
					                float pivotOffsetY = spriteRenderer.Sprite.rect.h * spriteRenderer.Sprite.pivot.y * scale;

					                x -= pivotOffsetX;
					                y -= pivotOffsetY;

					                tRect.x = (int)Math.Round(x);
					                tRect.y = (int)Math.Round(y);
					                tRect.w = (int)Math.Round(w);
					                tRect.h = (int)Math.Round(h);

					                SDL.SDL_RenderCopy(renderer, spriteRenderer.Sprite.image, ref spriteRenderer.Sprite.rect, ref tRect);
				                }
			                }
		                }
	                }

	                foreach (SpriteRenderer spriteRenderer in OcclusionManager.NonStaticRenderers)
	                {
		                if (spriteRenderer != null && spriteRenderer.Sprite != null)
		                {
			                float scale = (Clever.Size.Height / 600f) * 2f;

			                float cameraOffsetX = scale * transform.position.x - Clever.Size.Width / 2f;
			                float cameraOffsetY = scale * -transform.position.y - Clever.Size.Height / 2f;

			                float scaledX = spriteRenderer.transform.position.x * scale;
			                float scaledY = -spriteRenderer.transform.position.y * scale;
			                float posX = scaledX * spriteRenderer.transform.position.x - cameraOffsetX;
			                float posY = -spriteRenderer.transform.position.y * scale * spriteRenderer.transform.position.y - cameraOffsetY;

			                float x, y, w, h;

			                SDL.SDL_Rect tRect;
			                w = spriteRenderer.Sprite.rect.w * scale * spriteRenderer.transform.scale.x;
			                h = spriteRenderer.Sprite.rect.h * scale * spriteRenderer.transform.scale.y;

			                if (spriteRenderer.gameObject.parent != null)
			                {
				                x = posX + spriteRenderer.gameObject.parent.transform.position.x * scale * spriteRenderer.gameObject.parent.transform.scale.x;
				                y = posY - spriteRenderer.gameObject.parent.transform.position.y * scale * spriteRenderer.gameObject.parent.transform.scale.y;
				                w *= spriteRenderer.gameObject.parent.transform.scale.x * spriteRenderer.transform.scale.x;
				                h *= spriteRenderer.gameObject.parent.transform.scale.y * spriteRenderer.transform.scale.y;
			                }
			                else
			                {
				                x = scaledX * spriteRenderer.transform.scale.x - cameraOffsetX;
				                y = scaledY * spriteRenderer.transform.scale.y - cameraOffsetY;
			                }

			                float pivotOffsetX = spriteRenderer.Sprite.rect.w * spriteRenderer.Sprite.pivot.x * scale;
			                float pivotOffsetY = spriteRenderer.Sprite.rect.h * spriteRenderer.Sprite.pivot.y * scale;

			                x -= pivotOffsetX;
			                y -= pivotOffsetY;

			                tRect.x = (int)Math.Round(x);
			                tRect.y = (int)Math.Round(y);
			                tRect.w = (int)Math.Round(w);
			                tRect.h = (int)Math.Round(h);

			                SDL.SDL_RenderCopy(renderer, spriteRenderer.Sprite.image, ref spriteRenderer.Sprite.rect, ref tRect);
		                }
	                }

	                foreach (var instance in instances)
	                {
		                UIElement uiElement = instance.Value.GetComponent<UIElement>();

		                if (uiElement != null)
		                {
			                uiElement.Render();
		                }
	                }
                }
            }
            
            SDL.SDL_RenderPresent(renderer);
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
