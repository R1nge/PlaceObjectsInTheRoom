using UnityEngine;
using VContainer;

namespace _Assets.Scripts.Gameplay
{
    public class ObjectDragger : MonoBehaviour
    {
        [SerializeField] private LayerMask shelf;
        [SerializeField] private new Camera camera;
        [Inject] private PlayerStateController _playerStateController;
        private DraggableView _lastObject;

        private void Update()
        {
            CheckTouches();
            Drag();
        }

        private void CheckTouches()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                var touchWorldPosition = ConvertScreenSpaceToWorldSpace(touch.position);

                if (touch.phase == TouchPhase.Began)
                {
                    CheckObjectsUnderPosition(touchWorldPosition);
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (!TryPlace(touchWorldPosition))
                    {
                        Drop();
                    }
                }
            }

#if UNITY_EDITOR

            var mouseWorldPosition = ConvertScreenSpaceToWorldSpace(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse down");
                CheckObjectsUnderPosition(mouseWorldPosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!TryPlace(mouseWorldPosition))
                {
                    Debug.Log("DROP");
                    Drop();
                }
            }
#endif
        }

        private void CheckObjectsUnderPosition(Vector3 screenPosition)
        {
            var hit = Physics2D.Raycast(screenPosition, Vector2.zero, Mathf.Infinity);

            if (hit.collider == null)
            {
                Debug.LogWarning($"[Object Dragger] No object under position: {screenPosition}");
                return;
            }

            if (hit.transform.TryGetComponent(out DraggableView draggableView))
            {
                _lastObject = draggableView;
                _lastObject.DisableGravity();
                _playerStateController.SetState(PlayerStateController.PlayerState.Dragging);
            }
            else if (hit.transform.TryGetComponent(out ObjectShelfView objectShelfView))
            {
                _lastObject = objectShelfView.Take();
                _playerStateController.SetState(PlayerStateController.PlayerState.Dragging);
            }
        }

        private bool TryPlace(Vector3 screenPosition)
        {
            // Could've used RaycastAll and looped through all objects instead
            var hit = Physics2D.Raycast(screenPosition, Vector2.zero, Mathf.Infinity, shelf);

            if (hit.collider == null)
            {
                Debug.LogWarning($"[Object Dragger] No object under position: {screenPosition}");
                return false;
            }

            if (hit.transform.TryGetComponent(out ObjectShelfView objectShelfView))
            {
                objectShelfView.Place(_lastObject);
                _lastObject = null;
                _playerStateController.SetState(PlayerStateController.PlayerState.Moving);
            }

            return true;
        }

        private void Drag()
        {
            if (_lastObject != null)
            {
#if !UNITY_EDITOR
                var worldPositionTouch = ConvertScreenSpaceToWorldSpace(Input.GetTouch(0).position);
                worldPositionTouch.z = 0;
                _lastObject.transform.position = worldPositionTouch;
#endif


#if UNITY_EDITOR
                var worldPositionMouse = ConvertScreenSpaceToWorldSpace(Input.mousePosition);
                worldPositionMouse.z = 0;
                _lastObject.transform.position = worldPositionMouse;
#endif
            }
        }

        private void Drop()
        {
            if (_lastObject != null)
            {
                _lastObject.EnableGravity();
                _lastObject = null;
                _playerStateController.SetState(PlayerStateController.PlayerState.Moving);
            }
        }

        private Vector3 ConvertScreenSpaceToWorldSpace(Vector2 screenPosition) =>
            camera.ScreenToWorldPoint(screenPosition);
    }
}