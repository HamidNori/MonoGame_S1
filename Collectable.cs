using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonoGame_S1
{
    public class Collectable
    {
        public String Name { get; set; }
        public int ScoreValue { get; private set; }
        public bool Collected { get; private set; }
        public Vector2 Position { get; private set; }
        public Texture2D Texture { get; private set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);


        private int frameWidth;
        private int frameHeight;
        private int frameCount;
        private int currentFrame;
        private float timer;
        private float frameTime;
        public Collectable(string name, int scoreValue, Vector2 position, Texture2D texture, int frameCount, float framrTime)
        {
            Name = name;
            ScoreValue = scoreValue;
            Position = position;
            Texture = texture;
            Collected = false;

            this.frameCount = frameCount;
            this.frameTime = framrTime;

            frameWidth = texture.Width / frameCount;
            frameHeight = texture.Height;

            currentFrame = 0;
            timer = 0f;
        }
        public int Collect()
        {
            if (!Collected)
            {
                Collected = true;
                return ScoreValue;
            }
            return 0;
        }

        public void Update(GameTime gameTime)
        {
            if (!Collected)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer >= frameTime)
                {
                    currentFrame = (currentFrame + 1) % frameCount;
                    timer = 0f;
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Collected)
            {
                var sourceRectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
                spriteBatch.Draw(Texture, Position, sourceRectangle, Color.White);
            }
        }

        public void DrawScore(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (Collected)
            {
                var scourePosition = new Vector2(Position.X + Texture.Width / 2, Position.Y - 20);
                var scoreText = "+" + ScoreValue.ToString();
                spriteBatch.DrawString(font, scoreText, scourePosition, Color.Yellow);
            }

        }

    }
}