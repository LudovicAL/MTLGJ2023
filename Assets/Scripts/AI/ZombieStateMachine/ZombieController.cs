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
        public bool forceRagdoll;

        [HideInInspector] public Animator animator;
        [HideInInspector] public Rigidbody locomotionRigidbody;
        [HideInInspector] public CapsuleCollider locomotionCollider;
        [HideInInspector] public  bool isColliding;
        
        private StateMachine _stateMachine;

        
        private void Awake()
        {
            _stateMachine = new StateMachine();
            animator = GetComponent<Animator>();
            locomotionRigidbody = GetComponent<Rigidbody>();
            locomotionCollider = GetComponent<CapsuleCollider>();

            var standby = new StandbyState(this);
            var wandering = new WanderingState(this);
            var aggressive = new AgressiveState(this);
            var attackTarget = new AttackTargetState(this);
            var ragdoll = new RagdollState(this);
            var cleanup = new CleanUpState(this);

            //Set from->to state transition condition
            At(standby, wandering, HasGameStarted());
            At(wandering, aggressive, HasTarget());
            At(aggressive, wandering, HasNoTarget());
            At(attackTarget, wandering, HasNoTarget());
            At(aggressive, attackTarget, IsCloseToTarget());
            At(wandering, attackTarget, IsCloseToTarget());
            At(ragdoll, cleanup, IsNotMoving());

            //Set anystate->to state transition condition
            _stateMachine.AddAnyTransition(ragdoll, CollisionWithTarget());

            //Set starting state
            _stateMachine.SetState(standby);
            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            Func<bool> HasGameStarted() => () => GameManager.Instance.currentState == GameState.Started;
            Func<bool> HasTarget() => () => target != null;//Needs to be changed based on lod
            Func<bool> CollisionWithTarget() => () => isColliding || forceRagdoll;
            Func<bool> HasNoTarget() => () => target == null; //Needs to be changed based on lod
            Func<bool> IsCloseToTarget() => () => target != null && Vector3.Distance(target.transform.position, transform.position) <= minDistanceToTarget;
            Func<bool> IsNotMoving() => () => ragdollRigidbody.velocity.magnitude <= 0.5f;
        }

        private void Update()
        {
            _stateMachine.Tick();
            Debug.Log(_stateMachine._currentState.GetType());
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedTick();
        }

        private void OnCollisionEnter(Collision collision)
        {
            
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.relativeVelocity.magnitude >= 10.0f)
                {
                    isColliding = true;
                }
            }
        }
    }
}
