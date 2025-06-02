using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;



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
    MapManager mapManager;

    //Bakgrundslager
    Texture2D layer1;
    Texture2D layer2;
    Texture2D layer3;
    Texture2D layer4;
    private float backgroundTimer = 0f;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.IsFullScreen = false;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        camera = new(Vector2.Zero);

        tilemap = new TileMaps();

        tilemap.Intersections = new();

    }

    protected override void Initialize()
    {

        mapManager = new MapManager();
        mapManager.LoadLevels();
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


        //Tile Maps och annat
        Texture2D tileMapTextureAltes = Content.Load<Texture2D>("world_tileset");
        //background
        layer1 = Content.Load<Texture2D>("Layer1");
        layer2 = Content.Load<Texture2D>("Layer2");
        layer3 = Content.Load<Texture2D>("Layer3");
        layer4 = Content.Load<Texture2D>("Layer4");



        foreach (var level in mapManager.Levels)
        {
            level.tileMapTextureAltes = tileMapTextureAltes;
            level.tileMapTextureStore = new List<Rectangle>
            {
                new Rectangle(0, 0, 16, 16),    // Första tile (överst till vänster)
                new Rectangle(16, 0, 16, 16),   // Andra tile
                new Rectangle(32, 0, 16, 16),   // Tredje tile
                new Rectangle(48, 0, 16, 16)    // Fjärde tile
            };
        }

    }

    protected override void Update(GameTime gameTime)
    {

        backgroundTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        player.Update(gameTime);
        var currentMap = mapManager.CurrentMap;

        int mapWidth = currentMap.mapWidthInTiles * currentMap.tileSize;
        int mapHeight = currentMap.mapHeightInTiles * currentMap.tileSize;
        camera.Follow(player.collisionRectangle, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), mapWidth, mapHeight);
        mapManager.CurrentMap.Update(player, mapManager);

        if (player.Position.X > _graphics.PreferredBackBufferWidth)
        {
            mapManager.NextLevel();
            player.Position = new Vector2(0, player.Position.Y);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        int screenWidth = _graphics.PreferredBackBufferWidth;
        int screenHeight = _graphics.PreferredBackBufferHeight;

        // Beräkna offset för lagren
        float offsetY3 = (float)Math.Sin(backgroundTimer * 0.5f) * 20f; // långsam, 20 pixlar upp/ner
        float offsetY4 = (float)Math.Sin(backgroundTimer * 0.7f + 1f) * 30f; // annan takt och fas

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Rita varje lager så det täcker hela skärmen
        _spriteBatch.Draw(layer1, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
        _spriteBatch.Draw(layer2, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
        _spriteBatch.Draw(layer3, new Rectangle(0, (int)offsetY3, screenWidth, screenHeight), Color.White);
        _spriteBatch.Draw(layer4, new Rectangle(0, (int)offsetY4, screenWidth, screenHeight), Color.White);

        _spriteBatch.End();

        // Resten av din draw-kod (tilemaps, spelare, etc)
        _spriteBatch.Begin(transformMatrix: camera.Transform, samplerState: SamplerState.PointClamp);

        var currentMap = mapManager.CurrentMap;
        currentMap.DrawLayer(_spriteBatch, currentMap.win);
        currentMap.DrawLayer(_spriteBatch, currentMap.dead);
        currentMap.DrawLayer(_spriteBatch, currentMap.sign);
        currentMap.DrawLayer(_spriteBatch, currentMap.mg);
        currentMap.DrawLayer(_spriteBatch, currentMap.fg);

        player.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
        }
}
