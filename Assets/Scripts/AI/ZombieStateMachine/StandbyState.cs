using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace AI.ZombieStateMachine
{
    public class StandbyState : IState
    {
        private ZombieController _zombieController;
        
        public StandbyState(ZombieController aiController)
        {
            _zombieController = aiController;
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
