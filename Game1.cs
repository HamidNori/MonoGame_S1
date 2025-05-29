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
    TileMaps tilemap;
    private Texture2D debugPixel;

    MapManager mapManager;


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

        tilemap.tileMap = tilemap.mg;
        tilemap.tileMap = tilemap.fg;
        tilemap.tileMap = tilemap.win;
        tilemap.tileMap = tilemap.dead;
        tilemap.tileMap = tilemap.sign;


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

        Vector2 playerStartPosition = new Vector2(100, 300);
        player = new Player(playerSpriteTexture, playerStartPosition, Color.White);
        Vector2 enemyStartPosition = new Vector2(200, 300);


        //Tile Maps och annat
        Texture2D tileMapTextureAltes = Content.Load<Texture2D>("world_tileset");

        foreach (var level in mapManager.Levels)
        {
            tilemap.tileMapTextureAltes = tileMapTextureAltes;
            tilemap.tileMapTextureStore = new List<Rectangle>
            {
                new Rectangle(0, 0, 16, 16),    // Första tile (överst till vänster)
                new Rectangle(16, 0, 16, 16),   // Andra tile
                new Rectangle(32, 0, 16, 16),   // Tredje tile
                new Rectangle(48, 0, 16, 16)    // Fjärde tile
            };


        }
        
        debugPixel = new Texture2D(GraphicsDevice, 1, 1);
        debugPixel.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {


        player.Update(gameTime);
        camera.Follow(player.collisionRectangle, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
        tilemap.Update(player);
        mapManager.CurrentMap.Update(player);

        if (player.Position.X > _graphics.PreferredBackBufferWidth)
        {
            mapManager.NextLevel();
            player.Position = new Vector2(0, player.Position.Y);
        }
        
        
        //Tile Collision




        base.Update(gameTime);

        // TODO: Add your update logic here
    }

    protected override void Draw(GameTime gameTime)
    {


        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(transformMatrix: camera.Transform, samplerState: SamplerState.PointClamp);

        var currentMap = mapManager.CurrentMap;

        currentMap.tileMap = currentMap.win;
        currentMap.Draw(_spriteBatch);
        currentMap.tileMap = currentMap.dead;
        currentMap.Draw(_spriteBatch);
        currentMap.tileMap = currentMap.sign;
        currentMap.Draw(_spriteBatch);
        currentMap.tileMap = currentMap.mg;
        currentMap.Draw(_spriteBatch);
        currentMap.tileMap = currentMap.fg;
        currentMap.Draw(_spriteBatch);

        player.Draw(_spriteBatch);
        player.spriteComponent.DebugDraw(_spriteBatch, debugPixel);

        _spriteBatch.End();
        base.Draw(gameTime);
        }
}
