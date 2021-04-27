using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float pointsGived;
    [SerializeField] public float damageToPlayer;

    [SerializeField] public ParticleSystem explosionEffec;

    [SerializeField] private bool explosiveBlow;
    [SerializeField] private float timeToBanish;
    private float timer;
    void Start()
    {
        timer = 0;
        explosiveBlow = false;
    }

    void Update()
    {
        if (explosiveBlow)
            timer += Time.deltaTime;

        if(timer >= timeToBanish)
        {
            Destroy(gameObject);
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ParticleSystem explosion = Instantiate(explosionEffec, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            explosiveBlow = true;
        }
    }
}
