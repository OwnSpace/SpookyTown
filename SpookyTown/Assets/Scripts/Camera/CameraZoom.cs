using JetBrains.Annotations;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float maxFov = 25.0f;

    public float minFov = 2.0f;

    public float stepFov = 2.0f;

    public float sizeMax = 5.0f;

    public float sizeMin = 1.0f;

    public float sizeStep = 0.5f;

    [UsedImplicitly]
    private void LateUpdate()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0)
        {
            if (Camera.main.fieldOfView <= maxFov)
            {
                Camera.main.fieldOfView += stepFov;
            }

            if (Camera.main.orthographicSize <= sizeMax)
            {
                Camera.main.orthographicSize += sizeStep;
            }

        }

        if (scroll > 0)
        {
            if (Camera.main.fieldOfView > minFov)
            {
                Camera.main.fieldOfView -= stepFov;
            }

            if (Camera.main.orthographicSize >= sizeMin)
            {
                Camera.main.orthographicSize -= sizeStep;
            }
        }
    }
}
