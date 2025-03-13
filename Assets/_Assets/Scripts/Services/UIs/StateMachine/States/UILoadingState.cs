using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs.StateMachine.States
{
    public class UILoadingState : IAsyncState
    {
        private readonly MainMenuUIFactory _mainMenuUIFactory;
        private readonly UIStateMachine _uiStateMachine;
        private GameObject _ui;

        public UILoadingState(MainMenuUIFactory mainMenuUIFactory, UIStateMachine uiStateMachine)
        {
            _mainMenuUIFactory = mainMenuUIFactory;
            _uiStateMachine = uiStateMachine;
        }

        public async UniTask Enter()
        {
            _ui = _mainMenuUIFactory.CreateUI(UIStateType.Loading);
        }

        public async UniTask Exit()
        {
            Object.Destroy(_ui);
        }
    }
}