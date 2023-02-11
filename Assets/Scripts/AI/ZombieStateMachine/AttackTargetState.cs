using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.ZombieStateMachine
{
    public class AttackTargetState : IState
    {
        private ZombieController _zombieController;
        private float distanceOffset = 1.5f;
        
        public AttackTargetState(ZombieController zombieController)
        {
            _zombieController = zombieController;
        }

        public void OnEnter()
        {
            _zombieController.animator.SetBool("attackTarget", true);
            _zombieController.locomotionRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void OnExit()
        {
            _zombieController.animator.SetBool("attackTarget", false);
            _zombieController.locomotionRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        public void Tick()
        {
            if (Vector3.Distance(_zombieController.target.transform.position, _zombieController.transform.position) >
                _zombieController.minDistanceToTarget + distanceOffset)
            {
                _zombieController.target = null;
            }
        }

        public void FixedTick()
        {
                
        }
    }
}
