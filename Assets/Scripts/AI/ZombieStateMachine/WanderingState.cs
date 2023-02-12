using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.ZombieStateMachine
{
    public class WanderingState : IState
    {
        private ZombieController _zombieController;
        private Transform _playerTransform;
        private int _updateInterval = 10;

        public WanderingState(ZombieController zombieController)
        {
            _zombieController = zombieController;
        }

        public void OnEnter()
        {
            _zombieController.animator.SetBool("isIdle", true);
            _playerTransform = GameManager.Instance.player.transform;
        }

        public void OnExit()
        {
            _zombieController.animator.SetBool("isIdle", false);
        }

        public void Tick()
        {
            if (_zombieController.target != null) return;

            if (Time.frameCount % _updateInterval != 0) return;
            
            //How do we want to acquire targets? Distance, line of sight, sound, all the time? 
            //For now it's line on sight and only on the player but the system can handle decoys at some point

            Vector3 targetPosition = _playerTransform.position;
            Vector3 zombiePosition = _zombieController.transform.position;
            Vector3 direction = targetPosition - zombiePosition;
            if (Helpers.HasLineOfSight(_zombieController.transform.position,direction, "Player" ))
            {
                _zombieController.target = GameManager.Instance.player;
            }
        }
        public void FixedTick()
        {
            
        }
        
    }
}
