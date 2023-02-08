using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using ImGuiNET;

using Framework;

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

            ResourceManager.AddTextureBinding(Content, "unknown");
            ResourceManager.AddTextureBinding(Content, "cat");
            ResourceManager.AddTextureBinding(Content, "Vladimir");
            ResourceManager.AddTextureBinding(Content, "location_frame");
            ResourceManager.AddTextureBinding(Content, "circle");
            ResourceManager.AddTextureBinding(Content, "grass");

            ResourceManager.AddTextureBinding(Content, "forest");
            ResourceManager.AddTextureBinding(Content, "water");

            ResourceManager.LoadTextures(Content);

            World.Generate();
            World.LoadCotent(GraphicsDevice);

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

            
            World.Draw(spriteBatch, GraphicsDevice);


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
