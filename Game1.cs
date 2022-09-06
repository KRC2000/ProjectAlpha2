using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using ImGuiNET;
using MonoGame.Extended;


namespace ProjectAlpha2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private ImGuiIOPtr io;


        public static ImGuiRenderer imGuiRenderer;
        public static OrthographicCamera MainCamera;
        public static bool IsMouseCapturedByUi;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            SetFrameLimit(144);
            SetResolution(1200, 800);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            imGuiRenderer = new ImGuiRenderer(this);
            imGuiRenderer.RebuildFontAtlas();
            io = ImGui.GetIO();

            MainCamera = new OrthographicCamera(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceManager.AddTextureBinding(Content, TextureId.Unknown, "unknown");
            ResourceManager.AddTextureBinding(Content, TextureId.AvatarCat, "cat");
            ResourceManager.AddTextureBinding(Content, TextureId.AvatarDuck, "duck");
            ResourceManager.AddTextureBinding(Content, TextureId.AvatarElephant, "elephant");
            ResourceManager.AddTextureBinding(Content, TextureId.MarkerCircle, "circle");
            ResourceManager.AddTextureBinding(Content, TextureId.LocationImage_Vladimir, "Vladimir");
            ResourceManager.AddTextureBinding(Content, TextureId.MarkerFrame, "location_frame");
            ResourceManager.AddTextureBinding(Content, TextureId.Terrain, "terrain");

            World.Generate();
            World.LoadCotent();

            UI.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            World.Update(gameTime);

            if (!IsMouseCapturedByUi) CameraController.PanCamera(MainCamera);
            
            IsMouseCapturedByUi = io.WantCaptureMouse;

            InputManager.Refresh();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.Coral);

            
            World.Draw(spriteBatch);


            imGuiRenderer.BeforeLayout(gameTime);
            ImGui.ShowDemoWindow();
            UI.Draw();
            imGuiRenderer.AfterLayout();
            
            base.Draw(gameTime);

        }

        protected void SetResolution(int width, int height)
		{
			_graphics.PreferredBackBufferWidth = width;
			_graphics.PreferredBackBufferHeight = height;
			_graphics.ApplyChanges();
		}
        protected void SetFrameLimit(int targetFps)
		{
			if (targetFps != 0)
			{
				_graphics.SynchronizeWithVerticalRetrace = true;
				IsFixedTimeStep = true;
				TargetElapsedTime = TimeSpan.FromSeconds(1d / (double)targetFps);
			}
			else
			{
				_graphics.SynchronizeWithVerticalRetrace = false;
				IsFixedTimeStep = false;
			}
			_graphics.ApplyChanges();
		}

		protected void SetFullScreen(bool value)
		{
			_graphics.IsFullScreen = value;
			_graphics.ApplyChanges();
		}
    }
}
