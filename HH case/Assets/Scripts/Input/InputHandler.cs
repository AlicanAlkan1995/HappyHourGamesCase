using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HappyHourGames.Scripts.InputSystem
{
    /// <summary>
    /// Interface that provides definitions for input-related events.
    /// </summary>
    public interface IInputHandler
    {
        event Action<Vector3> OnPanInitiated;
        event Action<Vector3> OnPanPerformed;
        event Action<Vector3> OnClickPerformed;
        event Action<Vector3> OnClickEnded;
        event Action OnPanEnded;
        event Action<float> OnZoomRequested;

        public Vector2 GetMouseWorldPosition();

        public void OnEnable();
        public void OnDisable();
    }

    /// <summary>
    /// Handles the inputs for panning and zooming, raising the appropriate events when these actions occur.
    /// </summary>
    public class InputHandler : IInputHandler, GameInputs.IBoardViewMapActions
    {
        public event Action<Vector3> OnPanInitiated;
        public event Action<Vector3> OnPanPerformed;
        public event Action<Vector3> OnClickPerformed;
        public event Action<Vector3> OnClickEnded;
        public event Action OnPanEnded;
        public event Action<float> OnZoomRequested;

        private readonly UnityEngine.Camera _mainCamera;
        
        private GameInputs _gameInput;

        public InputHandler()
        {
            _mainCamera = UnityEngine.Camera.main;
            _gameInput = new GameInputs();
            _gameInput.BoardViewMap.SetCallbacks(this);
        }
        
        /// <summary>
        /// Enables the board view map input actions.
        /// </summary>
        public void OnEnable() => _gameInput.BoardViewMap.Enable();

        /// <summary>
        /// Disables the board view map input actions.
        /// </summary>
        public void OnDisable() => _gameInput.BoardViewMap.Disable();

        /// <summary>
        /// Handles the panning action based on the phase of the input context.
        /// </summary>
        /// <param name="context">The context related to the pan input action.</param>
        public void OnPan(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OnPanInitiated?.Invoke(Input.mousePosition);
            }
            else if (context.phase == InputActionPhase.Performed)
            {
                OnPanPerformed?.Invoke(Input.mousePosition);
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                OnPanEnded?.Invoke();
            }
        }

        /// <summary>
        /// Handles the zoom action and raises the OnZoomRequested event.
        /// </summary>
        /// <param name="context">The context related to the zoom input action.</param>
        public void OnZoom(InputAction.CallbackContext context)
        {
            OnZoomRequested?.Invoke(context.ReadValue<float>());
        }

         
        /// <summary>
        /// Handles the click action and raises the OnClickPerformed and OnClickCancelled events.
        /// </summary>
        /// <param name="context">The context related to the zoom input action.</param>
        public void OnClick(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnClickPerformed?.Invoke(Input.mousePosition);
                    break;
                case InputActionPhase.Performed:
                    break;
                case InputActionPhase.Canceled:
                    OnClickEnded?.Invoke(Input.mousePosition);
                    break;
            }
        }
        
        public Vector2 GetMouseWorldPosition()
        {
            var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return mouseWorldPosition;
        }
    }
}
