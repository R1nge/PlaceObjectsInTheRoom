using _Assets.Scripts.Services.StateMachine;
using _Assets.Scripts.Services.StateMachine.StatesCreators;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Assets.Scripts.Misc
{
    public class EntryPoint : MonoBehaviour
    {
        [Inject] private GameStateMachine _gameStateMachine;
        [Inject] private MainSceneStateCreator _mainSceneStateCreator;

        private void Start()
        {
            _mainSceneStateCreator.Init();
            _gameStateMachine.SwitchState(GameStateType.Init).Forget();
        }
    }
}