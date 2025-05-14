using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_S1
{
    
    public enum AnimationState 
    {
        Idle,
        Run,
        Roll,
        Hit,
        Death
    }

    interface IBaseComponent 
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    } 

    public class Vec2
    {
        public Vector2 position;
        public Vector2 velocity;

    }

    public class PlayerSpriteComponent : IBaseComponent
    {

        //Animation/Sprites
        public Texture2D Texture;
        public Vec2 Position;
        public Color Color;

        

        

        private int frameColumn = 0;
        private int frameRow = 0;
        private double timer = 0;
        private double switchTime = 200;
        private int frameWidth;
        private int frameHeight;
        private SpriteEffects spriteEffect = SpriteEffects.None;

        
        //variable för nuvarande animation
        private AnimationState currentAnimationState = AnimationState.Idle;
        private Vector2 movement;

        public PlayerSpriteComponent(Texture2D Texture, Vec2 Position, Color Color ) {
            this.Texture = Texture;
            this.Position = Position;
            this.Color = Color;

            frameWidth = Texture.Width / 8; 
            frameHeight = Texture.Height / 8; 
        }

        public void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
            UpdateMovement();
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


            //Hämta animation fårn bilden
            switch (currentAnimationState)
            {
                case AnimationState.Idle:
                    frameRow = 0; // Idle animation row
                    break;
                case AnimationState.Run:
                    frameRow = 2; // Run animation row
                    break;
                case AnimationState.Roll:
                    frameRow = 5; // Roll animation row
                    break;
                case AnimationState.Hit:
                    frameRow = 6; // Hit animation row
                    break;
                case AnimationState.Death:
                    frameRow = 7; // Death animation row
                    break;
                default:
                    frameRow = 0; // Default to idle if no state matches
                    break;
            }
        }

        private void UpdateMovement()
        {
            //Uppdatera spelarens rörelse och animationstillstånd
            KeyboardState kState = Keyboard.GetState();
            movement = new Vector2(0, 0);

            if (kState.IsKeyDown(Keys.D))
            {
                currentAnimationState = AnimationState.Run; 
                spriteEffect = SpriteEffects.None; 
            }
            else if (kState.IsKeyDown(Keys.A))
            {
                currentAnimationState = AnimationState.Run; 
                spriteEffect = SpriteEffects.FlipHorizontally; // Spegelvänd bilden

            }
            else if (kState.IsKeyDown(Keys.W))
            {
                currentAnimationState = AnimationState.Run; 
            }
            else if (kState.IsKeyDown(Keys.S))
            {
                currentAnimationState = AnimationState.Run; 
            }
            else
            {
                currentAnimationState = AnimationState.Idle; 
            }

            Position.position += movement;
        }
        public Rectangle DestinationRectangle
        {
            get
            {
                int scale = 4;
                return new Rectangle((int)Position.position.X, (int)Position.position.Y, frameWidth * scale, frameHeight * scale);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(frameColumn * frameWidth, frameRow * frameHeight, frameWidth, frameHeight);
            
            
            spriteBatch.Draw(Texture, DestinationRectangle, sourceRectangle, Color, 0f, Vector2.Zero, spriteEffect, 0f);        }

    }

    public class FollowCamera
    {
        public Vector2 position;

        public FollowCamera(Vector2 position)
        {
            this.position = position;
        }

        public void Follow(Rectangle target, Vector2 screenSize)
        {
            position = new Vector2 (
                -target.X + (screenSize.X / 2 - target.Width / 2),
                -target.Y + (screenSize.Y / 2 - target.Height / 2)
            );
        }

        public Matrix Transform => Matrix.CreateTranslation(new Vector3(position, 0));
    }

    class MovementComponent : IBaseComponent
    {

        int Speed = 5;
        int jumpPower = 10;
        Player player;
        public Vector2 camera;

        public MovementComponent(Player player) {
            this.player = player;
        }

        public void Update(GameTime gameTime)
        {
            
            KeyboardState kState = Keyboard.GetState();
            Vector2 movement = player.velocity;

            movement.X  = 0;

            if(kState.IsKeyDown(Keys.D))
            {
                movement.X = Speed;
                camera.X -= Speed;                

            }
            if(kState.IsKeyDown(Keys.A))
            {
                movement.X = -Speed;
                camera.X += Speed;                

            }
            if(kState.IsKeyDown(Keys.W))
            {
                movement.Y = -jumpPower;              

            }
            if (kState.IsKeyDown(Keys.S))
            {
                movement.Y = Speed;                
            }
            // movement.Y += 9.82f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.velocity = movement;
            player.Position += movement;
       }

       public void Draw(SpriteBatch spriteBatch) { }
    }
}