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
        public bool hasDied = false;
        public float deathTimer = 0f;
        public float respawnDelay = 1.0f; // sekunder
        public Vector2 spawnPosition = new Vector2(100, 300); // eller där du vill starta

        public Rectangle collisionRectangle => spriteComponent.collisionRectangle;

        public Player(Texture2D texture, Vector2 position, Color color) {
            this.position.position = position;
            spawnPosition = position;
            spriteComponent = new PlayerSpriteComponent(texture, this.position, color);
            Components.Add(spriteComponent);
            Components.Add(new MovementComponent(this));
        }

        public Vector2 Position 
        {
            get {return position.position; }
            set {position.position = value; }
        }

        public void Update(GameTime gameTime)
        {
            if (hasDied)
            {
                deathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (deathTimer >= respawnDelay)
                {
                    Respawn();
                }
                return; // Hoppa över övrig uppdatering när död
            }

            spriteComponent.Update(gameTime);
            foreach(IBaseComponent component in Components)
            {
                component.Update(gameTime);
            }
        }

        public void Respawn()
        {
            hasDied = false;
            deathTimer = 0f;
            Position = spawnPosition;
            velocity = Vector2.Zero;
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
            spriteComponent.Draw(spriteBatch);
        }
    }
}

