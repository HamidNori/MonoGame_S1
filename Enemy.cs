using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MonoGame_S1
{
    public class Enemy
    {
        private List<IBaseComponent> Components = new List<IBaseComponent>();
        private Vec2 position = new Vec2();
        public Enemy(Texture2D texture, Vector2 position, Color color) {
        this.position.position = position;
        Components.Add(new EnemySpriteComponent(texture, this.position, color));
        }

        public Vector2 Position 
        {
            get {return position.position; }
            set {position.position = value; }
        }

        public void Update(GameTime gameTime)
        {
            foreach(IBaseComponent component in Components)
            {
                component.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Components[0].Draw(spriteBatch);
        }


        }
    }
