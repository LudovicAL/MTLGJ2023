using System.Collections;
using System.Collections.Generic;
using AI.ZombieStateMachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    private PlayerController _playerController;
    private Vector3 _initialPosition;
        
    void OnEnable()
    {
        _playerController = (PlayerController)target;
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Michael Bay explosion"))
        {
            Vector3 explosionPos = _playerController.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, 50.0f);
            foreach (Collider hit in colliders)
            {
                ZombieController zb = hit.GetComponent<ZombieController>();

                if (zb != null)
                {
                    Debug.Log("has ZB");
                    zb.forceRagdoll = true;
                    zb.StateMachine.Tick();
                    zb.ragdollRigidbody.AddExplosionForce(5000.0f, explosionPos, 50.0f, 30.0F);
                }
                    
            }
        }
        
    }
}
