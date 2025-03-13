using UnityEngine;
using VContainer;

namespace _Assets.Scripts.Gameplay
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Vector2 boundaries;
        [SerializeField] private new Camera camera;
        [SerializeField] private Transform cameraStartTransform;
        [Inject] private PlayerStateController _playerStateController;
        private Vector2 _lastPosition;
        private Vector2 _touchStartPosition, _touchCurrentPosition;

        private void Awake()
        {
            ResetCameraPosition();
        }

        private void OnValidate()
        {
            if (boundaries.x < 0)
            {
                boundaries.x *= -1;
            }

            if (boundaries.y < 0)
            {
                boundaries.y *= -1;
            }
        }

        private void Update()
        {
            if (_playerStateController.CurrentState == PlayerStateController.PlayerState.Moving)
            {
                DetectTouches();
                CalculateNewCameraPosition();
                _lastPosition = CheckBoundaries(_lastPosition);
                SetCameraPosition(_lastPosition);
            }
        }

        private void LateUpdate()
        {
            camera.transform.position = CheckBoundaries(camera.transform.position);
        }

        private void ResetCameraPosition()
        {
            SetCameraPosition(cameraStartTransform.position);
        }

        private void SetCameraPosition(Vector3 position)
        {
            position.z = -10;
            _lastPosition = position;
            camera.transform.position = position;
        }


        private Vector3 CheckBoundaries(Vector3 position)
        {
            if (position.x > boundaries.x)
            {
                position.x = boundaries.x;
            }

            if (position.x < -boundaries.x)
            {
                position.x = -boundaries.x;
            }

            if (position.y > boundaries.y)
            {
                position.y = boundaries.y;
            }

            if (position.y < -boundaries.y)
            {
                position.y = -boundaries.y;
            }

            return position;
        }

        private void CalculateNewCameraPosition()
        {
            var direction = _touchStartPosition - _touchCurrentPosition;
            _lastPosition += direction;
        }

        private void DetectTouches()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    _touchStartPosition = ConvertScreenSpaceToWorldSpace(touch.position);
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    StopMovement();
                }

                _touchCurrentPosition = ConvertScreenSpaceToWorldSpace(touch.position);
            }

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                _touchStartPosition = ConvertScreenSpaceToWorldSpace(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                _touchCurrentPosition = ConvertScreenSpaceToWorldSpace(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopMovement();
            }
#endif
        }

        private void StopMovement()
        {
            _touchStartPosition = camera.transform.position;
            _touchCurrentPosition = camera.transform.position;
        }

        private Vector3 ConvertScreenSpaceToWorldSpace(Vector2 screenPosition) =>
            camera.ScreenToWorldPoint(screenPosition);
    }
}