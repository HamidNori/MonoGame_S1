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
        public Dictionary<Vector2, int> bg;

        public int tileSize = 64;
        public int tilesPerRow = 16;
        public int pixelTileSize = 16;

        public List<Rectangle> Intersections; 


        /// <summary>
        /// Laddar upp jälva kartan med hjälp av filePath
        /// </summary>
        public Dictionary<Vector2, int> LoadMap (string filepath) 
        {
            Dictionary<Vector2, int> result  = new();

            StreamReader reader = new (filepath);
            int y = 0;
            string line;
            while ((line = reader.ReadLine())!=null)
            {
                string[] items = line.Split(',');

                for (int x = 0; x < items.Length; x++)
                {
                    if (int.TryParse(items[x], out int value))
                    {
                        if (value != -1){
                            result[new Vector2(x,y)] = value;
                        }
                        
                    }
                }
                y++;
            } 
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

        public void Update(Player player)
        {
            MovementComponent movement = player.GetComponent<MovementComponent>();
            bool wasGrounded = false;
            
            // Applicera horisontell rörelse först
            player.Position = new Vector2(player.Position.X + player.velocity.X, player.Position.Y);
            
            // Kontrollera horisontell kollision
            foreach (var tile in mg)
            {
                Rectangle tileRect = new Rectangle(
                    (int)tile.Key.X * tileSize,
                    (int)tile.Key.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (CheckCollision(player.destinationRectangle, tileRect))
                {
                    if (player.velocity.X > 0) // Höger kollision
                    {
                        player.Position = new Vector2(tileRect.Left - player.destinationRectangle.Width, player.Position.Y);
                    }
                    else if (player.velocity.X < 0) // Vänster kollision
                    {
                        player.Position = new Vector2(tileRect.Right, player.Position.Y);
                    }
                    player.velocity.X = 0;
                    break;
                }
            }

            // Applicera vertikal rörelse
            player.Position = new Vector2(player.Position.X, player.Position.Y + player.velocity.Y);
            
            // Kontrollera vertikal kollision
            foreach (var tile in mg)
            {
                Rectangle tileRect = new Rectangle(
                    (int)tile.Key.X * tileSize,
                    (int)tile.Key.Y * tileSize,
                    tileSize,
                    tileSize
                );

                if (CheckCollision(player.destinationRectangle, tileRect))
                {
                    if (player.velocity.Y > 0) // Kollision nedåt
                    {
                        player.Position = new Vector2(player.Position.X, tileRect.Top - player.destinationRectangle.Height);
                        wasGrounded = true;
                        
                    }
                    else if (player.velocity.Y < 0) // Kollision uppåt
                    {
                        player.Position = new Vector2(player.Position.X, tileRect.Bottom);
                    }
                    player.velocity.Y = 0;
                    break;
                }
            }

            if (movement != null)
            {
               movement.SetGrounded(wasGrounded);
            }
        }


        /// <summary>
        /// Räknar ut hur många pixlar och annat det är i tiles
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (var item in tileMap)
            {
              Rectangle dest = new (
                (int)item.Key.X * tileSize,
                (int)item.Key.Y * tileSize,
                tileSize,
                tileSize
              );  

              int x = item.Value % tilesPerRow;
              int y = item.Value / tilesPerRow;


              Rectangle source = new (
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