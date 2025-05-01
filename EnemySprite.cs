using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_S1
{
    public class EnemySprite
    {
         //Animation
        public Texture2D Texture;
        public Vector2 Position;
        public Color Color;

        private int frameColumn = 0;
        private int frameRow = 1;
        private double timer = 0;
        private double switchTime = 200;
        private int frameWidth;
        private int frameHeight;

        //Egenskaper och rörelse
        
        public EnemySprite(Texture2D Texture, Vector2 Position, Color Color ) {
            this.Texture = Texture;
            this.Position = Position;
            this.Color = Color;
            

            frameWidth = Texture.Width / 4; 
            frameHeight = Texture.Height / 3; 
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer >= switchTime)
            {
                frameColumn++;
                if(frameColumn >= 4)
                    frameColumn = 0;
                timer = 0;
                
            }
        }


        public Rectangle Rect {
            get 
             {
                return new Rectangle((int)Position.X, (int)Position.Y, frameWidth, frameHeight);
             }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(frameColumn * frameWidth, frameRow * frameHeight, frameWidth, frameHeight);
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color);
        }

        
    }
}