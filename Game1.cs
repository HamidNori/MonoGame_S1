using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoGame_S1;

public class Game1 : Game
{

    //Anmäla saker
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    ColoredSprite enemy;
    ColoredSprite player;

    //Animation
    private  int frameColumn = 0;
    private int frameRow = 0;
    private double timer = 0;

    //Movement


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

        //Ladda Sprites

        Texture2D Player = Content.Load<Texture2D>("knight");
        Texture2D Enemy = Content.Load<Texture2D>("slime_purple");
        _spriteBatch = new SpriteBatch(GraphicsDevice);


        player = new ColoredSprite(Player, Vector2.Zero, Color.White);
        enemy = new ColoredSprite(Enemy, Vector2.Zero, Color.White);
    }

    protected override void Update(GameTime gameTime)
    {
        //Animation
        if (timer > .25)
        {
            frameColumn++;
            frameColumn %= 4;
            timer = 0;
        }

        //Movement
        KeyboardState state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.Right))
        {
            player.position.X += 5;
            frameRow = 1;
        }
        else if (state.IsKeyDown(Keys.Left))
        {
            player.position.X -= 5;
            frameRow = 2;
        }
        else if (state.IsKeyDown(Keys.Up))
        {
            player.position.Y -= 5;
            frameRow = 3;
        }
        else if (state.IsKeyDown(Keys.Down))
        {
            player.position.Y += 5;
            frameRow = 0;
        }
        else
        {
            frameRow = 0;
        }



        //Spites
        player.Update();
        enemy.Update();


        timer += gameTime.ElapsedGameTime.TotalSeconds;
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {


        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //Start Sprite
        
        //player Animation
        int PlayerWidth = player.texture.Width / 8; //Sprite Width Frame
        int PlayerHeight = player.texture.Height / 8; //Sprite Height Frame
        Rectangle PlayerRectangle = new Rectangle(frameColumn * PlayerWidth, 0, PlayerWidth,PlayerHeight); //vilken sprite
        _spriteBatch.Draw(player.texture, player.Rect, PlayerRectangle , player.color);


        int EnemyWidth = enemy.texture.Width / 4; //Sprite Width Frame
        int EnemyHeight = enemy.texture.Height / 4; //Sprite Height Frame
        Rectangle EnemyRectangle = new Rectangle(frameRow * EnemyWidth, 0, EnemyWidth,EnemyHeight); //vilken sprite
        _spriteBatch.Draw(enemy.texture, enemy.Rect, EnemyRectangle , enemy.color);
        _spriteBatch.End();


        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
