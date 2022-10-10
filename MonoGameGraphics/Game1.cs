using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NerdFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Math = NerdFramework.Math;
using Vector2 = NerdFramework.Vector2;
using Vector3 = NerdFramework.Vector3;

namespace MonoGameGraphics
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Renderer3 renderer = new Renderer3(new Ray3Region(new Ray3(Vector3.Zero, Vector3.zAxis), 100, 75), 800, 600);
        //private MeshTriangle3Collection tris = MeshParser.FromCube(Vector3.Zero, 20).polygons;
        //private MeshTriangle3Collection tris = MeshParser.FromIcoSphere(new Vector3(-15, 0, 15), 15, 4).polygons;
        //private MeshTriangle3Collection tris = MeshParser.FromUVSphere(new Vector3(0, 0, 15), 15);
        //private MeshTriangle3Collection tris = MeshParser.FromQuadSphere(new Vector3(-15, 0, 15), 15, 1).polygons;
        private MeshTriangle3Collection tris = MeshParser.FromFile("C:\\Users\\harry\\Desktop\\Mathi\\HarrylMath\\Test\\luke.obj").polygons;
        private Texture2D screen;

        private int frameCount = 0;
        private DateTime startFrameCount = DateTime.Now;
        private bool downNeg = false;
        private bool downPos = false;

        private int iterations = 1;

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
            renderer.scene = tris;
            screen = new Texture2D(_graphics.GraphicsDevice, renderer.width, renderer.height);

            base.Initialize();

            Trace.WriteLine(tris.triangles.Count);
            tris.origin = new Vector3(-15.0, -5.0, 20.0);
            tris.scale = Vector3.One * 3;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                downPos = true;
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                downNeg = true;
            if (Keyboard.GetState().IsKeyUp(Keys.OemPlus) && downPos)
            {
                downPos = false;
                iterations++;
                tris = MeshParser.FromQuadSphere(new Vector3(-15, 0, 15), 15, iterations, NormalType.Interpolated).polygons;
                renderer.scene = tris;
                System.Diagnostics.Trace.WriteLine(tris.triangles.Count);
            }
            if (Keyboard.GetState().IsKeyUp(Keys.OemMinus) && downNeg)
            {
                downNeg = false;
                iterations--;
                tris = MeshParser.FromQuadSphere(new Vector3(-15, 0, 15), 15, iterations, NormalType.Interpolated).polygons;
                renderer.scene = tris;
                System.Diagnostics.Trace.WriteLine(tris.triangles.Count);
            } if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                tris.origin += new Vector3(0.0, -10.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                tris.origin += new Vector3(0.0, 10.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                tris.scale += new Vector3(-10.0 * gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                tris.scale += new Vector3(10.0 * gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                tris.Rotate(0.0 * gameTime.ElapsedGameTime.TotalSeconds, 1.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0 * gameTime.ElapsedGameTime.TotalSeconds, new Vector3(0, 0, 0));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                tris.Rotate(0.0 * gameTime.ElapsedGameTime.TotalSeconds, -1.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0 * gameTime.ElapsedGameTime.TotalSeconds, new Vector3(0, 0, 0));
            }

            tris.Rotate(-0.01 * gameTime.ElapsedGameTime.TotalSeconds, 0.005 * gameTime.ElapsedGameTime.TotalSeconds, 0.0 * gameTime.ElapsedGameTime.TotalSeconds, new Vector3(0, 0, 0));
            //renderer.cameraLight.rayCaster.d.p = (renderer.cameraLight.rayCaster.d.p - new Vector3(0, 0, 15)).Rotate(-0.2 * gameTime.ElapsedGameTime.TotalSeconds, 0.2 * gameTime.ElapsedGameTime.TotalSeconds, 0.0) + new Vector3(0, 0, 15);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            //renderer.RenderRaytraced();
            renderer.RenderRasterized();
            //renderer.RenderSampled();
            //renderer.FillCircle(new Color3Sequence(Color3.White, Color3.Black), new Vector2(100, 50), 48);
            Color[] data = new Color[renderer.height * renderer.width];
            for (int y = 0; y < renderer.height; y++)
            {
                for (int x = 0; x < renderer.width; x++)
                {
                    data[x + (renderer.height - y - 1) * renderer.width] = new Color(renderer.lightBuffer[y, x].r, renderer.lightBuffer[y, x].g, renderer.lightBuffer[y, x].b);
                }
            }
            screen.SetData(data);
            _spriteBatch.Draw(screen, new Rectangle(0, 0, renderer.width, renderer.height), Color.White);
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
