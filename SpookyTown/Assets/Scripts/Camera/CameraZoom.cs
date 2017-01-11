using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraZoom : MonoBehaviour
    {
        public float maxFov = 25f;

        public float minFov = 2f;

        public float stepFov = 2f;

        public float sizeMax = 5f;

        public float sizeMin = 1f;

        public float sizeStep = 0.5f;

        [UsedImplicitly]
        private void LateUpdate()
        {
            var mainCamera = UnityEngine.Camera.main;
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll < 0)
            {
                if (mainCamera.fieldOfView <= maxFov)
                {
                    mainCamera.fieldOfView += stepFov;
                }

                if (mainCamera.orthographicSize <= sizeMax)
                {
                    mainCamera.orthographicSize += sizeStep;
                }
            }

            if (scroll > 0)
            {
                if (mainCamera.fieldOfView > minFov)
                {
                    mainCamera.fieldOfView -= stepFov;
                }

                if (mainCamera.orthographicSize >= sizeMin)
                {
                    mainCamera.orthographicSize -= sizeStep;
                }
            }
        }
    }
}
