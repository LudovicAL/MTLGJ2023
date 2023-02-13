using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshRendererTest : MonoBehaviour
{
    public GameObject zombie;

    public Mesh meshToDraw;
    public Material materialToDraw;
    public Material fakeZombieMaterial;
    public Material fakeSpawnedZombieMaterial;
    List<ZombieBoidInfo> m_ZombieBoids = new List<ZombieBoidInfo>();
    List<ZombieBoidInfo> m_PotentialRealZombies = new List<ZombieBoidInfo>();
    List<ZombieBoidInfo> m_TriagedPotentialRealZombies = new List<ZombieBoidInfo>();
    List<ZombieBoidInfo> m_NotRealZombies = new List<ZombieBoidInfo>();

    public List<GameObject> m_PooledZombies = new List<GameObject>();

    private Transform playerCar;

    private float zombieMaxAcceleration = 1.0f;
    private float zombieMaxSpeed = 2.5f;
    private float zombieAccelerationRandomRange = 0.01f;

    public int RealZombieLimit = 100;
    public float RealZombieRadius = 40;

    private float zombieDistanceBuffer = 3.0f;

    private int pooledZombiesSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        float zombieOffset = 5.0f;

        int numberOfX = 25;
        int numberOfY = 25;

        float halfWidth = ((float)numberOfX / 2.0f) * zombieOffset;
        float halfHeight = ((float)numberOfY / 2.0f) * zombieOffset;

        Vector3 startingOffset = transform.forward * 50.0f;

        for (int x = 0; x < numberOfX; ++x)
        {
            for (int y = 0; y < numberOfY; ++y)
            {
                float xPos = (x * zombieOffset);
                float yPos = (y * zombieOffset);

                ZombieBoidInfo z = new ZombieBoidInfo();
                z.position = transform.position + new Vector3(xPos, 0.0f, yPos) + startingOffset;
                z.rotation = transform.rotation;
                z.acceleration = Vector3.zero;
                m_ZombieBoids.Add(z);
            }
        }

        /*
        for (int x = 0; x < RealZombieLimit; ++x)
        {
            GameObject zombieSpawn = GameObject.Instantiate(zombie, transform);
            AI.ZombieStateMachine.ZombieController zombieController = zombieSpawn.GetComponent<AI.ZombieStateMachine.ZombieController>();
            //Put zombies into "sleep" mode for pooling.
            m_PooledZombies.Add(zombieSpawn);
        }
        */
    }

    public GameObject RequestPooledZombie(Vector3 position)
    {
        if (m_PooledZombies.Count > 0)
        {
            GameObject zombie = m_PooledZombies[0];
            m_PooledZombies.RemoveAt(0);
            zombie.SetActive(true);

            //zombie.transform.position = position;
            zombie.GetComponent<AI.ZombieStateMachine.ZombieController>().locomotionRigidbody.position = position;
            //zombie.GetComponent<AI.ZombieStateMachine.ZombieController>().locomotionRigidbody.isKinematic = false;

            return zombie;
        }
        return null;
    }

    public void ReturnPooledZombie(GameObject zombie)
    {
        //zombie.GetComponent<AI.ZombieStateMachine.ZombieController>().locomotionRigidbody.isKinematic = true;
        //zombie.transform.position = Vector3.down * 100.0f; //HACK

        zombie.SetActive(false);
        m_PooledZombies.Add(zombie);
    }


    // Update is called once per frame
    void Update()
    {
        if (playerCar == null)
        {
            GameObject carObject = GameObject.FindWithTag("Player");
            if (carObject != null)
            {
                playerCar = carObject.transform;
            }
            return;
        }

        if (pooledZombiesSpawned < RealZombieLimit)
        {
            GameObject zombieSpawn = GameObject.Instantiate(zombie, transform);
            zombieSpawn.SetActive(false);
            //zombieSpawn.GetComponent<AI.ZombieStateMachine.ZombieController>().locomotionRigidbody.isKinematic = true;
            //zombieSpawn.transform.position = Vector3.down * 100.0f; //HACK
            m_PooledZombies.Add(zombieSpawn);
            ++pooledZombiesSpawned;
        }

        m_PotentialRealZombies.Clear();
        m_TriagedPotentialRealZombies.Clear();
        m_NotRealZombies.Clear();

        Vector3 zombieHeightOffset = new Vector3(0.0f, 2.5f, 0.0f);

        NavFlow.Instance.UpdateListOfZombies(m_ZombieBoids);

        foreach (ZombieBoidInfo zombidBoid in m_ZombieBoids)
        {
            if (!zombidBoid.zombieDead && zombidBoid.realZombieObject == null)
            {
                //zombidBoid.acceleration += new Vector3(Random.Range(-zombieAccelerationRandomRange, zombieAccelerationRandomRange), 0.0f, Random.Range(-zombieAccelerationRandomRange, zombieAccelerationRandomRange));

                //if (zombidBoid.acceleration.magnitude > zombieMaxAcceleration)
                //{
                //    zombidBoid.acceleration = zombidBoid.acceleration.normalized * zombieMaxAcceleration;
                //}

                //zombidBoid.acceleration = (zombidBoid.destination - zombidBoid.position) * zombieMaxAcceleration;

                zombidBoid.velocity += zombidBoid.acceleration * Time.deltaTime;
                if (zombidBoid.velocity.magnitude > zombieMaxSpeed)
                {
                    zombidBoid.velocity = zombidBoid.velocity.normalized * zombieMaxSpeed;
                }

                zombidBoid.position += zombidBoid.velocity * Time.deltaTime;
                zombidBoid.rotation.SetLookRotation(zombidBoid.velocity);
            }

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
            else
            {
                if (zombidBoid.realZombieObject != null)
                {
                    ReturnPooledZombie(zombidBoid.realZombieObject);
                    zombidBoid.realZombieObject = null;
                }
            }
        }

        float cutOffDistance = 999999.9f;
        if (m_PotentialRealZombies.Count > RealZombieLimit)
        {
            foreach (ZombieBoidInfo potentialZombidBoid in m_PotentialRealZombies)
            {
                if (potentialZombidBoid.zombieDistanceFromPlayer > cutOffDistance)
                {
                    m_NotRealZombies.Add(potentialZombidBoid);
                    continue;
                }    

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
                        if (potentialZombidBoid.zombieDistanceFromPlayer < (triagedPotentialZombidBoid.zombieDistanceFromPlayer + zombieDistanceBuffer))
                        {
                            m_TriagedPotentialRealZombies.Insert(index, potentialZombidBoid);
                            zombieAdded = true;
                            break;
                        }
                        ++index;
                        if (index >= RealZombieLimit)
                            break;
                    }

                    if (!zombieAdded)
                    {
                        if (index < RealZombieLimit)
                        {
                            m_TriagedPotentialRealZombies.Add(potentialZombidBoid);
                        }
                        else
                        {
                            m_NotRealZombies.Add(potentialZombidBoid);
                        }
                    }
                }
               

                if (m_TriagedPotentialRealZombies.Count > RealZombieLimit)
                {
                    m_NotRealZombies.Add(m_TriagedPotentialRealZombies[RealZombieLimit]);
                    m_TriagedPotentialRealZombies.RemoveAt(RealZombieLimit);
                    cutOffDistance = m_TriagedPotentialRealZombies[RealZombieLimit - 1].zombieDistanceFromPlayer;
                }

            }

            foreach (ZombieBoidInfo triagedPotentialZombidBoid in m_TriagedPotentialRealZombies)
            {
                if (triagedPotentialZombidBoid.realZombieObject == null)
                {
                    triagedPotentialZombidBoid.realZombieObject = RequestPooledZombie(triagedPotentialZombidBoid.position);
                    if (triagedPotentialZombidBoid.realZombieObject)
                    {
                        triagedPotentialZombidBoid.realZombieObject.transform.position = triagedPotentialZombidBoid.position;
                    }
                }
                else
                {
                    //Debug.Log("Keeping my zombie!");
                    triagedPotentialZombidBoid.position = triagedPotentialZombidBoid.realZombieObject.transform.position;
                }
            }
        }
        else
        {
            foreach (ZombieBoidInfo potentialZombidBoid in m_PotentialRealZombies)
            {
                if (potentialZombidBoid.realZombieObject == null)
                {
                    potentialZombidBoid.realZombieObject = RequestPooledZombie(potentialZombidBoid.position);
                    if (potentialZombidBoid.realZombieObject)
                    {
                        potentialZombidBoid.realZombieObject.transform.position = potentialZombidBoid.position;
                    }
                }
                else
                {
                    //Debug.Log("Keeping my zombie!");
                    potentialZombidBoid.position = potentialZombidBoid.realZombieObject.transform.position;
                }
            }
        }

        foreach (ZombieBoidInfo zombidBoid in m_NotRealZombies)
        {
            if (zombidBoid.realZombieObject)
            {
                ReturnPooledZombie(zombidBoid.realZombieObject);
                zombidBoid.realZombieObject = null;
            }
        }

        Vector3 zombieHeight = new Vector3(0.0f, 2.0f, 0.0f);
        foreach (ZombieBoidInfo zombidBoid in m_ZombieBoids)
        {
            if (zombidBoid.realZombieObject == null)
            {
                Matrix4x4 m = Matrix4x4.TRS(zombidBoid.position + zombieHeight, zombidBoid.rotation, Vector3.one * 2.0f);
                Graphics.DrawMesh(meshToDraw, m, fakeZombieMaterial, 0);
            }
            else
            {
                //Matrix4x4 m = Matrix4x4.TRS(zombidBoid.position + zombieHeight, zombidBoid.rotation, Vector3.one * 1.0f);
                //Graphics.DrawMesh(meshToDraw, m, fakeSpawnedZombieMaterial, 0);
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
    public GameObject realZombieObject;
}

[CustomEditor(typeof(MeshRendererTest))]
public class MeshRendererTestEditor: Editor
{

    MeshRendererTest _meshRendererTest;

    private void Awake()
    {
        _meshRendererTest = (MeshRendererTest)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Spawn zombie"))
        {
            _meshRendererTest.m_PooledZombies.Add(GameObject.Instantiate(_meshRendererTest.zombie, _meshRendererTest.transform));
        }
    }
}