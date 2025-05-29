using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_S1
{
    public class Player
    {
        private List<IBaseComponent> Components = new List<IBaseComponent>();
        public Vec2 position = new Vec2();
        public Vector2 velocity = new Vector2(0,0);
        public PlayerSpriteComponent spriteComponent;
        public Rectangle collisionRectangle => spriteComponent.collisionRectangle;

        
        public Player(Texture2D texture, Vector2 position, Color color) {
            this.position.position = position;
            spriteComponent = new PlayerSpriteComponent(texture, this.position, color);
            Components.Add(new PlayerSpriteComponent(texture, this.position, color));
            Components.Add(new MovementComponent(this));
        }
        public Vector2 Position 
        {
            get {return position.position; }
            set {position.position = value; }
        }
        public void Update(GameTime gameTime)
        {
            spriteComponent.Update(gameTime);
            foreach(IBaseComponent component in Components)
            {
                component.Update(gameTime);
            }
        }

        public T GetComponent<T>() where T : IBaseComponent
        {
            foreach (IBaseComponent component in Components)
            {
                if (component is T)
                {
                    return (T)component;
                }
            }
            return default(T);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Components[0].Draw(spriteBatch);
            spriteComponent.Draw(spriteBatch);

        }


        }
    }

