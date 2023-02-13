using UnityEngine;
using System;

namespace AI.ZombieStateMachine
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(ZombieMaterialController))]
    public class ZombieController : MonoBehaviour
    {
        public GameObject target;
        public Rigidbody ragdollRigidbody;
        public float force = 100.0f;
        public float jumpForce = 100;
        public float minDistanceToTarget = 3.5f;
        public float dissolveDelay = 100.0f;
        public int xpValue = 1;
        public bool forceRagdoll;
        public float wanderingSpeed = 5f;

        [HideInInspector] public Animator animator;
        [HideInInspector] public Rigidbody locomotionRigidbody;
        [HideInInspector] public CapsuleCollider locomotionCollider;
        [HideInInspector] public ZombieMaterialController zombieMaterialController;
        [HideInInspector] public bool isColliding;
        [HideInInspector] public Vector3 wantedPosition;

        public StateMachine StateMachine { get; private set; }


        private void Awake()
        {
            StateMachine = new StateMachine();
            animator = GetComponent<Animator>();
            locomotionRigidbody = GetComponent<Rigidbody>();
            locomotionCollider = GetComponent<CapsuleCollider>();
            zombieMaterialController = GetComponent<ZombieMaterialController>();

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
            At(attackTarget, aggressive, IsFarFromTarget());
            At(aggressive, attackTarget, IsCloseToTarget());
            At(wandering, attackTarget, IsCloseToTarget());
            At(ragdoll, cleanup, IsNotMoving());

            //Set anystate->to state transition condition
            StateMachine.AddAnyTransition(ragdoll, CollisionWithTarget());

            //Set starting state
            StateMachine.SetState(standby);
            
            void At(IState to, IState from, Func<bool> condition) => StateMachine.AddTransition(to, from, condition);
            Func<bool> HasGameStarted() => () => GameManager.Instance.currentState == GameState.Started;
            Func<bool> HasTarget() => () => target != null;//Needs to be changed based on lod
            Func<bool> CollisionWithTarget() => () => isColliding || forceRagdoll;
            Func<bool> HasNoTarget() => () => target == null; //Needs to be changed based on lod
            Func<bool> IsCloseToTarget() => () => target != null && Vector3.Distance(target.transform.position, transform.position) <= minDistanceToTarget;
            Func<bool> IsFarFromTarget() => () => target != null && Vector3.Distance(target.transform.position, transform.position) > minDistanceToTarget;
            Func<bool> IsNotMoving() => () => ragdollRigidbody.velocity.magnitude <= 0.5f;
        }

        private void Update()
        {
            StateMachine.Tick();
            //Debug.Log(StateMachine._currentState.GetType());
        }

        private void FixedUpdate()
        {
            StateMachine.FixedTick();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.relativeVelocity.magnitude >= PlayerData.Instance.murderSpeed)
                {
                    isColliding = true;
                }
            }
        }

        public void MoveTowards(Vector3 _wantedPosition)
        {
            wantedPosition = _wantedPosition;
            transform.position = Vector3.MoveTowards(transform.position, wantedPosition, wanderingSpeed * Time.deltaTime);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(wantedPosition, 1f);
        }
    }
}
