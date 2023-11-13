
using UnityEngine;

namespace HappyHourGames.Scripts.Services
{
    /// <summary>
    /// Interface defining methods for camera panning behavior.
    /// </summary>
    public interface ICameraPanner
    {
        void InitiatePan(Vector3 initialPosition);
        void PerformPan(Vector3 currentPosition);
        void EndPan();
        void SetCameraPosition(Vector3 pos);
    }

    /// <summary>
    /// Provides functionality for panning the camera in the game world.
    /// </summary>
    public class CameraPanner : ICameraPanner, IUpdatable
    {
        private float panSpeed;
        private readonly Transform _mainCameraTransform;
        private readonly UnityEngine.Camera _mainCamera;

        private bool _isPanning;
        private Vector3 _originMousePosition;
        private Vector3 _currentMousePosition;
        private const float DampingFactor = 0.05f;

        public CameraPanner(float _panSpeed)
        {
            panSpeed = _panSpeed;
            _mainCamera = UnityEngine.Camera.main;
            if (_mainCamera != null) _mainCameraTransform = _mainCamera.transform;
        }

        /// <summary>
        /// Update method to constantly check and pan the camera.
        /// </summary>
        public void Update() => PanCamera();

        /// <summary>
        /// Initializes the panning process, capturing the initial mouse position.
        /// </summary>
        /// <param name="initialPosition">The initial position of the mouse at the start of the pan.</param>
        public void InitiatePan(Vector3 initialPosition)
        {
            _originMousePosition = GetMouseWorldPosition(initialPosition);
            _isPanning = true;
        }

        /// <summary>
        /// Performs the panning action by calculating the pan delta using current and previous mouse positions.
        /// </summary>
        /// <param name="currentPosition">The current position of the mouse while panning.</param>
        public void PerformPan(Vector3 currentPosition)
        {
            _currentMousePosition = currentPosition;
        }

        /// <summary>
        /// Ends the panning action and stops tracking the mouse movement for panning.
        /// </summary>
        public void EndPan() => _isPanning = false;

        /// <summary>
        /// Main method that translates the camera based on pan deltas and damping.
        /// </summary>
        private void PanCamera()
        {
            if (!_isPanning) return;

            var difference = GetMouseWorldPosition(_currentMousePosition) - _mainCameraTransform.position;
            _mainCameraTransform.position = _originMousePosition - difference;

            // var newPosition = panDelta * (panSpeed * Time.deltaTime);
            // _mainCamera.transform.position = panDelta;
            // _mainCamera.transform.Translate(newPosition, Space.Self);
        }

        private Vector3 GetMouseWorldPosition(Vector3 mousePosition)
        {
            return _mainCamera.ScreenToWorldPoint(mousePosition);
        }

        public void SetCameraPosition(Vector3 pos)
        {
            _mainCameraTransform.position = pos;
        }
    }
}