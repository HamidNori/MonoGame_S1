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
                    frameRow = 0; 
                    break;
                case AnimationState.Run:
                    frameRow = 2; 
                    break;
                case AnimationState.Roll:
                    frameRow = 5;
                    break;
                case AnimationState.Hit:
                    frameRow = 6; 
                    break;
                case AnimationState.Death:
                    frameRow = 7; 
                    break;
                default:
                    frameRow = 0; 
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
                currentAnimationState = AnimationState.Roll; 
            }
            else
            {
                currentAnimationState = AnimationState.Idle; 
            }

            Position.position += movement;
        }
        public Rectangle collisionRectangle
        {
            get
            {
                int width = frameWidth * 1; 
                int height = (int)(frameHeight * 1.3); 
                int offsetX = (frameWidth * 4 - width) / 2;
                int offsetY = (frameHeight * 4 - height) - 17; 

                return new Rectangle(
                    (int)Position.position.X + offsetX,
                    (int)Position.position.Y + offsetY,
                    width,
                    height
                );
            }
        }
        public Rectangle DestinationRectangle
        {
            get
            {
                return new Rectangle(
                    (int)Position.position.X,
                    (int)Position.position.Y,
                    frameWidth * 4,   
                    frameHeight * 4  
                );
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
                spriteEffect, 0f);
        }

    }

    public class FollowCamera
    {
        public Vector2 position;

        public FollowCamera(Vector2 position)
        {
            this.position = position;
        }

        public void Follow(Rectangle target, Vector2 screenSize, int mapWidth, int mapHeight)
        {

            float cameraX = -target.X + (screenSize.X / 2 - target.Width / 2);
            float cameraY = -target.Y + (screenSize.Y / 2 - target.Height / 2);

            float minX = -(mapWidth - screenSize.X);
            float minY = -(mapHeight - screenSize.Y);

            cameraX = MathHelper.Clamp(cameraX, minX, 0);
            cameraY = MathHelper.Clamp(cameraY, minY, 0);

            position = new Vector2(cameraX, cameraY);
        }

        public Matrix Transform => Matrix.CreateTranslation(new Vector3(position, 0));
    }

    class MovementComponent : IBaseComponent
    {
        int gravity = 1;        
        int movementSpeed = 5;   
        int jumpPower = -20;     
        public bool Grounded { get; set; }
        Player player;

        //Dashing
        public bool isDashing = false;
        public int dashSpeed = 20;
        public float dashDuration = 0.2f;
        public float dashTimer = 0.2f;
        public float dashCoolDown = 0.2f;
        public float dashCoolDownTimer = 0.3f;
        public int facingDirection = 1;

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

            // Hopp kontroll
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