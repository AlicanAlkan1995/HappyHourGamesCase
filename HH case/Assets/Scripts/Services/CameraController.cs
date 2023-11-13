using System;
using HappyHourGames.Scripts.InputSystem;

namespace HappyHourGames.Scripts.Services
{
    public interface ICameraController
    {
        void Destroy();
    }

    /// <summary>
    /// Manages the camera's movements and zoom, coordinating with input events.
    /// </summary>
    public class CameraController : ICameraController

    {
        // Use public properties or methods to set these from outside if needed.
        private IInputHandler InputHandler { get; set; }
        private ICameraPanner Panner { get; set; }
        private ICameraZoom Zoomer { get; set; }

        public CameraController(IInputHandler inputHandler, ICameraPanner panner, ICameraZoom zoomer)
        {
            InputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
            Panner = panner ?? throw new ArgumentNullException(nameof(panner));
            Zoomer = zoomer ?? throw new ArgumentNullException(nameof(zoomer));

            SubscribeToInputEvents();
        }


        public void Initialize()
        {
            SubscribeToInputEvents();
        }

        public void Destroy()
        {
            UnsubscribeFromInputEvents();
        }

        /// <summary>
        /// Subscribes the camera panning and zooming methods to the input handler's events.
        /// </summary>
        private void SubscribeToInputEvents()
        {
            InputHandler.OnPanInitiated += Panner.InitiatePan;
            InputHandler.OnPanPerformed += Panner.PerformPan;
            InputHandler.OnPanEnded += Panner.EndPan;
            InputHandler.OnZoomRequested += Zoomer.Zoom;
        }

        /// <summary>
        /// Unsubscribes the camera panning and zooming methods from the input handler's events.
        /// </summary>
        private void UnsubscribeFromInputEvents()
        {
            InputHandler.OnPanInitiated -= Panner.InitiatePan;
            InputHandler.OnPanPerformed -= Panner.PerformPan;
            InputHandler.OnPanEnded -= Panner.EndPan;
            InputHandler.OnZoomRequested -= Zoomer.Zoom;
        }
    }
}