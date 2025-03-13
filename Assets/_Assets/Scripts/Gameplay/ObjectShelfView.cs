using UnityEngine;

namespace _Assets.Scripts.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ObjectShelfView : MonoBehaviour
    {
        [SerializeField] private Transform shelfTransform;
        private DraggableView _draggable;

        public void Place(DraggableView draggableView)
        {
            draggableView.DisableGravity();
            draggableView.transform.SetParent(shelfTransform);
            draggableView.transform.localPosition = Vector3.zero;
            _draggable = draggableView;
        }

        public DraggableView Take()
        {
            if (_draggable != null)
            {
                _draggable.transform.parent = null;
                return _draggable;
            }

            return null;
        }
    }
}