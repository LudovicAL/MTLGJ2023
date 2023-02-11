using UnityEngine;
using System;

namespace AI.ZombieStateMachine
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class ZombieController : MonoBehaviour
    {
        public GameObject target;
        public Rigidbody ragdollRigidbody;
        public float force = 100.0f;
        public float minDistanceToTarget = 3.5f;

        public bool tempFakeRagDollState;

        [HideInInspector] public Animator animator;
        [HideInInspector] public Rigidbody locomotionRigidbody;
        [HideInInspector] public CapsuleCollider locomotionCollider;
        
        private StateMachine _stateMachine;
        
        private void Awake()
        {
            _stateMachine = new StateMachine();
            animator = GetComponent<Animator>();
            locomotionRigidbody = GetComponent<Rigidbody>();
            locomotionCollider = GetComponent<CapsuleCollider>();

            var standby = new StandbyState(this);
            var lookForTarget = new LookForTargetState(this);
            var moveToTarget = new MoveToTargetState(this);
            var attackTarget = new AttackTargetState(this);
            var ragdoll = new RagdollState(this);

            //Set from->to state transition condition
            At(standby, lookForTarget, HasGameStarted());
            At(lookForTarget, moveToTarget, HasTarget());

            //Set anystate->to state transition condition
            _stateMachine.AddAnyTransition(ragdoll, CollisionWithTarget());
            _stateMachine.AddAnyTransition(lookForTarget, HasNoTarget());
            _stateMachine.AddAnyTransition(attackTarget, IsCloseToTarget());

            //Set starting state
            _stateMachine.SetState(standby);
            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            Func<bool> HasGameStarted() => () => GameManager.Instance.currentState == GameState.Started;
            Func<bool> HasTarget() => () => target != null;
            Func<bool> CollisionWithTarget() => () => tempFakeRagDollState; //TODO: collision w/ vehicule
            Func<bool> HasNoTarget() => () => target == null;
            Func<bool> IsCloseToTarget() => () => target != null && Vector3.Distance(target.transform.position, transform.position) <= minDistanceToTarget;
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedTick();
        }
    }
}
