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
            _playerTransform = PlayerData.Instance.playerVehicle.transform;
            _zombieController.locomotionRigidbody.isKinematic = true;
            
            Vector3 zombiePosition = _zombieController.transform.position;
            _wantedPosition = NavFlow.Instance.FindBestPosition(zombiePosition);
        }

        public void OnExit()
        {
            _zombieController.locomotionRigidbody.isKinematic = false;
            _zombieController.animator.SetBool("isIdle", false);
        }

        public void Tick()
        {
            if (_zombieController.target != null) return;

            Vector3 zombiePosition = _zombieController.transform.position;
            _zombieController.MoveTowards(_wantedPosition);
            zombiePosition = _zombieController.transform.position;
            Vector3 toWantedPos = _wantedPosition - zombiePosition;
            if (toWantedPos.sqrMagnitude < 1f)
                _wantedPosition = NavFlow.Instance.FindBestPosition(zombiePosition);
            
            //How do we want to acquire targets? Distance, line of sight, sound, all the time? 
            //For now it's line on sight and only on the player but the system can handle decoys at some point
            if (Time.frameCount % _updateInterval == 0)
            {
                Vector3 targetPosition = _playerTransform.position;
                Vector3 direction = targetPosition - zombiePosition;
                if (Helpers.HasLineOfSight(zombiePosition, direction, "Player", 60)) // arbitrary distance
                {
                    _zombieController.target = PlayerData.Instance.playerVehicle;
                }
            }
        }
        
        
        
        public void FixedTick()
        {
            
        }
        
    }
}
