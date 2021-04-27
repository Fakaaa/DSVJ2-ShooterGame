using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] public Enemy prefabEnemy;
    [SerializeField] public int amountEnemyPerSpawn;

    [SerializeField] public float timeForRespawnN;
    private float timer;

    [SerializeField] public float maxDistanceX;
    [SerializeField] public float minDistanceX;
    [SerializeField] public float maxDistanceZ;
    [SerializeField] public float minDistanceZ;

    private Vector3 randomPosition;

    private AudioSource respawnSound;
    [SerializeField] private bool deactivateSpawn;
    void Start()
    {
        timer = 0;
        respawnSound = gameObject.GetComponent<AudioSource>();
    }
    void Update()
    {
        if(!deactivateSpawn)
        {
            if(timer <= timeForRespawnN)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                if(!respawnSound.isPlaying)
                    respawnSound.Play();
                randomPosition = new Vector3(Random.Range(minDistanceX,maxDistanceX), 0 , Random.Range(minDistanceZ, maxDistanceZ));
                Instantiate(prefabEnemy, randomPosition, Quaternion.identity);
            }
        }
    }
}
