using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float pointsGived;
    [SerializeField] public float damageToPlayer;

    [SerializeField] public ParticleSystem explosionEffec;

    [SerializeField] public bool explosiveBlow;
    [SerializeField] public bool alive;
    [SerializeField] private float timeToBanish;

    [SerializeField] private Rigidbody myBody;
    private float timer;
    private AudioSource explosionSound;
    private ParticleSystem explosionEffectAux;
    void Start()
    {
        timer = 0;
        explosiveBlow = false;
        alive = true;

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
            if(FindObjectOfType<Player>() != null)
                FindObjectOfType<Player>().ReciveDamage(50);
        }
    }
    public void CreateExplosion()
    {
        explosionEffectAux = Instantiate(explosionEffec, gameObject.transform.position, Quaternion.identity);
        explosionEffectAux.Play();
        myBody.AddExplosionForce(20, transform.position, 15, 4, ForceMode.Impulse);
        explosionSound.Play();
        alive = false;
        explosiveBlow = true;
    }
}
