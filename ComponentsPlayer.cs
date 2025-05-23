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

    public interface IBaseComponent
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
        private int frameHeight ;
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
                return new Rectangle(
                    (int)Position.position.X,
                    (int)Position.position.Y,
                    frameWidth * scale,
                    frameHeight * scale);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(
                frameColumn * frameWidth,
                frameRow * frameHeight,
                frameWidth,
                frameHeight);
            
            
            spriteBatch.Draw(
                Texture,
                DestinationRectangle,
                sourceRectangle,
                Color,
                0f,
                Vector2.Zero,
                spriteEffect, 0f);        }

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
        int gravity = 1;        // Ökad gravitation
        int movementSpeed = 5;   // Samma rörelsehastighet
        int jumpPower = -20;      // Starkare hopp
        public bool Grounded { get; set; }
        Player player;

        public KeyboardState kState;
        public KeyboardState oldKState;

        //Dashing
        public bool isDashing = false;
        public int dashSpeed = 20;
        public float dashDuration = 0.2f;
        public float dashTimer = 0.2f;
        public float dashCoolDown = 0.2f;
        public float dashCoolDownTimer = 0.3f;
        public int facingDirection = 1; // 1 = höger, -1 = vänster

        private KeyboardState previousKState;

        public MovementComponent(Player player)
        {
            this.player = player;
            previousKState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            //Fysiken

            //Statements
            KeyboardState kState = Keyboard.GetState();
            Vector2 movement = new Vector2(0, player.velocity.Y);

            // Normal rörelse
            if (kState.IsKeyDown(Keys.D))
            {
                facingDirection = 1;
                movement.X = movementSpeed;
            }
            else if (kState.IsKeyDown(Keys.A))
            {
                facingDirection = -1;
                movement.X = -movementSpeed;
            }

            // Dash kontroll
            if (kState.IsKeyDown(Keys.W) && 
                previousKState.IsKeyUp(Keys.W) && 
                dashCoolDownTimer <= 0 && 
                !isDashing)
            {
                isDashing = true;
                dashTimer = dashDuration;
                dashCoolDownTimer = dashCoolDown;
            }

            // Dash hantering
            if (isDashing)
            {
                movement.X = dashSpeed * facingDirection;
                movement.Y = 0;
                dashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (dashTimer <= 0)
                {
                    isDashing = false;
                }
            }

            // Hopp kontroll - ändrad för att använda previousKState
            if (kState.IsKeyDown(Keys.Space)
                && Grounded
                && !isDashing)
            {
                movement.Y = jumpPower;
                Grounded = false;
            }

            // Gravitation
            if (!Grounded)
            {
                movement.Y += gravity;
            }

            // Dash cooldown
            if (dashCoolDownTimer > 0)
            {
                dashCoolDownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Uppdatera previous state
            previousKState = kState;
            
            player.velocity = movement;
        }

        public void SetGrounded(bool grounded)
        {
            Grounded = grounded;
            if (Grounded)
            {
                player.velocity.Y = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch) { }
    }
}