using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float pointsGived;
    [SerializeField] public float damageToPlayer;

    [SerializeField] public ParticleSystem explosionEffec;

    [SerializeField] public bool explosiveBlow;
    [SerializeField] private float timeToBanish;

    [SerializeField] private Rigidbody myBody;
    private float timer;
    private AudioSource explosionSound;
    private ParticleSystem explosionEffectAux;
    void Start()
    {
        timer = 0;
        explosiveBlow = false;
        explosionSound = gameObject.GetComponent<AudioSource>();
        explosionEffectAux = null;
    }
    void Update()
    {
        if (explosiveBlow)
            timer += Time.deltaTime;

        if(timer >= timeToBanish)
        {
            explosionEffectAux.transform.parent = gameObject.transform;
            Destroy(gameObject);
            timer = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            CreateExplosion();
            if(Player.Get() != null)
                Player.Get().ReciveDamage(50);
        }
    }
    public void CreateExplosion()
    {
        explosionEffectAux = Instantiate(explosionEffec, gameObject.transform.position, Quaternion.identity);
        myBody.AddExplosionForce(20, transform.position, 15, 4, ForceMode.Impulse);
        if (!explosionSound.isPlaying)
            explosionSound.Play();
        explosiveBlow = true;
    }
}
