using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.ZombieStateMachine
{
    public class RagdollState : IState
    {
        private ZombieController _zombieController;
        private float _minForce = 250.0f;
        private float _maxForce = 3000.0f;
        
        private float _minVerticalForce = 5.0f;
        private float _maxVerticalForce = 20.0f;
        
        
        public RagdollState(ZombieController zombieController)
        {
            _zombieController = zombieController;
        }

        public void OnEnter()
        {
            ExecuteCollision();
        }

        public void OnExit()
        {
            _zombieController.locomotionCollider.enabled = true;
        }

        public void Tick()
        {
            
        }

        public void FixedTick()
        {
        }

        private void ExecuteCollision()
        {
            float impactForce = Random.Range(_minForce, _maxForce);
            float verticalImpact = Random.Range(_minVerticalForce, _maxVerticalForce);
            
            _zombieController.locomotionCollider.enabled = false;
            _zombieController.animator.enabled = false;
            Vector3 impactDirection = (_zombieController.target.transform.position - _zombieController.transform.position) * -1;
            impactDirection.y = verticalImpact;
            _zombieController.ragdollRigidbody.AddForce(impactForce * impactDirection.normalized, ForceMode.Impulse);
        }
    }
}
