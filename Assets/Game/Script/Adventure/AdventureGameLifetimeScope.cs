using MessagePipe;
using RPG.Adventure.Input;
using RPG.Adventure.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class AdventureGameLifetimeScope : LifetimeScope
{
    [SerializeField, Tooltip("PlayerのStateMachine")]
    private PlayerStateMachine _stateMachine = default;
    
    protected override void Configure(IContainerBuilder builder)
    {
        // MessagePipeの設定
        var options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<PlayerAdventureInput>(options);
        
        // Provider
        builder.RegisterEntryPoint<PlayerAdventureInputProvider>(Lifetime.Singleton);

        builder.RegisterComponent(_stateMachine.PlayerProperty);
    }
}
