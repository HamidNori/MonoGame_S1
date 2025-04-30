using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_S1
{
    public class MovingSprite : ScaledSprite
    {
        private float speed;
        public MovingSprite(Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.speed = speed;
        }

        public override void Update()
        {
            base.Update();
            position.X += speed;
        }
    }
}