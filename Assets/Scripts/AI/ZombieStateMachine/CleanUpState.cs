using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace AI.ZombieStateMachine
{
    public class CleanUpState : IState
    {
        private ZombieController _zombieController;
        
        public CleanUpState(ZombieController zombieController)
        {
            _zombieController = zombieController;
        }

        public void OnEnter()
        {
            Debug.Log("Entered cleanup state");
            _zombieController.gameObject.SetActive(false);
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
