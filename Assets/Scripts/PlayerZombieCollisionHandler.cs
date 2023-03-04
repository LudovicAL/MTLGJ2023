using AI.ZombieStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZombieCollisionHandler : MonoBehaviour
{
    Rigidbody m_rigidbody;
    MeshCollider m_meshCollider;
    Collider[] m_hitResults = new Collider[20];

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_meshCollider = GetComponent<MeshCollider>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        int hitCount = Physics.OverlapBoxNonAlloc(currentPos, m_meshCollider.bounds.extents, m_hitResults, transform.rotation, 1 << 6);
        for (int i = 0; i < hitCount; i++)
        {
            ZombieController zombie = m_hitResults[i].gameObject.GetComponent<ZombieController>();
            zombie.forceRagdoll = true;
        }
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        //Gizmos.DrawCube(Vector3.zero, new Vector3(4, 4, 4));
        //Gizmos.DrawCube(meshCollider.bounds.center, meshCollider.bounds.size);
        Gizmos.DrawCube(Vector3.zero, new Vector3(2, 2, 4)); // TODO: find a way to get the actual car's size
    }
}
