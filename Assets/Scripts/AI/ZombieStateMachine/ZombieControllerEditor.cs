using UnityEditor;
using UnityEngine;

namespace AI.ZombieStateMachine
{
    [CustomEditor(typeof(ZombieController))]
    [CanEditMultipleObjects]
    public class ZombieControllerEditor : Editor
    {
        public float collisionForce = 10.0f;
        public float verticalImpact = 10.0f;
        public Vector3 impactDirection;

        private ZombieController _zombieController;
        private Vector3 _initialPosition;
        
        void OnEnable()
        {
            _zombieController = (ZombieController)target;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Fake collision"))
            {
                _initialPosition = _zombieController.transform.position;
                _zombieController.animator.enabled = false;
                _zombieController.tempFakeRagDollState = true;
                if (_zombieController.target != null)
                {
                    impactDirection = (_zombieController.target.transform.position - _initialPosition) * -1;
                    impactDirection.y = verticalImpact;
                }
                _zombieController.ragdollRigidbody.AddForce(collisionForce * impactDirection.normalized, ForceMode.Impulse);
            }
                        
            collisionForce = EditorGUILayout.FloatField(collisionForce);
            verticalImpact = EditorGUILayout.FloatField(verticalImpact);
            impactDirection = EditorGUILayout.Vector3Field("Impact vector: ", impactDirection);
            this.Repaint();
            
            if (GUILayout.Button("Reset state"))
            {
                _zombieController.transform.position = _initialPosition;
                _zombieController.animator.enabled = true;
                _zombieController.tempFakeRagDollState = false;
                _zombieController.target = null;
            }
        }
    }
}
