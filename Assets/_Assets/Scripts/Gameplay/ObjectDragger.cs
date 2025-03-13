using UnityEngine;
using VContainer;

namespace _Assets.Scripts.Gameplay
{
    public class ObjectDragger : MonoBehaviour
    {
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
            Debug.Log("Check touches");

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    CheckObjectsUnderPosition(ConvertScreenSpaceToWorldSpace(touch.position));
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    Drop();
                }
            }

#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse down");
                CheckObjectsUnderPosition(ConvertScreenSpaceToWorldSpace(Input.mousePosition));
            }

            if (Input.GetMouseButtonUp(0))
            {
                Drop();
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
            else if (hit.transform.parent.TryGetComponent(out DraggableView draggableViewParent))
            {
                _lastObject = draggableViewParent;
                _lastObject.DisableGravity();
                _playerStateController.SetState(PlayerStateController.PlayerState.Dragging);
            }
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