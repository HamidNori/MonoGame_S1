using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace MonoGame_S1;

public class Game1 : Game
{

    //Anmäla saker
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    //Karaktärer
    Player player;
    Enemy enemy;
    TileMaps tilemap;





    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        tilemap = new TileMaps();

        tilemap.mg = tilemap.LoadMap("../../../TileMapsFiles/CSV_files/level1_mg.csv");
        tilemap.fg = tilemap.LoadMap("../../../TileMapsFiles/CSV_files/level1_fg.csv");
        tilemap.collision = tilemap.LoadMap("../../../TileMapsFiles/CSV_files/level1_collisions.csv");

        tilemap.tileMap = tilemap.mg;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
    // TODO: use this.Content to load your game content here

        _spriteBatch = new SpriteBatch(GraphicsDevice);


        //Spelare och annat
        Texture2D playerSpriteTexture = Content.Load<Texture2D>("knight");
        Texture2D enemySpriteTexture = Content.Load<Texture2D>("slime_purple");

        player = new Player(playerSpriteTexture, Vector2.Zero, Color.White);
        enemy = new Enemy(enemySpriteTexture, Vector2.Zero, Color.White);


        //Tile Maps och annat
        Texture2D tileMapTextureAltes = Content.Load<Texture2D>("world_tileset");

        tilemap.tileMapTextureAltes = tileMapTextureAltes;
        tilemap.tileMapTextureStore = new List<Rectangle>
        {
            new Rectangle(0, 0, 16, 16),
            new Rectangle(16, 0, 16, 16),
            new Rectangle(32, 0, 16, 16),
            new Rectangle(48, 0, 16, 16)
        };

    }

    protected override void Update(GameTime gameTime)
    {


        player.Update(gameTime);
        enemy.Update(gameTime);
        base.Update(gameTime);

        // TODO: Add your update logic here
    }

    protected override void Draw(GameTime gameTime)
    {


        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //Start Sprite
        
        //Rita spelare och annat
        player.Draw(_spriteBatch);
        enemy.Draw(_spriteBatch);


        //Rita Tiles
        tilemap.Draw(_spriteBatch);
        tilemap.tileMap = tilemap.mg;
        tilemap.Draw(_spriteBatch);
        tilemap.tileMap = tilemap.fg;
        tilemap.Draw(_spriteBatch);


        _spriteBatch.End();


        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
