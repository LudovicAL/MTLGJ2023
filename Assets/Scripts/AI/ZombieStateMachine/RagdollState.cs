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
            PlayerData.Instance.Xp.Add(_zombieController.xpValue);
            ExecuteCollision();
        }

        public void OnExit()
        {
        }

        public void Tick()
        {
            
        }

        public void FixedTick()
        {
        }

        private void ExecuteCollision()
        {
            if (_zombieController.target == null || _zombieController.forceRagdoll) return;
            
            float impactForce = Random.Range(_minForce, _maxForce);
            float verticalImpact = Random.Range(_minVerticalForce, _maxVerticalForce);
            
            _zombieController.locomotionCollider.enabled = false;
            _zombieController.animator.enabled = false;
            Vector3 impactDirection = (_zombieController.target.transform.position - _zombieController.transform.position) * -1;
            impactDirection.y = verticalImpact;
            _zombieController.ragdollRigidbody.AddForce(impactForce * impactDirection.normalized, ForceMode.Impulse);
            
            _zombieController.isColliding = false;
            _zombieController.forceRagdoll = false;
        }
    }
}
