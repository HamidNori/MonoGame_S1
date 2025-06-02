using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MonoGame_S1
{

    /// <summary>
    /// TileMap komponeneter och annat
    /// </summary>
    public class TileMaps
    {
        public Dictionary<Vector2, int> tileMap;
        public List<Rectangle> tileMapTextureStore;
        public Texture2D tileMapTextureAltes;
        public Dictionary<Vector2, int> mg;
        public Dictionary<Vector2, int> fg;        
        public Dictionary<Vector2, int> dead;
        public Dictionary<Vector2, int> win;
        public Dictionary<Vector2, int> sign;


        public int tileSize = 64;
        public int tilesPerRow = 16;
        public int pixelTileSize = 16;

        public List<Rectangle> Intersections; 

        public int mapWidthInTiles { get; private set; }
        public int mapHeightInTiles { get; private set; }


        /// <summary>
        /// Laddar upp jälva kartan med hjälp av filePath
        /// </summary>
        public Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new();
            StreamReader reader = new(filepath);
            int y = 0;
            int maxWidth = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(',');
                maxWidth = Math.Max(maxWidth, items.Length);
                for (int x = 0; x < items.Length; x++)
                {
                    if (int.TryParse(items[x], out int value))
                    {
                        if (value != -1)
                        {
                            result[new Vector2(x, y)] = value;
                        }
                    }
                }
                y++;
            }
            mapWidthInTiles = maxWidth;
            mapHeightInTiles = y;
            return result;
        }

        /// <summary>
        /// Listar ut kollisionen horizonralt
        /// </summary>
        public List<Rectangle> getIntesectingTilesHorizontal (Rectangle target)
        {
            List<Rectangle> intersections = new();

            int widthInTiles = (target.Width - (target.Width % tileSize)) / tileSize;
            int heightInTiles = (target.Height - (target.Height % tileSize)) / tileSize;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    intersections.Add(new Rectangle(
                        (target.X + x * tileSize) / tileSize,
                        (target.Y + y * tileSize) / tileSize,
                        tileSize,
                        tileSize
                    ));
                }
            }
            return intersections;
        }
        /// <summary>
        /// Listar ut kollisionen verticalt
        /// </summary>
        public List<Rectangle> getIntesectingTilesVertical (Rectangle target)
        {
            List<Rectangle> intersections = new();

            int widthInTiles = (target.Width - (target.Width % tileSize)) / tileSize;
            int heightInTiles = (target.Height - (target.Height % tileSize)) / tileSize;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    intersections.Add(new Rectangle(
                        (target.X + x * tileSize) / tileSize,
                        (target.Y + y * tileSize) / tileSize,
                        tileSize,
                        tileSize
                    ));
                }
            }
            return intersections;
        }
        /// <summary>
        /// Kollisionen fungerrar
        /// </summary>
        private bool CheckCollision(Rectangle playerRect, Rectangle tileRect)
        {
            return playerRect.Intersects(tileRect);
        }



        public void Update(Player player, MapManager mapManager)
        {
            MovementComponent movement = player.GetComponent<MovementComponent>();
            bool wasGrounded = false;

            Vector2 newPosX = new Vector2(player.Position.X + player.velocity.X, player.Position.Y);
            Rectangle futureRectX = player.spriteComponent.collisionRectangle;
            futureRectX.X = (int)newPosX.X + (player.collisionRectangle.X - (int)player.Position.X);

            foreach (var tile in mg)
            {
                Rectangle tileRect = new Rectangle(
                    (int)tile.Key.X * tileSize,
                    (int)tile.Key.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (CheckCollision(futureRectX, tileRect))
                {
                    if (player.velocity.X > 0)
                    {
                        newPosX.X = tileRect.Left - (futureRectX.Width + (futureRectX.X - (int)newPosX.X));
                    }
                    else if (player.velocity.X < 0)
                    {
                        newPosX.X = tileRect.Right - (futureRectX.X - (int)newPosX.X);
                    }
                    player.velocity.X = 0;
                    break;
                }
            }
            player.Position = new Vector2(newPosX.X, player.Position.Y);

            Vector2 newPosY = new Vector2(player.Position.X, player.Position.Y + player.velocity.Y);
            Rectangle futureRectY = player.spriteComponent.collisionRectangle;
            futureRectY.Y = (int)newPosY.Y + (player.collisionRectangle.Y - (int)player.Position.Y);

            foreach (var tile in mg)
            {
                Rectangle tileRect = new Rectangle(
                    (int)tile.Key.X * tileSize,
                    (int)tile.Key.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (CheckCollision(futureRectY, tileRect))
                {
                    if (player.velocity.Y > 0) // Kollision nedåt
                    {
                        newPosY.Y = tileRect.Top - (futureRectY.Height + (futureRectY.Y - (int)newPosY.Y));
                        wasGrounded = true;
                    }
                    else if (player.velocity.Y < 0) // Kollision uppåt
                    {
                        newPosY.Y = tileRect.Bottom - (futureRectY.Y - (int)newPosY.Y);
                    }
                    player.velocity.Y = 0;
                    break;
                }
            }
            player.Position = new Vector2(player.Position.X, newPosY.Y);

            // Sätt grounded
            if (movement != null)
            {
                movement.SetGrounded(wasGrounded);
            }


            // Kolla om spelaren dör
            foreach (var tile in dead)
            {
                Rectangle tileRect = new Rectangle(
                    (int)tile.Key.X * tileSize,
                    (int)tile.Key.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (CheckCollision(player.spriteComponent.collisionRectangle, tileRect))
                {
                    player.hasDied = true;
                    player.Position = player.spawnPosition; // eller (0,0) om du vill
                    player.velocity = Vector2.Zero;
                    break; // Avsluta loopen om spelaren dör
                }
            }

            //Kolla om spelaren vinner 
            foreach (var tile in win)
            {
                Rectangle tileRect = new Rectangle(
                    (int)tile.Key.X * tileSize,
                    (int)tile.Key.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (CheckCollision(player.spriteComponent.collisionRectangle, tileRect))
                {
                    mapManager.NextLevel();
                    player.Position = player.spawnPosition;
                    player.velocity = Vector2.Zero;
                    break;
                }
            }
        }


        /// <summary>
        /// Räknar ut hur många pixlar och annat det är i tiles
        /// </summary>
        public void DrawLayer(SpriteBatch spriteBatch, Dictionary<Vector2, int> layer)
        {
            if (layer == null) return;
            foreach (var item in layer)
            {
                Rectangle dest = new(
                    (int)item.Key.X * tileSize,
                    (int)item.Key.Y * tileSize,
                    tileSize,
                    tileSize
                );

                int x = item.Value % tilesPerRow;
                int y = item.Value / tilesPerRow;

                Rectangle source = new(
                    x * pixelTileSize,
                    y * pixelTileSize,
                    pixelTileSize,
                    pixelTileSize
                );
                spriteBatch.Draw(tileMapTextureAltes, dest, source, Color.White);
            }

            

        }

    }
}