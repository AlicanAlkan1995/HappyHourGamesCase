using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HappyHourGames.Scripts.Services
{
    public interface ICameraZoom
    {
        void Zoom(float zoomAmount);
    }

    public class CameraZoom : ICameraZoom
    {
        private float minZoom;
        private float maxZoom;
        private float zoomSpeed;
        private int screenCenterThreshold;
        private UnityEngine.Camera _mainCamera;

        public CameraZoom(float minZoom, float maxZoom, float zoomSpeed, int screenCenterThreshold)
        {
            this.minZoom = minZoom;
            this.maxZoom = maxZoom;
            this.zoomSpeed = zoomSpeed;
            this.screenCenterThreshold = screenCenterThreshold;

            _mainCamera = UnityEngine.Camera.main;
        }

        /// <summary>
        /// Adjust the camera's zoom based on input zoomAmount.
        /// </summary>
        /// <param name="zoomAmount">Positive values zoom in, negative values zoom out.</param>
        public void Zoom(float zoomAmount)
        {
            // If the mouse is close to the center of the screen, do not zoom.
            //  if (IsMouseCloseToScreenCenter()) // WHY DO WE HAVE THIS
            //   return;

            AdjustCameraZoom(zoomAmount);
        }

        /// <summary>
        /// Determines if the mouse is close to the screen's center.
        /// </summary>
        /// <returns>True if close to center, otherwise false.</returns>
        private bool IsMouseCloseToScreenCenter()
        {
            var screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f);
            var mouseScreenPos = Input.mousePosition;
            var distanceFromCenter = Vector3.Distance(mouseScreenPos, screenCenter);

            return distanceFromCenter < screenCenterThreshold;
        }

        /// <summary>
        /// Adjusts the camera's orthographic size based on the zoom amount.
        /// </summary>
        /// <param name="zoomAmount">Amount to zoom. Positive values zoom in, negative values zoom out.</param>
        private void AdjustCameraZoom(float zoomAmount)
        {
            var actualZoomAmount = zoomAmount * zoomSpeed;
            var orthographicSize = _mainCamera.orthographicSize;
            var newZoom = orthographicSize - actualZoomAmount;
            var clampedZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

            _mainCamera.orthographicSize = Mathf.Lerp(orthographicSize, clampedZoom, Time.deltaTime * zoomSpeed);
        }
    }
}