using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_S1
{



    public class EnemySpriteComponent : IBaseComponent
    {

        //Animation/Sprites
        public Texture2D Texture;
        public Vec2 Position;
        public Color Color;

        private int frameColumn = 0;
        private int frameRow = 1;
        private double timer = 0;
        private double switchTime = 200;
        private int frameWidth;
        private int frameHeight;
        

        public EnemySpriteComponent(Texture2D Texture, Vec2 position, Color Color ) {
            this.Texture = Texture;
            this.Position = position;
            this.Color = Color;

            frameWidth = Texture.Width / 4; 
            frameHeight = Texture.Height / 3 ; 
        }

        public void Update(GameTime gameTime)
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
                return new Rectangle((int)Position.position.X, (int)Position.position.Y, frameWidth, frameHeight);
             }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(frameColumn * frameWidth, frameRow * frameHeight, frameWidth, frameHeight);
            
            
            int scale = 2;
            Rectangle destinationRectangle = new Rectangle((int)Position.position.X, (int)Position.position.Y, frameWidth * scale, frameHeight * scale);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color);
        }

    

    class MovementComponent : IBaseComponent
    {

        
        Enemy enemy;

        public MovementComponent(Enemy enemy) {
            this.enemy = enemy;
        }

        public void Update(GameTime gameTime)
        {}
        

        public void Draw(SpriteBatch spriteBatch) {}
    }
    }
}