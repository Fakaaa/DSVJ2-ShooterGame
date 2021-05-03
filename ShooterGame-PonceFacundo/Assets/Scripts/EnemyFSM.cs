using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    public enum TypeEnemy
    {
        Ghost,
        Bomb
    }
    public TypeEnemy myType;

    [SerializeField]
    public enum States
    {
        Idle,
        Attack,
        GoBack
    }
    public States myCurrentState;

    [Header("EnemyType: -BOMB-")]

    [SerializeField][Tooltip("Only if type enemy is: BOMB")]
    public float timeUntilExplode;
    private float timerToExplode = 0;
    private float timerToBanish = 0;
    [SerializeField] public bool explosiveBlow;
    [SerializeField] public bool timeToExplode;
    private bool playerEscapes;
    [SerializeField] private float timeToBanish;
    [SerializeField] public float radiusDetectPlayer;
    [SerializeField] public ParticleSystem prefabExplosionSfx;
    private ParticleSystem explosionEffectAux;

    [Space(20)]

    [Header("EnemyType: -GHOST-")]

    [SerializeField]
    [Range(5,20)]
    [Tooltip("Only if type enemy is: GHOST")]
    private float speedEnemy;

    [Header(" Enemy Generic ")]

    [SerializeField] private float damageEnemy;
    [SerializeField] public float pointsGived;
    [SerializeField] private Rigidbody myBody;
    private AudioSource mySound;
    [SerializeField] private LayerMask player;
    private Player myRefPlayer;

    public void Start()
    {
        myRefPlayer = FindObjectOfType<Player>();

        switch (myType)
        {
            case TypeEnemy.Ghost:
                speedEnemy = 5;
                damageEnemy = 20;
                pointsGived = 200;
                gameObject.tag = "Ghost";
                break;
            case TypeEnemy.Bomb:
                timerToBanish = 0;
                timerToExplode = 0;
                damageEnemy = 50;
                timeToBanish = 2;
                radiusDetectPlayer = 5;
                explosiveBlow = false;
                timeToExplode = false;
                playerEscapes = false;
                explosionEffectAux = null;
                gameObject.tag = "Bomb";
                break;
        }
        mySound = gameObject.GetComponent<AudioSource>();
    }

    public void Update()
    {
        CheckBehaivourTypeEnemy(myCurrentState, myType);
    }

    public void ApplyBehaviourIdle(TypeEnemy whatType)
    {
        switch (whatType)
        {
            case TypeEnemy.Ghost:
                break;
            case TypeEnemy.Bomb:
                if (Physics.CheckSphere(transform.position, radiusDetectPlayer, player) && !timeToExplode)
                        myCurrentState = States.Attack;
                break;
        }
    }
    public void ApplyBehaviourAttack(TypeEnemy whatType)
    {
        switch (whatType)   
        {
            case TypeEnemy.Ghost:
                break;
            case TypeEnemy.Bomb:              
                if (Physics.CheckSphere(transform.position, radiusDetectPlayer, player))
                {
                    timeToExplode = true;
                    playerEscapes = false;
                }
                else
                {
                    playerEscapes = true;
                }
                if (timeToExplode)
                {
                    if (timerToExplode <= timeUntilExplode)
                        timerToExplode += Time.deltaTime;
                    else
                    {
                        CreateExplosion();
                        if (!playerEscapes)
                            AttackPlayer();
                    }
                }
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, radiusDetectPlayer);
    }
    public void ApplyBehaviourGoBack(TypeEnemy whatType)
    {
        switch (whatType)
        {
            case TypeEnemy.Ghost:
                break;
            case TypeEnemy.Bomb:
                CleanBomb();
                break;
        }
    }
    public void CheckBehaivourTypeEnemy(States whatState, TypeEnemy whatType)
    {
        switch (whatState)
        {
            case States.Idle:
                    ApplyBehaviourIdle(whatType);
                break;
            case States.Attack:
                    ApplyBehaviourAttack(whatType);
                break;
            case States.GoBack:
                    ApplyBehaviourGoBack(whatType);
                break;
        }
    }
    public void AttackPlayer()
    {
        if (myRefPlayer != null)
            myRefPlayer.ReciveDamage(damageEnemy);
    }
    public void CreateExplosion()
    {
        explosionEffectAux = Instantiate(prefabExplosionSfx, gameObject.transform.position, Quaternion.identity);
        explosionEffectAux.Play();
        myBody.AddExplosionForce(20, transform.position, 15, 4, ForceMode.Impulse);
        mySound.Play();
        explosiveBlow = true;
        myCurrentState = States.GoBack;
    }
    public void CleanBomb()
    {
        if (explosiveBlow)
            timerToBanish += Time.deltaTime;

        if (timerToBanish >= timeToBanish)
        {
            explosionEffectAux.transform.parent = gameObject.transform;
            Destroy(gameObject);
            timerToBanish = 0;
        }
    }
}
