using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NerdFramework;
using NerdPanel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NerdPanel
{
    public class Game1 : Game
    {
        private System.Windows.Forms.Control _winform;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool _dragging = false;
        private Texture2D screen;

        public InterfaceEngine renderer;
        public List<BaseTab> tabs;
        public int currentTab;

        private int frameCount = 0;
        private DateTime startFrameCount = DateTime.Now;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 144.0);
            //_graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _winform = System.Windows.Forms.Form.FromHandle(Window.Handle);
            _winform.AllowDrop = true;
            _winform.DragEnter += new System.Windows.Forms.DragEventHandler(DragEnter);
            _winform.DragDrop += new System.Windows.Forms.DragEventHandler(DragDrop);
            _winform.DragLeave += new System.EventHandler(DragLeave);

            renderer = new InterfaceEngine(800, 480);
            tabs = new List<BaseTab>();
            currentTab = 0;

            tabs.Add(new Grapher("y=x^2"));

            screen = new Texture2D(_graphics.GraphicsDevice, renderer.width, renderer.height);
            base.Initialize();
        }

        public Texture2 ConvertTexture(Texture2D texture)
        {
            Color[] rawData = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(rawData);

            Color3[,] rawDataAsGrid = new Color3[texture.Height, texture.Width];
            Parallel.For(0, texture.Height, row => {
                for (int column = 0; column < texture.Width; column++)
                {
                    Color color = rawData[row * texture.Width + column];
                    rawDataAsGrid[texture.Height - row - 1, column] = new Color3(color.R, color.G, color.B, color.A / 255.0);
                }
            });
            return new Texture2(rawDataAsGrid);
        }

        private void DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = System.Windows.Forms.DragDropEffects.Copy;
            _dragging = true;
        }

        private void DragLeave(object sender, System.EventArgs e)
        {
            _dragging = false;
        }

        private void DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            _dragging = false;
            System.Diagnostics.Trace.WriteLine(e.Data.GetData(e.Data.GetFormats()[0], false));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (currentTab >= 0 && currentTab < tabs.Count)
                tabs[currentTab].Update(renderer, gameTime.ElapsedGameTime.TotalSeconds);

            Color[] data = new Color[renderer.height * renderer.width];
            Parallel.For(0, renderer.height, y => {
                for (int x = 0; x < renderer.width; x++)
                {
                    data[(renderer.width - x - 1) + y * renderer.width] = new Color(renderer.lightBuffer[y, x].r, renderer.lightBuffer[y, x].g, renderer.lightBuffer[y, x].b);
                }
            });
            screen.SetData(data);
            //renderer.Clear();
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!_dragging)
                GraphicsDevice.Clear(Color.CornflowerBlue);
            else
                GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            if (currentTab >= 0 && currentTab < tabs.Count)
                tabs[currentTab].Draw(renderer);
            //renderer.RenderUI();

            _spriteBatch.Draw(screen, new Microsoft.Xna.Framework.Vector2(0f, 0f), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);

            frameCount++;
            if (DateTime.Now - startFrameCount >= new TimeSpan(0, 0, 1))
            {
                Trace.WriteLine(frameCount + " FPS (" + (renderer.frameNum / gameTime.TotalGameTime.TotalSeconds) + " Avg.)");
                frameCount = 0;
                startFrameCount = DateTime.Now;
            }
        }
    }
}
