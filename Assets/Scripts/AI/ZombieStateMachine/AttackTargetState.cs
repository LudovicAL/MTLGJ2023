using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
            Vector3 zombiePosition = _zombieController.transform.position;
            Vector3 targetPosition = _zombieController.target.transform.position;
            Vector3 toTarget = targetPosition - zombiePosition;
            toTarget.y = 0f;
            Vector3 toTargetNorm = toTarget.normalized;
            
            _zombieController.transform.rotation = Quaternion.Slerp(_zombieController.transform.rotation,
                Quaternion.LookRotation(toTargetNorm), 0.1f);
            
            _zombieController.locomotionRigidbody.AddForce(toTargetNorm * _zombieController.force);
        }

        public void FixedTick()
        {
                
        }
    }
}
