using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AI.ZombieStateMachine
{
    public class RagdollState : IState
    {
        private ZombieController _zombieController;

        public RagdollState(ZombieController aiController)
        {
            _zombieController = aiController;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
            //Do something once when exiting this state
        }

        public void Tick()
        {
            
        }

        public void FixedTick()
        {
            
        }
    }
}
