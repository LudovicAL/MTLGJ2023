using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererTest : MonoBehaviour
{
    public Mesh meshToDraw;
    public Material materialToDraw;
    public Material fakeZombieMaterial;
    List<ZombieBoidInfo> m_ZombieBoids = new List<ZombieBoidInfo>();
    List<ZombieBoidInfo> m_PotentialRealZombies = new List<ZombieBoidInfo>();
    List<ZombieBoidInfo> m_TriagedPotentialRealZombies = new List<ZombieBoidInfo>();

    List<ZombieBoidInfo> m_PreviousFrameRealZombies = new List<ZombieBoidInfo>();
    List<ZombieBoidInfo> m_CurrentFrameRealZombies = new List<ZombieBoidInfo>();

    private Transform playerCar;

    // Start is called before the first frame update
    void Start()
    {
        playerCar = GameObject.FindWithTag("Player").transform;

        float halfWidth = 75.0f;
        float halfHeight = 75.0f;
        float zombieHeight = 1.0f;

        int numberOfX = 200;
        int numberOfY = 200;

        float xOffset = (2.0f * halfWidth) / (float)numberOfX;
        float yOffset = (2.0f * halfHeight) / (float)numberOfY;

        for (int x = 0; x < 100; ++x)
        {
            for (int y = 0; y < 100; ++y)
            {
                float xPos = (x - halfWidth) * xOffset;
                float yPos = (y - halfHeight) * yOffset;

                ZombieBoidInfo z = new ZombieBoidInfo();
                z.position = transform.position + new Vector3(xPos, zombieHeight, yPos);
                z.rotation = transform.rotation;
                z.acceleration = Vector3.zero;
                m_ZombieBoids.Add(z);
            }
        }
    }

    private float zombieMaxAcceleration = 1.0f;
    private float zombieMaxSpeed = 10.0f;
    private float zombieAccelerationRandomRange = 0.01f;

    private int RealZombieLimit = 200;
    private float RealZombieRadius = 40;

    // Update is called once per frame
    void Update()
    {
        m_PotentialRealZombies.Clear();
        m_TriagedPotentialRealZombies.Clear();
        Vector3 zombieHeightOffset = new Vector3(0.0f, 2.5f, 0.0f);

        foreach (ZombieBoidInfo zombidBoid in m_ZombieBoids)
        {
            zombidBoid.isRealZombie = false;
            if (!zombidBoid.zombieDead)
            {
                zombidBoid.acceleration += new Vector3(Random.Range(-zombieAccelerationRandomRange, zombieAccelerationRandomRange), 0.0f, Random.Range(-zombieAccelerationRandomRange, zombieAccelerationRandomRange));

                if (zombidBoid.acceleration.magnitude > zombieMaxAcceleration)
                {
                    zombidBoid.acceleration = zombidBoid.acceleration.normalized * zombieMaxAcceleration;
                }

                zombidBoid.velocity += zombidBoid.acceleration * Time.deltaTime;
                if (zombidBoid.velocity.magnitude > zombieMaxSpeed)
                {
                    zombidBoid.velocity = zombidBoid.velocity.normalized * zombieMaxSpeed;
                }
            }

            zombidBoid.position += zombidBoid.velocity * Time.deltaTime;
            zombidBoid.rotation.SetLookRotation(zombidBoid.velocity);

            //if ((playerCar.position - zombidBoid.position).magnitude < 2.0f)
            //{
            //    zombidBoid.zombieDead = true;
            //    zombidBoid.velocity.x = Random.Range(-15.0f, 15.0f);
            //    zombidBoid.velocity.y = 20.0f;
            //    zombidBoid.velocity.z = Random.Range(-15.0f, 15.0f);
            //}

            zombidBoid.zombieDistanceFromPlayer = (playerCar.position - zombidBoid.position).magnitude;
            if (zombidBoid.zombieDistanceFromPlayer < RealZombieRadius)
            {
                Vector3 screenPosition = Camera.main.WorldToViewportPoint(zombidBoid.position + zombieHeightOffset);
                if (screenPosition.x >= -0.2f && screenPosition.x <= 1.2f
                    && screenPosition.y >= 0.0f && screenPosition.y <= 1.0f
                    && screenPosition.z >= 0.0f)
                {
                    m_PotentialRealZombies.Add(zombidBoid);
                }
            }
        }

        float cutOffDistance = 999999.9f;
        if (m_PotentialRealZombies.Count > RealZombieLimit)
        {
            foreach (ZombieBoidInfo potentialZombidBoid in m_PotentialRealZombies)
            {
                if (potentialZombidBoid.zombieDistanceFromPlayer > cutOffDistance)
                    continue;

                if (m_TriagedPotentialRealZombies.Count == 0)
                {
                    m_TriagedPotentialRealZombies.Add(potentialZombidBoid);
                }
                else
                {
                    int index = 0;
                    bool zombieAdded = false;
                    foreach (ZombieBoidInfo triagedPotentialZombidBoid in m_TriagedPotentialRealZombies)
                    {
                        if (potentialZombidBoid.zombieDistanceFromPlayer < triagedPotentialZombidBoid.zombieDistanceFromPlayer)
                        {
                            m_TriagedPotentialRealZombies.Insert(index, potentialZombidBoid);
                            zombieAdded = true;
                            break;
                        }
                        ++index;
                        if (index >= RealZombieLimit)
                            break;
                    }

                    if (!zombieAdded && index < RealZombieLimit)
                    {
                        m_TriagedPotentialRealZombies.Add(potentialZombidBoid);
                    }
                }
               

                if (m_TriagedPotentialRealZombies.Count > RealZombieLimit)
                {
                    m_TriagedPotentialRealZombies.RemoveAt(RealZombieLimit);
                    cutOffDistance = m_TriagedPotentialRealZombies[RealZombieLimit - 1].zombieDistanceFromPlayer;
                }

            }

            foreach (ZombieBoidInfo triagedPotentialZombidBoid in m_TriagedPotentialRealZombies)
            {
                triagedPotentialZombidBoid.isRealZombie = true;
            }
        }
        else
        {
            foreach (ZombieBoidInfo potentialZombidBoid in m_PotentialRealZombies)
            {
                potentialZombidBoid.isRealZombie = true;
            }
        }

        foreach (ZombieBoidInfo zombidBoid in m_ZombieBoids)
        {
            if (zombidBoid.isRealZombie)
            {
                Matrix4x4 m = Matrix4x4.TRS(zombidBoid.position, zombidBoid.rotation, Vector3.one * 2.0f);
                Graphics.DrawMesh(meshToDraw, m, materialToDraw, 0);
            }
            else
            {
                Matrix4x4 m = Matrix4x4.TRS(zombidBoid.position, zombidBoid.rotation, Vector3.one * 2.0f);
                Graphics.DrawMesh(meshToDraw, m, fakeZombieMaterial, 0);
            }
        }
    }
}

public class ZombieBoidInfo
{
    public Vector3 position;
    public Vector3 acceleration;
    public Vector3 destination;
    public Quaternion rotation;
    public Vector3 velocity;
    public bool zombieDead = false;
    public float zombieDistanceFromPlayer = 0.0f;
    public bool isRealZombie = false;
}