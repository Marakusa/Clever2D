using System;
using Clever2D.Core;
using Clever2D.UI;
using SDL2;

namespace Clever2D.Engine
{
	public class Camera : CleverScript
	{
		private static Camera mainCamera;
		public static Camera MainCamera
		{
			get
			{
				return mainCamera;
			}
		}
		
		public override void Start()
		{
			mainCamera = this;
		}
		
		public override void Update()
		{
			IntPtr renderer = Clever.Renderer;
			
            SDL.SDL_SetRenderDrawColor(renderer, 30, 30, 100, 255);
            SDL.SDL_RenderClear(renderer);

            Scene loadedScene = SceneManager.LoadedScene;
            
            if (loadedScene != null)
            {
                var instances = loadedScene.SpawnedGameObjects;

                if (instances.Count > 0)
                {
                    foreach (var instance in instances)
                    {
                        SpriteRenderer spriteRenderer = instance.Value.GetComponent<SpriteRenderer>();
                        
                        if (spriteRenderer != null)
                        {
                            float scale = (Clever.Size.Height / 600f) * 2f;

                            float x, y, w, h;
                            
                            SDL.SDL_Rect tRect;
                            w = spriteRenderer.Sprite.rect.w * scale * instance.Value.transform.Scale.x;
                            h = spriteRenderer.Sprite.rect.h * scale * instance.Value.transform.Scale.y;

                            float cameraOffsetX = scale * transform.Position.x - Clever.Size.Width / 2f;
                            float cameraOffsetY = scale * -transform.Position.y - Clever.Size.Height / 2f;

                            float scaledX = instance.Value.transform.Position.x * scale;
                            float scaledY = -instance.Value.transform.Position.y * scale;
                            float posX = scaledX * instance.Value.transform.Position.x - cameraOffsetX;
                            float posY = -instance.Value.transform.Position.y * scale * instance.Value.transform.Position.y - cameraOffsetY;

                            if (instance.Value.parent != null)
                            {
                                x = posX + instance.Value.parent.transform.Position.x * scale * instance.Value.parent.transform.Scale.x + scale * instance.Value.transform.Position.x - cameraOffsetX;
                                y = posY - instance.Value.parent.transform.Position.y * scale * instance.Value.parent.transform.Scale.y + scale * -instance.Value.transform.Position.y - cameraOffsetY;
                                w *= spriteRenderer.Sprite.rect.w * instance.Value.transform.Scale.x;
                                h *= spriteRenderer.Sprite.rect.h * instance.Value.transform.Scale.y;
                            }
                            else
                            {
                                x = scaledX * instance.Value.transform.Scale.x - cameraOffsetX;
                                y = scaledY * instance.Value.transform.Scale.y - cameraOffsetY;
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
	}
}
