using MessagePipe;
using RPG.Adventure;
using RPG.Adventure.Enemy;
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

    [SerializeField]
    private EnemyStateMachine[] _enemyStateMachines = default;
    
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
        
        // 敵に参照を渡す
        foreach (var n in _enemyStateMachines)
        {
            builder.RegisterComponent(n);
        }
    }
}
