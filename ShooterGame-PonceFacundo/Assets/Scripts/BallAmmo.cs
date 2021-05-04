using UnityEngine;

public class BallAmmo : MonoBehaviour
{
    [SerializeField] public float myDamage;
    [SerializeField] public float timeUntilIBanish;
    [SerializeField] public ParticleSystem explosionEffect;
    [SerializeField] public AudioSource explosionSfx;

    private ParticleSystem garbagePartycle;
    private float timer;
    private void Start()
    {
        garbagePartycle = null;
        explosionSfx = gameObject.GetComponent<AudioSource>();
    }
    void Update()
    {
        CleanBall();
    }
    public void CleanBall()
    {
        if (timer <= timeUntilIBanish)
            timer += Time.deltaTime;
        else
        {
            if(garbagePartycle != null)
                garbagePartycle.transform.parent = transform;
            Destroy(gameObject); 
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ghost")
        {
            garbagePartycle = Instantiate(explosionEffect, collision.gameObject.transform.position, Quaternion.identity);
            if(explosionSfx != null)
                explosionSfx.Play();
            collision.gameObject.GetComponent<EnemyFSM>().DamageGhost(myDamage);
        }
        else if(collision.collider.tag == "Bomb")
        {
            garbagePartycle = Instantiate(explosionEffect, collision.gameObject.transform.position, Quaternion.identity);
            if(explosionSfx != null)
                explosionSfx.Play();
            collision.gameObject.GetComponent<EnemyFSM>().CreateExplosion();
        }
    }
}
