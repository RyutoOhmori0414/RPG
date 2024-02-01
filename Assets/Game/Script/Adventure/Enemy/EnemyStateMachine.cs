using UnityEngine;
using RPG.CommonStateMachine;
using RPG.Adventure.Player;
using UnityEngine.Serialization;
using VContainer;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPG.Adventure.Enemy
{
    public class EnemyStateMachine : AbstractStateMachine<AbstractEnemyState>
    {
        [SerializeField, Tooltip("EnemyのData")]
        private EnemyPropertyScriptableObject _property = default;
        [FormerlySerializedAs("player")] [SerializeField]
        private PlayerStateMachine _player = default;

        [Inject]
        private IAdventureManager _adventureManager = default;

        public IAdventureManager AdventureManager => _adventureManager;
        
        private Animator _animator = default;
        public Animator EnemyAnimator => _animator;
        
        public Transform PlayerTransform => _player.transform;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            // キャッシュの初期化
            InitCache(new EnemySearchState(_property, this));
            InitCache(new EnemyChaseState(_property, this));
            InitCache(new EnemyAttackState(_property, this));
        }

        private void Start()
        {
            TransitionState<EnemySearchState>();
        }

        private void OnDestroy()
        {
            CacheClear();
        }

        /// <summary>攻撃した際のAnimationEvent</summary>
        private void OnAttackAnimationEvent() => GetCache<EnemyAttackState>().OnAttackAnimationEvent();
        
        /// <summary>攻撃終了を伝えるAnimationEvent</summary>
        private void OnAttackEndAnimationEvent() => GetCache<EnemyAttackState>().OnAttackEndAnimationEvent();

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying)
            {
                ((AbstractEnemyState)_currentState).OnDrawGizmo();
            }
            else
            {
                if (TryGetCache(out EnemySearchState search))
                {
                    search.OnDrawGizmo();
                }
                else
                {
                    InitCache(new EnemySearchState(_property, this));
                    GetCache<EnemySearchState>().OnDrawGizmo();
                }
                
                if (TryGetCache(out EnemyChaseState chase))
                {
                    chase.OnDrawGizmo();
                }
                else
                {
                    InitCache(new EnemyChaseState(_property, this));
                    GetCache<EnemyChaseState>().OnDrawGizmo();
                }
                
                if (TryGetCache(out EnemyAttackState attack))
                {
                    attack.OnDrawGizmo();
                }
                else
                {
                    InitCache(new EnemyAttackState(_property, this));
                    GetCache<EnemyAttackState>().OnDrawGizmo();
                }
            }
        }
#endif
    }
}