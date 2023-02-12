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

        private Vector3 _wantedPosition;
        
        public WanderingState(ZombieController zombieController)
        {
            _zombieController = zombieController;
        }

        public void OnEnter()
        {
            _zombieController.animator.SetBool("isIdle", true);
            _playerTransform = GameManager.Instance.player.transform;
            // TODO: sleep physics
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
            if (Helpers.HasLineOfSight(zombiePosition, direction, "Player", 15)) // arbitrary distance
            {
                _zombieController.target = GameManager.Instance.player;
                return;
            }

            _wantedPosition = NavFlow.Instance.FindBestPosition(zombiePosition);
            _zombieController.MoveTowards(_wantedPosition);
        }
        
        
        
        public void FixedTick()
        {
            
        }
        
    }
}
