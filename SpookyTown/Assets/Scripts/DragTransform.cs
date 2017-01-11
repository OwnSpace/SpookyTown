using Assets.Scripts.Camera;
using Assets.Scripts.Grid;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    [UsedImplicitly]
    public class DragTransform : MonoBehaviour
    {
        private readonly Color mouseOverColor = Color.blue;

        private readonly Color originalColor = Color.yellow;

        private new Renderer renderer;

        private static GridManager gridManager;

        private float minX;

        private float maxX;

        private float minZ;

        private float maxZ;

        private GameObject isoCamera;

        private Vector3 pos;

        private Transform tr;

        private Plane movePlane;

        private float fixedDistance;

        private float hitDist;

        private float t;

        private Ray camRay;

        private Vector3 point;

        private Vector3 corPoint;

        public float speed = 6f;

        [UsedImplicitly]
        private void Start()
        {
            renderer = GetComponent<Renderer>();
            isoCamera = GameObject.FindGameObjectWithTag("MainCamera");
            gridManager = GridManager.Instance;
            tr = transform;
            pos = tr.position;
            maxX = gridManager.gridWorldSize.x / 2 - gridManager.nodeRadius;
            minX = -maxX;
            maxZ = gridManager.gridWorldSize.y / 2 - gridManager.nodeRadius;
            minZ = -maxZ;
            fixedDistance = pos.y;
        }

        [UsedImplicitly]
        private void OnMouseEnter()
        {
            renderer.material.color = mouseOverColor;
        }

        [UsedImplicitly]
        private void OnMouseExit()
        {
            renderer.material.color = originalColor;
        }

        [UsedImplicitly]
        private void OnMouseDown()
        {
            movePlane = new Plane(-UnityEngine.Camera.main.transform.forward, transform.position);

            ToggleCamera(false);
        }

        [UsedImplicitly]
        private void OnMouseDrag()
        {
            camRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            if (movePlane.Raycast(camRay, out hitDist))
            {
                point = camRay.GetPoint(hitDist);
                t = -(fixedDistance - camRay.origin.y) / (camRay.origin.y - point.y);
                corPoint.x = camRay.origin.x + (point.x - camRay.origin.x) * t;
                corPoint.y = camRay.origin.y + (point.y - camRay.origin.y) * t;
                corPoint.z = camRay.origin.z + (point.z - camRay.origin.z) * t;
                pos = ClampToGridSize(corPoint);
                transform.position = pos;
            }
        }

        [UsedImplicitly]
        private void OnMouseUp()
        {
            FitPositionToNodeSize();
            ToggleCamera(true);
        }

        [UsedImplicitly]
        private void Update()
        {
            Movement();
        }

        private void Movement()
        {
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && tr.position == pos)
            {
                pos += Vector3.right;
            }
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && tr.position == pos)
            {
                pos += Vector3.left;
            }
            else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && tr.position == pos)
            {
                pos += Vector3.forward;
            }
            else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && tr.position == pos)
            {
                pos += Vector3.back;
            }

            pos = ClampToGridSize(pos);

            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        }

        private void ToggleCamera(bool cameraEnabled)
        {
           isoCamera.gameObject.GetComponent<CameraFollow>().enabled = cameraEnabled;
        }

        private Vector3 ClampToGridSize(Vector3 position)
        {
            return new Vector3(Mathf.Clamp(position.x, minX, maxX), position.y, Mathf.Clamp(position.z, minZ, maxZ));
        }

        private void FitPositionToNodeSize()
        {
            var x = Mathf.Round(pos.x);
            var z = Mathf.Round(pos.z);
            x = pos.x > x ? x + 0.5f : x - 0.5f;
            z = pos.z > z ? z + 0.5f : z - 0.5f;
            pos = new Vector3(x, pos.y, z);
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        }
    }
}
