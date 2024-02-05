using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace RPG.Battle.System
{
    public class BattleLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var option = builder.RegisterMessagePipe();

            builder.RegisterMessageBroker<PhaseParams>(option);

            builder.RegisterEntryPoint<BattleEventManager>(Lifetime.Singleton);
        }
    }   
}