using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

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
    [SerializeField] private float timeUntilMoveErractically;
    [SerializeField]
    [Range(18,120)] 
    private float minimumDistanceToChase;
    [SerializeField] private FirstPersonController playerPos;
    private Vector3 posToMoveErratically;
    private Vector3 posToChaseAndAttack;


    private float maxDistanceX = 0;
    private float minDistanceX = 0;
    private float maxDistanceZ = 0;
    private float minDistanceZ = 0;
    private float timerMoveErra;
    private bool randomPosCreated;
    private bool lookedThePlayer;
    private bool alreadyPlaceTheTarget;

    [Space(20)]

    [Header(" ENEMY GENERIC ")]
    [Space(10)]

    [SerializeField] private float damageEnemy;
    [SerializeField] public float pointsGived;
    private AudioSource mySound;
    [SerializeField] private LayerMask player;
    [SerializeField] private Player myRefPlayer;

    [Space(20)]
    [Header("OTHERS")]
    [SerializeField] public Terrain theTerrain;
    [SerializeField] public SpawnerManager myRefToSpawner;

    public void Start()
    {
        switch (myType)
        {
            case TypeEnemy.Ghost:
                posToMoveErratically = Vector3.zero;
                posToChaseAndAttack = Vector3.zero;
                maxDistanceX = 141;
                minDistanceX = 50;
                maxDistanceZ = 88;
                minDistanceZ = 21;
                pointsGived = 200;
                randomPosCreated = false;
                lookedThePlayer = false;
                alreadyPlaceTheTarget = false;
                playerPos = FindObjectOfType<FirstPersonController>();
                myRefPlayer = FindObjectOfType<Player>();
                alive = true;
                gameObject.tag = "Ghost";
                break;
            case TypeEnemy.Bomb:
                timerToBanish = 0;
                timerToExplode = 0;
                damageEnemy = 50;
                timeToBanish = 2;
                radiusDetectPlayer = 5;
                myRefPlayer = FindObjectOfType<Player>();
                explosiveBlow = false;
                timeToExplode = false;
                playerEscapes = false;
                explosionEffectAux = null;
                gameObject.tag = "Bomb";
                break;
        }
        myRefToSpawner = FindObjectOfType<SpawnerManager>();
        mySound = gameObject.GetComponent<AudioSource>();
    }

    public void Update()
    {
        CheckBehaivourTypeEnemy(myCurrentState, myType);
        CheckIfPlayerIsNear();
    }
    public void ApplyMoveErratically()
    {
        if (!randomPosCreated)
        {
            float randX = Random.Range(minDistanceX, maxDistanceX);
            float randZ = Random.Range(minDistanceZ, maxDistanceZ);
            posToMoveErratically = new Vector3(randX, 0, randZ);
            posToMoveErratically.y = gameObject.transform.localScale.y + theTerrain.SampleHeight(posToMoveErratically);
            randomPosCreated = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, posToMoveErratically, speedEnemy * Time.deltaTime);
        transform.LookAt(posToMoveErratically, Vector3.up);

        if (transform.position == posToMoveErratically)
        {
            timerMoveErra = 0;
            randomPosCreated = false;
        }
    }
    public void CheckIfPlayerIsNear()
    {
        if (playerPos != null)
        {
            if (Vector3.Distance(transform.position, playerPos.transform.position) <= minimumDistanceToChase && myCurrentState != States.GoBack)
            {
                myCurrentState = States.Attack;
                lookedThePlayer = true;
            }
            else if(Vector3.Distance(transform.position, playerPos.transform.position) >= minimumDistanceToChase && myCurrentState == States.GoBack)
            {
                myCurrentState = States.Idle;
                lookedThePlayer = false;
            }
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

                if(lookedThePlayer)
                {

                    if (transform.position == posToChaseAndAttack)
                    {
                        if(Vector3.Distance(transform.position, playerPos.transform.position) >= minimumDistanceToChase)
                        {
                            lookedThePlayer = false;
                            myCurrentState = States.Idle;
                        }
                        else
                        {
                            alreadyPlaceTheTarget = false;
                        }
                    }

                    if (!alreadyPlaceTheTarget)
                    {
                        float posX = playerPos.gameObject.transform.position.x - playerPos.gameObject.transform.localScale.x;
                        float posZ = playerPos.gameObject.transform.position.z - playerPos.gameObject.transform.localScale.z;

                        posToChaseAndAttack = new Vector3(posX, 0, posZ);
                        posToChaseAndAttack.y = gameObject.transform.localScale.y + theTerrain.SampleHeight(posToChaseAndAttack);

                        alreadyPlaceTheTarget = true;
                    }

                    transform.position = Vector3.MoveTowards(transform.position, posToChaseAndAttack, speedEnemy * Time.deltaTime);
                    transform.LookAt(posToChaseAndAttack, Vector3.up);

                    Debug.Log("Distancia enemigo player = " + Vector3.Distance(transform.position, playerPos.transform.position));

                    if (Vector3.Distance(transform.position, playerPos.transform.position) <= 1.5f)
                    {
                        alreadyPlaceTheTarget = false;
                        lookedThePlayer = false;
                        myCurrentState = States.GoBack;
                    }
                }
                else
                {
                    randomPosCreated = false;
                }

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

                Vector3 dir02 = transform.position - playerPos.transform.position;
                transform.Translate(dir02.normalized * speedEnemy * Time.deltaTime, Space.World);

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
            if (myRefToSpawner != null)
                myRefToSpawner.DecreaseInstanceBomb();
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

            if (myRefToSpawner != null)
                myRefToSpawner.DecreaseInstanceGhost();

            if (FindObjectOfType<Player>() != null && !alive)
                FindObjectOfType<Player>().SetPoints(200);

            Destroy(gameObject);
        }
    }
}
