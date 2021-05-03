using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] public EnemyFSM prefabEnemyBomb;
    [SerializeField] public Box prefabBoxes;
    [SerializeField] public Terrain theTerrain;

    [SerializeField] public float timeForRespawnEnemies;
    [SerializeField] public float timeForRespawnBoxes;
    private float timerEnemys;
    private float timerBoxes;

    [SerializeField] public float maxDistanceX;
    [SerializeField] public float minDistanceX;
    [SerializeField] public float maxDistanceZ;
    [SerializeField] public float minDistanceZ;

    private Vector3 randomPosition;

    private AudioSource respawnSound;
    [SerializeField] private bool deactivateSpawn;
    void Start()
    {
        timerEnemys = 0;
        timerBoxes = 0;
        respawnSound = gameObject.GetComponent<AudioSource>();
    }
    void LateUpdate()
    {
        if(!deactivateSpawn)
        {
            timerEnemys += Time.deltaTime;
            timerBoxes += Time.deltaTime;

            RespawnSomething(prefabEnemyBomb.gameObject, ref timerEnemys, timeForRespawnEnemies);
            RespawnSomething(prefabBoxes.gameObject, ref timerBoxes, timeForRespawnBoxes);
        }
    }
    public void RespawnSomething(GameObject objectToSpawn, ref float actualTime, float timeToSpawn )
    {
        if (actualTime >= timeToSpawn)   
        {
            actualTime = 0;
            respawnSound.Play();

            int randPosX = Random.Range((int)minDistanceX, (int)maxDistanceX);
            int randPosZ = Random.Range((int)minDistanceZ, (int)maxDistanceZ);

            randomPosition = new Vector3(randPosX , 0, randPosZ);
            randomPosition.y = objectToSpawn.gameObject.transform.localScale.y + theTerrain.SampleHeight(randomPosition);

            Debug.Log("xd " + theTerrain.terrainData.GetHeight(randPosX, randPosZ));
            Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        }
    }
}
