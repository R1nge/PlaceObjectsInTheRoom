using UnityEngine;

namespace _Assets.Scripts.Gameplay
{
    public class DraggableView : MonoBehaviour
    {
        [SerializeField] private new Rigidbody2D rigidbody2D;
        private float _startGravityScale;

        private void Awake()
        {
            _startGravityScale = rigidbody2D.gravityScale;
        }

        public void EnableGravity()
        {
            rigidbody2D.gravityScale = _startGravityScale;
        }

        public void DisableGravity()
        {
            rigidbody2D.gravityScale = 0;
        }
    }
}