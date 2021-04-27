using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] public List<Enemy> enemys;

    [SerializeField] public float timeForRespawnN;
    void Start()
    {
        enemys.Clear();
    }
    void Update()
    {
        
    }
}
