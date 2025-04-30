using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_S1
{
    public class ColoredSprite : ScaledSprite
    {
        public Color color;

        public ColoredSprite(Texture2D texture, Vector2 position, Color color) : base(texture, position)
        {
            this.color = color;
        }
        
    }
}