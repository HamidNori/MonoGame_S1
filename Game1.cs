using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGame_S1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    ScaledSprite sprite;

    private  int frameColumn = 0;
    private int frameRow = 0;
    private double timer = 0;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
    // TODO: use this.Content to load your game content here

        Texture2D texture = Content.Load<Texture2D>("knight");
        _spriteBatch = new SpriteBatch(GraphicsDevice);


        sprite = new ScaledSprite(texture, new Vector2(0, 0));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();


        if (timer > .25)
        {
            frameColumn++;
            frameColumn %= 4;
            timer = 0;
        }


        timer += gameTime.ElapsedGameTime.TotalSeconds;
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        int width = sprite.texture.Width / 8;
        int height = sprite.texture.Height / 8;
        Rectangle destenationRectangle = new Rectangle(0,0, 100, 100);
        Rectangle sourceRectanagle = new Rectangle(frameColumn * width, 0, width,height);
        
        _spriteBatch.Draw(sprite.texture, sprite.position, sourceRectanagle , Color.White);
        _spriteBatch.End();


        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
