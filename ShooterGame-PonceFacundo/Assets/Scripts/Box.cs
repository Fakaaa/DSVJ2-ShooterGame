using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] public float pointsGived;
    [SerializeField] private AudioSource takeBox;
    [SerializeField] private SpawnerManager myRefToSpawner;
    private void Start()
    {
        myRefToSpawner = FindObjectOfType<SpawnerManager>();    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            takeBox.Play();

            if (myRefToSpawner != null)
                myRefToSpawner.DecreaseInstanceBox();

            if (FindObjectOfType<Player>() != null)
                FindObjectOfType<Player>().SetPoints(200);
            
            Destroy(gameObject);
        }
    }
}
