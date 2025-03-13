using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.StateMachine.States
{
    public class GameState : IAsyncState
    {
        private readonly GameStateMachine _stateMachine;

        public GameState(GameStateMachine stateMachine) => _stateMachine = stateMachine;

        public async UniTask Enter() { }

        public async UniTask Exit() { }
    }
}