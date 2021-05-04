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

    private Vector3 randomPosition;

    private AudioSource respawnSound;
    [SerializeField] private bool deactivateSpawn;
    void Start()
    {
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

            RespawnSomething(prefabGhost.gameObject, ref timerEnemysGhost, timeForRespawnGhost);
            RespawnSomething(prefabEnemyBomb.gameObject, ref timerEnemysBomb, timeForRespawnBombs);
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

            Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        }
    }
}
