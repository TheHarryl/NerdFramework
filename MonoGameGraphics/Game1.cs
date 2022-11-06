using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NerdFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Math = NerdFramework.Math;
using Vector2 = NerdFramework.Vector2;
using Vector3 = NerdFramework.Vector3;

namespace MonoGameGraphics
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private static double FOV = Math.PI;
        private Renderer3 renderer = new Renderer3(new Ray3Sector(new Ray3(new Vector3(0.0, 0.0, -20.0), Vector3.zAxis), 100, 75, FOV), 800, 600);
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
        private double cameraSpeed = 100.0;

        private MouseState lastMouseState;
        private MouseState mouseState;

        private double totalAngularDisplacementX = 0.0;
        private double totalAngularDisplacementY = 0.0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 144.0);
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            MeshTriangle3Collection baseplate = MeshParser.FromCube(Vector3.Zero, 1.0).polygons;
            renderer.scene = tris;
            screen = new Texture2D(_graphics.GraphicsDevice, renderer.width, renderer.height);

            base.Initialize();

            Trace.WriteLine(tris.triangles.Count);
            tris.origin = new Vector3(0.0, 0.0, 30.0);
            tris.RotateY(Math.PI, Vector3.Zero);
            tris.scale = Vector3.One * 3;//17;
            tris.Rotate(-0.001, 0.005, 0.0, new Vector3(0, 0, 0));

            renderer.camera.Rotate(-0.001, 0.001, 0.001);

            renderer.cameraLight.rayCaster.d.p = (renderer.cameraLight.rayCaster.d.p - new Vector3(0, 0, 15)).Rotate(0.0, -Math.HalfPI, 0.0) + new Vector3(0, 0, 15);
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

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Dictionary<string, Texture2> textures = new Dictionary<string, Texture2>();

            List<string> tNames = new List<string>() { "belt.jpg", "chest.png", "leg.png", "luke face.png", "luke hair.png", "skybox\\up2.png", "skybox\\down2.png", "skybox\\front2.png", "skybox\\back2.png", "skybox\\left2.png", "skybox\\right2.png" };

            foreach (string name in tNames)
            {
                using (FileStream fs = new FileStream("C:\\Users\\harry\\Desktop\\Mathi\\HarrylMath\\Test\\texture\\" + name, FileMode.Open))
                {
                    textures.Add(name, ConvertTexture(Texture2D.FromStream(_graphics.GraphicsDevice, fs)));
                }
            }

            renderer.AddMaterials(MaterialParser.FromFile("C:\\Users\\harry\\Desktop\\Mathi\\HarrylMath\\Test\\luke.mtl", textures));

            renderer.skyboxFront = textures["skybox\\front2.png"];
            renderer.skyboxBack = textures["skybox\\back2.png"];
            renderer.skyboxLeft = textures["skybox\\left2.png"];
            renderer.skyboxRight = textures["skybox\\right2.png"];
            renderer.skyboxTop = textures["skybox\\up2.png"];
            renderer.skyboxBottom = textures["skybox\\down2.png"];
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.OemPlus))
                downPos = true;
            else if (downPos)
            {
                downPos = false;
                iterations++;
                tris = MeshParser.FromIcoSphere(new Vector3(0.0, 0.0, 100.0), 15, iterations, NormalType.Interpolated).polygons;
                tris.Rotate(-0.001, 0.005, 0.0, new Vector3(0, 0, 0));
                renderer.scene = tris;
                System.Diagnostics.Trace.WriteLine(tris.triangles.Count);
            }
            if (keyboardState.IsKeyDown(Keys.OemMinus))
                downNeg = true;
            else if (downNeg)
            {
                downNeg = false;
                iterations--;
                tris = MeshParser.FromQuadSphere(new Vector3(0.0, 0, 100.0), 15, iterations, NormalType.Interpolated).polygons;
                tris.Rotate(-0.001, 0.005, 0.0, new Vector3(0, 0, 0));
                renderer.scene = tris;
                System.Diagnostics.Trace.WriteLine(tris.triangles.Count);
            }
            
            if (keyboardState.IsKeyDown(Keys.Down))
                tris.origin += new Vector3(0.0, -10.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0);
            if (keyboardState.IsKeyDown(Keys.Up))
                tris.origin += new Vector3(0.0, 10.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0);
            if (keyboardState.IsKeyDown(Keys.Q))
                tris.scale += new Vector3(-1.0 * gameTime.ElapsedGameTime.TotalSeconds);
            if (keyboardState.IsKeyDown(Keys.R))
                tris.scale += new Vector3(1.0 * gameTime.ElapsedGameTime.TotalSeconds);
            if (keyboardState.IsKeyDown(Keys.Left))
                tris.Rotate(0.0 * gameTime.ElapsedGameTime.TotalSeconds, -1.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0 * gameTime.ElapsedGameTime.TotalSeconds, new Vector3(0, 0, 0));
            if (keyboardState.IsKeyDown(Keys.Right))
                tris.Rotate(0.0 * gameTime.ElapsedGameTime.TotalSeconds, 1.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0 * gameTime.ElapsedGameTime.TotalSeconds, new Vector3(0, 0, 0));
            if (keyboardState.IsKeyDown(Keys.D))
                renderer.camera.d.p += renderer.camera.d.v * cameraSpeed * gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.A))
                renderer.camera.d.p += renderer.camera.w.Normalized() * cameraSpeed * gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.S))
                renderer.camera.d.p -= renderer.camera.d.v * cameraSpeed * gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.H))
                renderer.camera.d.p -= renderer.camera.w.Normalized() * cameraSpeed * gameTime.ElapsedGameTime.TotalSeconds;

            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            //totalAngularDisplacementX += gameTime.ElapsedGameTime.TotalSeconds;
            //totalAngularDisplacementY += gameTime.ElapsedGameTime.TotalSeconds;

            if (lastMouseState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Pressed)
            {
                Vector2 angularDisplacement = new Vector2(
                    (mouseState.X - lastMouseState.X) / (double)renderer.width * FOV,
                    (mouseState.Y - lastMouseState.Y) / (double)renderer.height * (FOV * ((double)renderer.height /  renderer.width))
                );
                Mouse.SetPosition(lastMouseState.X, lastMouseState.Y);
                mouseState = lastMouseState;
                totalAngularDisplacementX = (totalAngularDisplacementX + angularDisplacement.x) % Math.TwoPI;
                totalAngularDisplacementY = (totalAngularDisplacementY + angularDisplacement.y) % Math.TwoPI;
                //Vector3 target = new Vector3(0.0, 0.0, 1.0).RotateX(totalAngularDisplacementY).RotateY(totalAngularDisplacementX);
                //Vector3 target = Vector3.Lerp(Vector3.zAxis, Vector3.yAxis, angularDisplacement.y / Math.PI).RotateY(-totalAngularDisplacementX);
                //renderer.camera.RotateTo(target);
                //System.Diagnostics.Trace.WriteLine(renderer.camera.d.v);
                renderer.camera.RotatePolar(-angularDisplacement.x);
                renderer.camera.RotateZenith(angularDisplacement.y);
                //Vector3 target = Vector3.Lerp(renderer.camera.d.v, renderer.camera.h, angularDisplacement.y / Math.PI);
                //renderer.camera.RotateTo(target);

                //renderer.camera.RotateAbout(renderer.camera.w, angularDisplacement.y);
            }
            int scrollWheelDelta = mouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue;
            if (scrollWheelDelta != 0)
                renderer.camera.d.p += renderer.camera.d.v * cameraSpeed * scrollWheelDelta * 0.01;

            // renderer.cameraLight.rayCaster.d.p = (renderer.cameraLight.rayCaster.d.p - new Vector3(0, 0, 15)).Rotate(0.0 * gameTime.ElapsedGameTime.TotalSeconds, 2.0 * gameTime.ElapsedGameTime.TotalSeconds, 0.0) + new Vector3(0, 0, 15);

            UpdateScreen();

            base.Update(gameTime);
        }

        private void UpdateScreen()
        {
            Color[] data = new Color[renderer.height * renderer.width];
            Parallel.For(0, renderer.height, y => {
                for (int x = 0; x < renderer.width; x++)
                {
                    data[x + (renderer.height - y - 1) * renderer.width] = new Color(renderer.lightBuffer[y, x].r, renderer.lightBuffer[y, x].g, renderer.lightBuffer[y, x].b);
                }
            });
            screen.SetData(data);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            //renderer.RenderRaytraced();
            //renderer.FillCircle(new Color3Sequence(Color3.White, Color3.Black), new Vector2(100, 50), 48);

            renderer.RenderRasterized();
            //renderer.RenderSampled();

            /*Color3 c = new Color3Sequence(Color3.Red, Color3.Green, Color3.Blue, Color3.Red).ColorAt((gameTime.TotalGameTime.TotalSeconds % 2) / 2);
            renderer.RenderShader((x, y) =>
            {
                int size = 5;
                int y0 = (y > renderer.height - 1 - size ? renderer.height - 1 - size : (y < size ? size : y));
                int x0 = (x > renderer.width - 1 - size ? renderer.width - 1 - size : (x < size ? size : x));
                double max = Math.Max(renderer.depthBuffer[y0 - size, x0], renderer.depthBuffer[y0 + size, x0], renderer.depthBuffer[y0, x0 - size], renderer.depthBuffer[y0, x0 + size]);
                return Math.Abs(max - renderer.depthBuffer[y, x]) > 20;
            }, (x, y, color) =>
            {
                return c;
            });
            /*renderer.RenderDynamicShader((x, y) =>
            {
                return renderer.lightBuffer[y, x] != Color3.Black;
            }, (x, y, color) =>
            {
                int displacement = (int)Tween.BounceIn(-50, 50, ((gameTime.TotalGameTime.TotalSeconds % 2) / 2 + y/250.0) % 1);
                if (displacement > 0)
                    displacement = -displacement;
                displacement += 50;
                int y0 = (y > renderer.height - 1 ? renderer.height - 1 : (y < 0 ? 0 : y));
                int x0 = (x + displacement > renderer.width - 1 ? renderer.width - 1 : (x + displacement < 0 ? 0 : x + displacement));
                return renderer.lightBuffer[y0, x0];
            });
            Color3 c = new Color3Sequence(Color3.Red, Color3.Green, Color3.Blue, Color3.Red).ColorAt((gameTime.TotalGameTime.TotalSeconds % 2) / 2);
            renderer.RenderDynamicShader((x, y) =>
            {
                int size = 1;
                int y0 = (y > renderer.height - 1 - size ? renderer.height - 1 - size : (y < size ? size : y));
                int x0 = (x > renderer.width - 1 - size ? renderer.width - 1 - size : (x < size ? size : x));
                double max = Math.Max((renderer.normalBuffer[y, x] - renderer.normalBuffer[y0 - size, x0]).Magnitude(), (renderer.normalBuffer[y, x] - renderer.normalBuffer[y0 + size, x0]).Magnitude(), (renderer.normalBuffer[y, x] - renderer.normalBuffer[y0, x0 - size]).Magnitude(), (renderer.normalBuffer[y, x] - renderer.normalBuffer[y0, x0 + size]).Magnitude());
                return max > 0.2;
            }, (x, y, color) =>
            {
                return c;
            });*/

            _spriteBatch.Draw(screen, new Microsoft.Xna.Framework.Vector2(0f, 0f), Color.White);
            base.Draw(gameTime);

            _spriteBatch.End();

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
