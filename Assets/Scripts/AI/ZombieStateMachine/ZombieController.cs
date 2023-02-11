using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace AI.ZombieStateMachine
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class ZombieController : MonoBehaviour
    {
        public GameObject target;
        public Animator animator;
        public Rigidbody zombieRigidbody;
        public float movementSpeed = 100.0f;

        public bool tempFakeRagDollState;
        
        private StateMachine _stateMachine;
        
        private void Awake()
        {
            _stateMachine = new StateMachine();
            animator = GetComponent<Animator>();
            zombieRigidbody = GetComponent<Rigidbody>();

            var standby = new StandbyState(this);
            var lookForTarget = new LookForTargetState(this);
            var moveToTarget = new MoveToTargetState(this);
            var ragdoll = new RagdollState(this);

            //Set from->to state transition condition
            At(standby, lookForTarget, HasGameStarted());
            At(lookForTarget, moveToTarget, HasTarget());

            //Set anystate->to state transition condition
            _stateMachine.AddAnyTransition(ragdoll, CollisionWithTarget());
            _stateMachine.AddAnyTransition(lookForTarget, HasNoTarget());

            //Set starting state
            _stateMachine.SetState(standby);
            
            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            Func<bool> HasGameStarted() => () => GameManager.Instance.currentState == GameState.Started;
            Func<bool> HasTarget() => () => target != null && !tempFakeRagDollState;
            Func<bool> CollisionWithTarget() => () => target != null && tempFakeRagDollState; //TODO: collision w/ vehicule
            Func<bool> HasNoTarget() => () => target == null;
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
