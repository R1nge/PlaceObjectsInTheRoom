using _Assets.Scripts.Gameplay;
using _Assets.Scripts.Services.StateMachine;
using _Assets.Scripts.Services.StateMachine.StatesCreators;
using _Assets.Scripts.Services.UIs;
using _Assets.Scripts.Services.UIs.StateMachine;
using VContainer;
using VContainer.Unity;

namespace _Assets.Scripts.CompositionRoot
{
    public class MainSceneInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerStateController>(Lifetime.Singleton);
            
            builder.Register<MainMenuUIStatesFactory>(Lifetime.Singleton);
            builder.Register<MainMenuUIFactory>(Lifetime.Singleton);
            builder.Register<MainMenuStatesFactory>(Lifetime.Singleton);

            builder.Register<MainSceneStateCreator>(Lifetime.Singleton);
        }
    }
}