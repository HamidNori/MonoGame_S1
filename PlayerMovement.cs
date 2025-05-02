// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;

// namespace MonoGame_S1
// {
//     public class PlayerMovement : PlayerSprite
//     {
//         public PlayerMovement(Texture2D Texture, Vector2 Position, Color Color) : base(Texture, Position, Color) {   }
         
//         public override void Update(GameTime gameTime) 
//         {
            
//             KeyboardState kState = Keyboard.GetState();

//             if(kState.IsKeyDown(Keys.D))
//             {
//                 Position.X += 1;
//             }
//             if(kState.IsKeyDown(Keys.A))
//             {
//                 Position.X -= 1;
//             }
//             if(kState.IsKeyDown(Keys.W))
//             {
//                 Position.Y += 1;
//             }
//             if(kState.IsKeyDown(Keys.S))
//             {
//                 Position.Y -= 1;
//             }
//         }
        
//     }
// }