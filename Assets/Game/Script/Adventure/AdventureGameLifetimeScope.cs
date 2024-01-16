using MessagePipe;
using RPG.Adventure;
using RPG.Adventure.Input;
using RPG.Adventure.Player;
using RPG.Sandbox;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

public class AdventureGameLifetimeScope : LifetimeScope
{
    [SerializeField, Tooltip("PlayerのStateMachine")]
    private PlayerStateMachine stateMachine = default;
    
    protected override void Configure(IContainerBuilder builder)
    {
        // MessagePipeの設定
        var options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<PlayerAdventureInput>(options);
        
        // Provider
        builder.RegisterEntryPoint<PlayerAdventureInputProvider>(Lifetime.Singleton);

        builder.RegisterComponent(stateMachine.PlayerProperty);
        
        // Manager
        builder.Register<IAdventureManager, TestAdventureManager>(Lifetime.Singleton);
    }
}
