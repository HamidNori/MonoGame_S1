using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


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
        public Dictionary<Vector2, int> collisions;

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
                        (target.Y + y * tileSize - 1) / tileSize,
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
                        (target.X + x * tileSize -1) / tileSize,
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
        public void Update (Player player)
        {
            Vector2 velocity = player.velocity;
            Intersections = getIntesectingTilesHorizontal(player.destinationRectangle);
                
                foreach (var rect in Intersections)
                {
                    if (mg.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                    {
                        Rectangle collsion = new Rectangle(
                            rect.X * tileSize,
                            rect.Y * tileSize,
                            tileSize,
                            tileSize 
                        );


                        if (velocity.X > 0.0f)
                        {
                            player.position.position.X = collsion.Left - player.destinationRectangle.Width;
                        }
                        else if (velocity.X <0.0f)
                        {
                            player.position.position.X = collsion.Right;
                        }
                    }
                }


            Intersections = getIntesectingTilesVertical(player.destinationRectangle);
            foreach (var rect in Intersections)
            {
                if (mg.TryGetValue(new Vector2 (rect.X, rect.Y), out int _val))
                {
                    Rectangle collsion = new Rectangle(
                            rect.X * tileSize,
                            rect.Y * tileSize,
                            tileSize,
                            tileSize 
                        );


                        if (velocity.Y > 0.0f)
                        {
                            player.position.position.Y = collsion.Top - player.destinationRectangle.Height;
                        }
                        else if (velocity.Y <0.0f)
                        {
                            player.position.position.Y = collsion.Bottom;
                        }
                }
            }
        
        player.Position += velocity;
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