using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace AI.ZombieStateMachine
{
    public class CleanUpState : IState
    {
        private ZombieController _zombieController;
        private float _dissolveDelay;
        
        public CleanUpState(ZombieController zombieController)
        {
            _zombieController = zombieController;
            _dissolveDelay = _zombieController.dissolveDelay;
        }

        public void OnEnter()
        {
            _zombieController.ragdollRigidbody.Sleep();
            _zombieController.locomotionRigidbody.Sleep();
            
            _zombieController.zombieMaterialController.onDissolveDone.AddListener(OnExit);
        }

        public void OnExit()
        {
            _zombieController.gameObject.SetActive(false);
            _dissolveDelay = 0.0f;
            _zombieController.zombieMaterialController.Reset();
            _zombieController.zombieMaterialController.onDissolveDone.RemoveListener(OnExit);
        }

        public void Tick()
        {
            _dissolveDelay -= 1.0f;

            if (_dissolveDelay <= 0.0f)
            {
                _zombieController.zombieMaterialController.StartDissolve();
            }

        }
        
        public void FixedTick()
        {
            
        }
    }
}
