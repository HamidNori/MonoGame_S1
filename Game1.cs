using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_S1;

public class Game1 : Game
{

    //Anmäla saker
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    //Karaktärer
    Player player;



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
        playerSprites = new()
    }

    protected override void LoadContent()
    {
    // TODO: use this.Content to load your game content here

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D playerSpriteTexture = Content.Load<Texture2D>("knight");
        Texture2D enemySpriteTexture = Content.Load<Texture2D>("slime_purple");

        player = new Player(playerSpriteTexture, Vector2.Zero, Color.White);
        enemySprite = new EnemySprite(enemySpriteTexture, Vector2.Zero, Color.White);
        playerMovement = new PlayerMovement(playerSpriteTexture, Vector2.Zero, Color.White);
    }

    protected override void Update(GameTime gameTime)
    {
        player.Update(gameTime);
        base.Update(gameTime);



        // TODO: Add your update logic here
    }

    protected override void Draw(GameTime gameTime)
    {


        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //Start Sprite
        playerSprite.Draw(_spriteBatch);
        enemySprite.Draw(_spriteBatch);
        _spriteBatch.End();


        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
