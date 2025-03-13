namespace _Assets.Scripts.Gameplay
{
    public class PlayerStateController
    {
        private PlayerState _currentState = PlayerState.Moving;
        public PlayerState CurrentState => _currentState;

        public void SetState(PlayerState state) => _currentState = state;

        public enum PlayerState : byte
        {
            Moving = 1,
            Dragging = 2
        }
    }
}