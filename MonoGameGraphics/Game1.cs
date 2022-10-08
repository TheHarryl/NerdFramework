using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NerdFramework;
using System;
using System.Diagnostics;
using Vector2 = NerdFramework.Vector2;
using Vector3 = NerdFramework.Vector3;

namespace MonoGameGraphics
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Renderer3 renderer = new Renderer3(new Ray3Region(new Ray3(Vector3.Zero, Vector3.zAxis), 70, 35), 200, 100);
        //private Triangle3Group tris = Triangle3Group.FromCube(Vector3.Zero, 20);
        private Triangle3Group tris = Triangle3Group.FromIcophere(new Vector3(0, 0, 15), 15, 2);
        private Texture2D screen;

        private int frameCount = 0;
        private DateTime startFrameCount = DateTime.Now;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            renderer.scene = new Triangle3Group(tris.triangles);
            screen = new Texture2D(_graphics.GraphicsDevice, renderer.width, renderer.height);

            base.Initialize();

            Trace.WriteLine((new Color3Sequence(Color3.White)).ColorAt(0.5));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            tris.Rotate(-1 * gameTime.ElapsedGameTime.TotalSeconds, 0.5 * gameTime.ElapsedGameTime.TotalSeconds, 0.0 * gameTime.ElapsedGameTime.TotalSeconds, new Vector3(0, 0, 0));
            //renderer.cameraLight.rayCaster.d.p = (renderer.cameraLight.rayCaster.d.p - new Vector3(0, 0, 15)).Rotate(-2.0 * gameTime.ElapsedGameTime.TotalSeconds, 2.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0) + new Vector3(0, 0, 15);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            //renderer.RenderRaytraced();
            renderer.RenderRasterized();
            Color[] data = new Color[renderer.height * renderer.width];
            for (int y = 0; y < renderer.height; y++)
            {
                for (int x = 0; x < renderer.width; x++)
                {
                    data[(renderer.width - x - 1) + (renderer.height - y - 1) * renderer.width] = new Color(renderer.lightBuffer[y, x].r, renderer.lightBuffer[y, x].g, renderer.lightBuffer[y, x].b);
                }
            }
            screen.SetData(data);
            _spriteBatch.Draw(screen, new Rectangle(0, 0, renderer.width * 4, renderer.height * 4), Color.White);
            base.Draw(gameTime);

            _spriteBatch.End();

            frameCount++;
            if (DateTime.Now - startFrameCount >= new TimeSpan(0, 0, 1))
            {
                Trace.WriteLine(frameCount + " FPS");
                frameCount = 0;
                startFrameCount = DateTime.Now;
            }
        }
    }
}
