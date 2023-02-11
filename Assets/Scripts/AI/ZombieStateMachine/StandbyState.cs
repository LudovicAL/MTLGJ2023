using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace AI.ZombieStateMachine
{
    public class StandbyState : IState
    {
        private ZombieController _zombieController;
        
        public StandbyState(ZombieController zombieController)
        {
            _zombieController = zombieController;
        }

        public void OnEnter()
        {

        }

        public void OnExit()
        {
        }

        public void Tick() {}
        
        public void FixedTick()
        {
            
        }
    }
}
