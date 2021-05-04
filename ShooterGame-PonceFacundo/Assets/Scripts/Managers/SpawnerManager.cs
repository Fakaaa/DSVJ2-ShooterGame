using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] public EnemyFSM prefabEnemyBomb;
    [SerializeField] public EnemyFSM prefabGhost;
    [SerializeField] public Box prefabBoxes;
    [SerializeField] public Terrain theTerrain;

    [SerializeField] public float timeForRespawnBombs;
    [SerializeField] public float timeForRespawnGhost;
    [SerializeField] public float timeForRespawnBoxes;
    private float timerEnemysBomb;
    private float timerEnemysGhost;
    private float timerBoxes;

    [SerializeField] public float maxDistanceX;
    [SerializeField] public float minDistanceX;
    [SerializeField] public float maxDistanceZ;
    [SerializeField] public float minDistanceZ;

    [SerializeField] 
    [Tooltip("More than 35 and my pc explode xd")]
    [Range(0, 35)]
    public int maxGhostSpawned;
    public int actualQuantityGhosts;
    [SerializeField]
    [Tooltip("More than 35 and my pc explode xd")]
    [Range(0, 35)]
    public int maxBombsSpawned;
    public int actualQuantityBombs;
    [SerializeField]
    [Tooltip("More than 35 and my pc explode xd")]
    [Range(0, 35)]
    public int maxBoxesSpawned;
    public int actualQuantityBoxes;

    private Vector3 randomPosition;

    private AudioSource respawnSound;
    [SerializeField] private bool deactivateSpawn;
    void Start()
    {
        actualQuantityBoxes = 0;
        actualQuantityBombs = 0;
        actualQuantityGhosts = 0;
        timerEnemysBomb = 0;
        timerEnemysGhost = 0; 
        timerBoxes = 0;
        respawnSound = gameObject.GetComponent<AudioSource>();
    }
    void LateUpdate()
    {
        if(!deactivateSpawn)
        {
            timerEnemysGhost += Time.deltaTime;
            timerEnemysBomb += Time.deltaTime;
            timerBoxes += Time.deltaTime;

            RespawnSomething(prefabGhost.gameObject, ref timerEnemysGhost, timeForRespawnGhost, ref actualQuantityGhosts, maxGhostSpawned);
            RespawnSomething(prefabEnemyBomb.gameObject, ref timerEnemysBomb, timeForRespawnBombs, ref actualQuantityBombs, maxBombsSpawned);
            RespawnSomething(prefabBoxes.gameObject, ref timerBoxes, timeForRespawnBoxes, ref actualQuantityBoxes, maxBoxesSpawned);
        }
    }
    public void RespawnSomething(GameObject objectToSpawn, ref float actualTime, float timeToSpawn ,ref int actualNumber, int limitSpawn)
    {
        if (actualTime >= timeToSpawn)   
        {
            actualTime = 0;

            int randPosX = Random.Range((int)minDistanceX, (int)maxDistanceX);
            int randPosZ = Random.Range((int)minDistanceZ, (int)maxDistanceZ);

            randomPosition = new Vector3(randPosX , 0, randPosZ);
            randomPosition.y = objectToSpawn.gameObject.transform.localScale.y + theTerrain.SampleHeight(randomPosition);
            
            if(actualNumber < limitSpawn)
            {
                respawnSound.Play();
                Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
                actualNumber++;
            }
        }
    }
    public void DecreaseInstanceGhost()
    {
        actualQuantityGhosts -= 1;
    }
    public void DecreaseInstanceBomb()
    {
        actualQuantityBombs -= 1;
    }
    public void DecreaseInstanceBox()
    {
        actualQuantityBoxes -= 1;
    }
}
