using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs.StateMachine.States
{
    public class UIGameState : IAsyncState
    {
        private readonly MainMenuUIFactory _mainMenuUIFactory;
        private GameObject _ui;

        public UIGameState(MainMenuUIFactory mainMenuUIFactory) => _mainMenuUIFactory = mainMenuUIFactory;

        public async UniTask Enter() => _ui = _mainMenuUIFactory.CreateUI(UIStateType.Game);

        public async UniTask Exit() => Object.Destroy(_ui);
    }
}