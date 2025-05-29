using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Diagnostics;


namespace MonoGame_S1;

public class Game1 : Game
{

    //Anmäla saker
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private FollowCamera camera;

    //Karaktärer
    Player player;
    Enemy enemy;
    TileMaps tilemap;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        // _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        // _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        // _graphics.IsFullScreen = false;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        camera = new(Vector2.Zero);

        tilemap = new TileMaps();

        tilemap.Intersections = new();

        tilemap.mg = tilemap.LoadMap("../../../TileMapsFiles/CSV_files/level1_mg.csv");
        tilemap.fg = tilemap.LoadMap("../../../TileMapsFiles/CSV_files/level1_fg.csv");
        tilemap.bg = tilemap.LoadMap("../../../TileMapsFiles/CSV_files/level2_bg.csv");


        tilemap.tileMap = tilemap.mg;
        tilemap.tileMap = tilemap.fg;
        tilemap.tileMap = tilemap.bg;


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

        Vector2 playerStartPosition = new Vector2(100, 300);
        player = new Player(playerSpriteTexture, playerStartPosition, Color.White);
        Vector2 enemyStartPosition = new Vector2(200, 300);
        enemy = new Enemy(enemySpriteTexture, enemyStartPosition, Color.White);


        //Tile Maps och annat
        Texture2D tileMapTextureAltes = Content.Load<Texture2D>("world_tileset");

        tilemap.tileMapTextureAltes = tileMapTextureAltes;
        tilemap.tileMapTextureStore = new List<Rectangle>
        {
            new Rectangle(0, 0, 16, 16),    // Första tile (överst till vänster)
            new Rectangle(16, 0, 16, 16),   // Andra tile
            new Rectangle(32, 0, 16, 16),   // Tredje tile
            new Rectangle(48, 0, 16, 16)    // Fjärde tile
        };

    }

    protected override void Update(GameTime gameTime)
    {


        player.Update(gameTime);
        enemy.Update(gameTime);
        camera.Follow(player.collisionRectangle, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
        tilemap.Update(player);
        
        
        //Tile Collision
        


        
        base.Update(gameTime);

        // TODO: Add your update logic here
    }

    protected override void Draw(GameTime gameTime)
    {


        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: camera.Transform, samplerState: SamplerState.PointClamp); //Start Sprite

        //Rita Tiles
        tilemap.tileMap = tilemap.bg;
        tilemap.Draw(_spriteBatch);

        tilemap.tileMap = tilemap.mg;
        tilemap.Draw(_spriteBatch);

        tilemap.tileMap = tilemap.fg;
        tilemap.Draw(_spriteBatch);

        //Rita spelare och annat
        player.Draw(_spriteBatch);
        enemy.Draw(_spriteBatch);

        
        
        

   


        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
