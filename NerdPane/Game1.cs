using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NerdPane
{
    public class Game1 : Game
    {
        private System.Windows.Forms.Control _winform;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool _dragging = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _winform = System.Windows.Forms.Form.FromHandle(Window.Handle);
            _winform.AllowDrop = true;
            _winform.DragEnter += new System.Windows.Forms.DragEventHandler(DragEnter);
            _winform.DragDrop += new System.Windows.Forms.DragEventHandler(DragDrop);
            _winform.DragLeave += new System.EventHandler(DragLeave);

            base.Initialize();
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!_dragging)
                GraphicsDevice.Clear(Color.CornflowerBlue);
            else
                GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
