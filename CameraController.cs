using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using Framework;

namespace ProjectAlpha2
{
    public class CameraController
    {
        public static void PanCamera(OrthographicCamera camera)
        {
            if (InputManager.GetMouseState().IsButtonDown(MonoGame.Extended.Input.MouseButton.Middle))
            {
                camera.Move(InputManager.GetMouseState().DeltaPosition.ToVector2() / camera.Zoom);
            }

            if (InputManager.GetMouseState().DeltaScrollWheelValue > 0) camera.ZoomOut(0.05f);
            if (InputManager.GetMouseState().DeltaScrollWheelValue < 0) camera.ZoomIn(0.05f);
        }
    }
}