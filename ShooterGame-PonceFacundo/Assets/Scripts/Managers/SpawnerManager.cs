using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] public Enemy prefabEnemy;
    [SerializeField] public Box prefabBoxes;

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
    void Update()
    {
        if(!deactivateSpawn)
        {
            timerEnemys += Time.deltaTime;
            timerBoxes += Time.deltaTime;

            RespawnSomething(prefabEnemy.gameObject, ref timerEnemys, timeForRespawnEnemies);
            RespawnSomething(prefabBoxes.gameObject, ref timerBoxes, timeForRespawnBoxes);
        }
    }
    public void RespawnSomething(GameObject objectToSpawn, ref float actualTime, float timeToSpawn )
    {
        if (actualTime >= timeToSpawn)   
        {
            actualTime = 0;
            respawnSound.Play();
            randomPosition = new Vector3(Random.Range(minDistanceX, maxDistanceX), objectToSpawn.transform.position.y, Random.Range(minDistanceZ, maxDistanceZ));
            Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        }
    }
}
