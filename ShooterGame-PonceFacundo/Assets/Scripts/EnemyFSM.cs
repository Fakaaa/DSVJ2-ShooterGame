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

    [Space(20)]
    [Header(" BOMB ")]
    [Space(10)]

    [SerializeField]
    [Tooltip("Only if type enemy is: BOMB")]
    public float timeUntilExplode;
    private float timerToExplode = 0;
    private ParticleSystem explosionEffectAux;
    private bool playerEscapes;
    private float timerToBanish = 0;
    [SerializeField] public bool explosiveBlow;
    [SerializeField] public bool timeToExplode;
    [SerializeField] private float timeToBanish;
    [SerializeField] public float radiusDetectPlayer;
    [SerializeField] public ParticleSystem prefabExplosionSfx;
    [SerializeField] private Rigidbody myBody;

    [Space(20)]

    [Header(" GHOST ")]
    [Space(10)]

    [SerializeField]
    [Range(5, 20)]
    [Tooltip("Only if type enemy is: GHOST")]
    private float speedEnemy;
    [SerializeField] 
    [Range(0, 300)] 
    public float hp_Ghost;
    [SerializeField] public bool alive;
    [SerializeField] private Vector3 posToMove;
    [SerializeField] private float timeUntilMoveErractically;
    [SerializeField]
    [Range(18,120)] 
    private float minimumDistanceToChase;
    private Vector3 myPos;
    private Vector3 playerPos;

    private float maxDistanceX = 0;
    private float minDistanceX = 0;
    private float maxDistanceZ = 0;
    private float minDistanceZ = 0;
    private float timerMoveErra;
    private bool randomPosCreated;

    [Header(" ENEMY GENERIC ")]

    [SerializeField] private float damageEnemy;
    [SerializeField] public float pointsGived;
    private AudioSource mySound;
    [SerializeField] private LayerMask player;
    private Player myRefPlayer;

    [Header("OTHERS")]
    [SerializeField] public Terrain theTerrain;

    public void Start()
    {
        myRefPlayer = FindObjectOfType<Player>();

        switch (myType)
        {
            case TypeEnemy.Ghost:
                posToMove = transform.position;
                myPos = transform.position;
                playerPos = myRefPlayer.transform.position;
                maxDistanceX = 141;
                minDistanceX = 50;
                maxDistanceZ = 88;
                minDistanceZ = 21;
                pointsGived = 200;
                randomPosCreated = false;
                alive = true;
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
        myPos = transform.position;
        playerPos = myRefPlayer.transform.position;

        CheckBehaivourTypeEnemy(myCurrentState, myType);
    }

    public void ApplyMoveErratically()
    {
        if (!randomPosCreated)
        {
            float randX = Random.Range(minDistanceX, maxDistanceX);
            float randZ = Random.Range(minDistanceZ, maxDistanceZ);
            posToMove = new Vector3(randX, 0, randZ);
            posToMove.y = gameObject.transform.localScale.y + theTerrain.SampleHeight(posToMove);
            randomPosCreated = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, posToMove, speedEnemy * Time.deltaTime);

        if (transform.position == posToMove)
        {
            timerMoveErra = 0;
            randomPosCreated = false;
        }
    }
    public void CheckIfPlayerIsNear()
    {
        float myCalc = Mathf.Sqrt(Mathf.Pow((myPos.x - playerPos.x), 2) + Mathf.Pow((myPos.y - playerPos.y), 2));

        Debug.Log("Resultado en distancia= " + myCalc);

        if (myCalc <= minimumDistanceToChase)
        {
            myCurrentState = States.Attack;
        }
    }
    public void ApplyBehaviourIdle(TypeEnemy whatType)
    {
        switch (whatType)
        {
            case TypeEnemy.Ghost:
                if (timerMoveErra <= timeUntilMoveErractically)
                    timerMoveErra += Time.deltaTime;
                else if (timerMoveErra >= timeUntilMoveErractically)
                {
                    ApplyMoveErratically();
                }
                CheckIfPlayerIsNear();
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, radiusDetectPlayer);
    }
    public void DamageGhost(float dmg)
    {
        hp_Ghost -= dmg;

        if (hp_Ghost <= 0)
        {
            hp_Ghost = 0;
            alive = false;

            if (FindObjectOfType<Player>() != null && !alive)
                FindObjectOfType<Player>().SetPoints(200);

            Destroy(gameObject);
        }
    }
}
