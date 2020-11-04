using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Monogame_Collision_with_Rectangles_Example
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;
        MouseState mouseState;

        // Textures
        Texture2D pacLeftTexture;
        Texture2D pacRightTexture;
        Texture2D pacUpTexture;
        Texture2D pacDownTexture;

        Texture2D currentPacTexture;    // This will be used to draw PacMan in whatever direction he is facing
        Rectangle pacRect;

        Texture2D exitTexture;
        Rectangle exitRect;

        Texture2D barrierTexture;
        Rectangle barrierRect1, barrierRect2;

        Texture2D coinTexture;
        List<Rectangle> coins;

        int pacSpeed;        

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
            
            pacSpeed = 3;
            pacRect = new Rectangle(10, 10, 60, 60);

            barrierRect1 = new Rectangle(0, 250, 350, 75);
            barrierRect2 = new Rectangle(450, 250, 350, 75);

            // It is better to make a texture the size you want it to be in the program so ot doesn't need to be scaled uppon drawing
            coins = new List<Rectangle>();
            coins.Add(new Rectangle(400, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(475, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(200, 350, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(400, 350, coinTexture.Width, coinTexture.Height));

            exitRect = new Rectangle(700, 380, 100, 100);

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Textures
            // Pacman
            pacDownTexture = Content.Load<Texture2D>("pac_down");
            pacUpTexture = Content.Load<Texture2D>("pac_up");
            pacRightTexture = Content.Load<Texture2D>("pac_right");
            pacLeftTexture = Content.Load<Texture2D>("pac_left");
            currentPacTexture = pacRightTexture;

            // Barrier
            barrierTexture = Content.Load<Texture2D>("rock_barrier");

            // Exit
            exitTexture = Content.Load<Texture2D>("hobbit_door");

            // Coin
            coinTexture = Content.Load<Texture2D>("coin");

        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacRect.X -= pacSpeed;
                currentPacTexture = pacLeftTexture;
                if (pacRect.Intersects(barrierRect1))
                {
                    pacRect.X = barrierRect1.Right;
                }
                if (pacRect.Intersects(barrierRect2))
                {
                    pacRect.X = barrierRect2.Right;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacRect.X += pacSpeed;               
                currentPacTexture = pacRightTexture;
                if (pacRect.Intersects(barrierRect1))
                {
                    pacRect.X = barrierRect1.Left - pacRect.Width;
                }
                if (pacRect.Intersects(barrierRect2))
                {
                    pacRect.X = barrierRect2.Left - pacRect.Width;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pacRect.Y -= pacSpeed;
                currentPacTexture = pacUpTexture;
                if (pacRect.Intersects(barrierRect1))
                {
                    pacRect.Y = barrierRect1.Bottom;
                }
                if (pacRect.Intersects(barrierRect2))
                {
                    pacRect.Y = barrierRect2.Bottom;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacRect.Y += pacSpeed;
                currentPacTexture = pacDownTexture;
                if (pacRect.Intersects(barrierRect1))
                {
                    pacRect.Y = barrierRect1.Top - pacRect.Height;
                }
                if (pacRect.Intersects(barrierRect2))
                {
                    pacRect.Y = barrierRect2.Top - pacRect.Height;
                }
            }

            // Iterates through our list of coins by index and removes collisions
            for (int i = 0; i < coins.Count; i++)
            {
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;    // We must reduce our index by one because we removed an item from our list
                }
            }
            
            // Check for an exit witht he door
            if(mouseState.LeftButton == ButtonState.Pressed)
            {
                if (exitRect.Contains(mouseState.X, mouseState.Y))
                    Exit();
            }
            if (exitRect.Contains(pacRect))
                Exit();

            base.Update(gameTime);        
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            _spriteBatch.Draw(barrierTexture, barrierRect1, Color.White);
            _spriteBatch.Draw(barrierTexture, barrierRect2, Color.White);
            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(currentPacTexture, pacRect, Color.White);
            foreach(Rectangle coin in coins)
                _spriteBatch.Draw(coinTexture, coin, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
