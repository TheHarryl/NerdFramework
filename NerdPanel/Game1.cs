using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NerdPanel
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private System.Windows.Forms.Form _winform;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _winform = System.Windows.Forms.Form.FromHandle(Window.Handle) as System.Windows.Forms.Form;
            _winform.AllowDrop = true;
            _winform.DragEnter += new System.Windows.Forms.DragEventHandler(DragEnter);
            _winform.DragDrop += new System.Windows.Forms.DragEventHandler(DragDrop);

            base.Initialize();
        }

        private void DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("dd");
        }

        private void DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("ss");
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
