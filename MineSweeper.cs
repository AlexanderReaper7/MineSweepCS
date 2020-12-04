using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineSweepCS
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MineSweeper : Game
    {
        public const double UNITS = 20.0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MineField mineField;
        Texture2D white;
        KeyboardState prevKeyboardState;
        MouseState prevMouseState;

        public MineSweeper(int cols, int rows, double concentration)
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true,
                PreferredBackBufferWidth = (int)(cols * UNITS),
                PreferredBackBufferHeight = (int)(rows * UNITS)
            };
            Content.RootDirectory = "Content";
            mineField = new MineField(cols, rows, concentration);
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            white = new Texture2D(GraphicsDevice,1,1);
            white.SetData(new []{Color.White});
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                var cell = GetCellFromMousePosition();
                if (cell.HasValue)
                {
                    if (mineField.States[cell.Value.Item1][cell.Value.Item2] == CellState.Hidden)
                    {
                        mineField.RevealCell(cell.Value.Item1, cell.Value.Item2);
                    }
                }
            }
            else if (Mouse.GetState().RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {
                var cell = GetCellFromMousePosition();
                if (cell.HasValue)
                {
                    if (mineField.States[cell.Value.Item1][cell.Value.Item2] == CellState.Hidden)
                    {
                        mineField.FlagCell(cell.Value.Item1, cell.Value.Item2);
                    }
                    else if (mineField.States[cell.Value.Item1][cell.Value.Item2] == CellState.Flagged)
                    {
                        mineField.UnFlagCell(cell.Value.Item1, cell.Value.Item2);
                    }
                }  
            }

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        (int, int)? GetCellFromMousePosition()
        {
            (int mx, int my) = Mouse.GetState().Position;
            int x = (int) (mx / UNITS);
            if (x < 0 || x >= mineField.Cols) return null;
            int y = (int)(my / UNITS);
            if (y < 0 || y >= mineField.Rows) return null;

            return (x, y);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            for (int x = 0; x < mineField.States.Count; x++)
            {
                for (int y = 0; y < mineField.States[x].Count; y++)
                {
                    switch (mineField.States[x][y])
                    {
                        case CellState.Hidden: 
                            // Draw a white rectangle 
                            spriteBatch.Draw(white, new Rectangle((int)(x * UNITS) +1, (int) (y * UNITS)+1, (int) UNITS-1, (int) UNITS-1), Color.White);
                            break;
                        case CellState.Revealed:
                            if (mineField.Mines[x][y] is byte b)
                            {
                                //spriteBatch.DrawString(); TODO:
                                var color = new Color((float)(b % 2), (float)(b % 3)/2, (float)(b % 4)/3, 1.0f);
                                spriteBatch.Draw(white, new Rectangle((int)(x * UNITS) + 1, (int)(y * UNITS) + 1, (int)UNITS - 1, (int)UNITS - 1), color);
                            }
                            else
                            {
                                var ntime = gameTime.TotalGameTime.Ticks / 5000000f;
                                var nnewcolor = new Color((float)(Math.Sin(ntime)), (float)(Math.Sin(ntime + (Math.PI / 2f))), (float)(Math.Sin(ntime + Math.PI)), 1.0f);
                                spriteBatch.Draw(white, new Rectangle((int)(x * UNITS) + 1, (int)(y * UNITS) + 1, (int)UNITS - 1, (int)UNITS - 1), nnewcolor);
                            }
                            break;
                        case CellState.Flagged:
                            var time = gameTime.TotalGameTime.Ticks / 5000000f;
                            var newcolor = new Color((float)(Math.Sin(time)), (float)(Math.Sin(time+(Math.PI / 2f))), (float)(Math.Sin(time+Math.PI)), 1.0f);
                            spriteBatch.Draw(white, new Rectangle((int)(x * UNITS) + 1, (int)(y * UNITS) + 1, (int)UNITS - 1, (int)UNITS - 1), newcolor);
                            break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
