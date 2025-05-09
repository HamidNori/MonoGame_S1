using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_S1
{
    public class TitleMaps
    {
        public Dictionary<Vector2, int> tileMap;
        private List<Rectangle> tileMapTextureStore;
        private Texture2D tileMapTextureAltes;


        private Dictionary<int, Texture2D> LoadMap (string filepath) 
        {
            Dictitionary <Vector2, int> tileMap = new;

            StremReader reader = new (filepath);

            int y = 0;
            string line;
            while ((line = reader.ReadLine() !=null))
            {
                string[] items = line.Split(',');

                for (int x = 0; xx < items.Length; x++)
                {
                    if (int.TryParse(items[x], out int value))
                    {
                        if (value>0){
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
            foreach (var item in tileMap)
            {
              Rectangle dest = new (
                (int)item.Key.X * 64,
                (int)item.Key.XY* 64,
                64,
                64
              );  

              Rectangle source = tileMapTexture[item.Value-1];
              _sppriteBatch(tileMapTextureAltes, dest, source, Color.White);
            }

        }



    }
}