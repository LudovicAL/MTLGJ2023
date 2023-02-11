using UnityEngine;

namespace AI.ZombieStateMachine
{
    public class MoveToTargetState : IState
    {
        private ZombieController _zombieController;

        public MoveToTargetState(ZombieController zombieController)
        {
            _zombieController = zombieController;
        }

        public void OnEnter()
        {
            _zombieController.animator.SetBool("hasTarget", true);
        }

        public void OnExit()
        {
            _zombieController.animator.SetBool("hasTarget", false);
        }

        public void Tick()
        {
            
        }
        public void FixedTick()
        {
            Vector3 playerPosition = _zombieController.target.transform.position;
            Vector3 zombiePosition = _zombieController.transform.position;
            Vector3 direction = playerPosition - zombiePosition;
            
            
            /*if (!Helpers.HasLineOfSight(_zombieController.transform.position,direction, "Player" ))
            {
                _zombieController.target = null;
                return;
            }*/
            direction.y = 0;
            _zombieController.transform.rotation = Quaternion.Slerp(_zombieController.transform.rotation,
                Quaternion.LookRotation(direction), 0.1f);
            
            _zombieController.locomotionRigidbody.AddForce(direction.normalized * _zombieController.force);
        }
        
    }
}
