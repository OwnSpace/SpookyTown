using UnityEngine;

// ReSharper disable once UnusedMember.Global
namespace Assets.Scripts.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        private Vector3 resetCamera;

        private Vector3 origin;

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            resetCamera = UnityEngine.Camera.main.transform.position;
        }

        // ReSharper disable once UnusedMember.Local
        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                origin = MousePos();
            }

            if (Input.GetMouseButton(0))
            {
                var difference = MousePos() - transform.position;
                transform.position = origin - difference;
            }

            if (Input.GetMouseButton(1))
            {
                transform.position = resetCamera;
            }
        }

        /// <summary>
        /// The position of the mouse in world coordinates
        /// </summary>
        private Vector3 MousePos()
        {
            return UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
