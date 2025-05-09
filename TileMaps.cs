using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MonoGame_S1
{
    public class TileMaps
    {
        public Dictionary<Vector2, int> tileMap;
        public List<Rectangle> tileMapTextureStore;
        public Texture2D tileMapTextureAltes;
        public Dictionary<Vector2, int> mg;
        public Dictionary<Vector2, int> fg;        
        public Dictionary<Vector2, int> collision;



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

        public void Draw(SpriteBatch spriteBatch)
        {

            int display_tilesize = 64;
            int num_tiles_per_row = 16;
            int pixel_tilesize = 16;

            foreach (var item in tileMap)
            {
              Rectangle dest = new (
                (int)item.Key.X * display_tilesize,
                (int)item.Key.Y * display_tilesize,
                display_tilesize,
                display_tilesize
              );  

              int x = item.Value % num_tiles_per_row;
              int y = item.Value / num_tiles_per_row;


              Rectangle source = new (
                x * pixel_tilesize,
                y * pixel_tilesize,
                pixel_tilesize,
                pixel_tilesize
              );

              spriteBatch.Draw(tileMapTextureAltes, dest, source, Color.White);
            }

        }



    }
}