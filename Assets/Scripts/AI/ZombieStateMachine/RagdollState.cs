using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.ZombieStateMachine
{
    public class RagdollState : IState
    {
        private ZombieController _zombieController;

        public RagdollState(ZombieController zombieController)
        {
            _zombieController = zombieController;
        }

        public void OnEnter()
        {
            _zombieController.locomotionCollider.enabled = false;
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
            //_zombieController.transform.position = _zombieController.ragdollRigidbody.transform.position;
        }
    }
}
