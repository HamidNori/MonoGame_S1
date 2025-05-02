using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_S1
{
    
    interface IBaseComponent {
        void Update(GameTime gameTime);
    } 

    public class SpriteComponent : IBaseComponent
    {

        //Animation/Sprites
        public Texture2D Texture;
        public Vector2 Position;
        public Color Color;

        private int frameColumn = 0;
        private int frameRow = 0;
        private double timer = 0;
        private double switchTime = 100;
        private int frameWidth;
        private int frameHeight;
        

        public SpriteComponent(Texture2D Texture, Vector2 Position, Color Color ) {
            this.Texture = Texture;
            this.Position = Position;
            this.Color = Color;

            frameWidth = Texture.Width / 8; 
            frameHeight = Texture.Height / 8; 
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
                return new Rectangle((int)Position.X, (int)Position.Y, frameWidth, frameHeight);
             }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(frameColumn * frameWidth, frameRow * frameHeight, frameWidth, frameHeight);
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color);
        }

    }

    class MovementComponent : IBaseComponent
    {

        int Speed = 5;
        Player player;

        public MovementComponent(Player player) {
            this.player = player;
        }

        public void Update(GameTime gameTime)
        {
            
            KeyboardState kState = Keyboard.GetState();

            if(kState.IsKeyDown(Keys.D))
            {
                player.Position.X += 1;
            }
            if(kState.IsKeyDown(Keys.A))
            {
                player.Position.X -= 1;
            }
            if(kState.IsKeyDown(Keys.W))
            {
                player.Position.Y += 1;
            }
            if(kState.IsKeyDown(Keys.S))
            {
                player.Position.Y -= 1;
            }
       }
    }
}