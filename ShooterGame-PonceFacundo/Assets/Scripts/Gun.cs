using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Gun : MonoBehaviour
{
    [SerializeField]
    public enum TypeGun
    {
        Pistol,
        BallGun
    }
    public TypeGun myType;

    [SerializeField] private ParticleSystem prefabBulletImpact;
    [SerializeField] private ParticleSystem muzzleFlash;

    public delegate void ShowUIGun(string ammo, int typeShoot);
    public static ShowUIGun myUI;

    [SerializeField] public Camera viewPlayer;
    [SerializeField] private float maxRange;

    [SerializeField] public float verticalRecoil;
    [SerializeField] public float horizontalRecoil;
    [SerializeField] FirstPersonController player;

    public int clipSize;
    public int actualMagazine;
    public int maxAmmoToCarry;
    private Ray myRayDestiny;

    [SerializeField]private AudioSource revolverShoot;
    [SerializeField]private AudioSource reload;

    [SerializeField] public float fireRate;
    [SerializeField][Range(0.2f, 10.0f)] public float DMG_PerBullet;

    [SerializeField] public float forceBallThrow;
    [SerializeField] public float upwardModifier;
    [SerializeField] public float radiusExplosion;
    [SerializeField] public BallAmmo prefabBallAmmo;
    [SerializeField] public Transform cannonRocketBall;

    public enum TypeShoot
    {
        SingleShoot,
        Automatic
    }
    [SerializeField] public TypeShoot myActualTypeShoot;

    private float shootTimer;
    private bool alreadyShoot;
    private bool selectionType;
    private void Start()
    {
        shootTimer = 0;
        alreadyShoot = false;
        selectionType = false;
        actualMagazine = clipSize;
        UpdateUI();
    }
    private void Update()
    {
        Shoot();
        
        ReloadGun();

        ChangeShootType();
    }
    public void CalcFireRate()
    {
        if(shootTimer < fireRate && alreadyShoot)
            shootTimer += Time.deltaTime;
        if (shootTimer >= fireRate)
        {
            shootTimer = 0;
            alreadyShoot = false;
        }
    }
    public bool GetTypeShoot()
    {
        bool myActualInput = false;

        switch (myActualTypeShoot)
        {
            case TypeShoot.SingleShoot:
                myActualInput = Input.GetButtonDown("Fire1");
                break;
            case TypeShoot.Automatic:
                myActualInput = Input.GetButton("Fire1");
                break;
        }

        return myActualInput;
    }
    public void Shoot()
    {
        CalcFireRate();

        if (actualMagazine > 0 && !alreadyShoot)
        {
            if (myType == TypeGun.Pistol)
                PistolType();
            else
                BallCannon();
        }
    }
    public void ReloadGun()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reload.Play();

            actualMagazine = clipSize;

            UpdateUI();
        }
    }
    public void UpdateUI()
    {
        myUI?.Invoke((actualMagazine.ToString() + "/" + clipSize.ToString()), (int)myActualTypeShoot);
    }
    public void ChangeShootType()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            selectionType = !selectionType;

            if (!selectionType)
                myActualTypeShoot = TypeShoot.Automatic;
            else
                myActualTypeShoot = TypeShoot.SingleShoot;

            UpdateUI();
        }
    }
    public void PistolType()
    {
        UpdateUI();

        Vector3 mousePos = Input.mousePosition;
        myRayDestiny = viewPlayer.ScreenPointToRay(mousePos);
        Debug.DrawRay(myRayDestiny.origin, myRayDestiny.direction * maxRange, Color.red);

        if (GetTypeShoot())
        {
            alreadyShoot = true;

            revolverShoot.Play();

            player.m_MouseLook.AddRecoil(Random.Range(-horizontalRecoil, horizontalRecoil), verticalRecoil);

            muzzleFlash.Play();

            RaycastHit myHit;

            if (Physics.Raycast(myRayDestiny.origin, myRayDestiny.direction * maxRange, out myHit))
            {
                Instantiate(prefabBulletImpact, myHit.point, Quaternion.LookRotation(myHit.normal));

                if (myHit.collider.tag == "Bomb")
                {
                    if (FindObjectOfType<Player>() != null)
                        FindObjectOfType<Player>().SetPoints(100);
                    if(myHit.rigidbody != null)
                        myHit.rigidbody.AddExplosionForce(20, transform.position, 15, 4, ForceMode.Impulse);
                    if(myHit.collider != null)
                        myHit.collider.gameObject.GetComponent<EnemyFSM>().CreateExplosion();
                }
                if (myHit.collider.tag == "Ghost")
                {
                    if(myHit.collider != null)
                        myHit.collider.gameObject.GetComponent<EnemyFSM>().DamageGhost(DMG_PerBullet);
                }

                actualMagazine--;
            }
            UpdateUI();
        }
    }
    public void BallCannon()
    {
        UpdateUI();

        Vector3 mousePos = Input.mousePosition;
        myRayDestiny = viewPlayer.ScreenPointToRay(mousePos);
        Debug.DrawRay(myRayDestiny.origin, myRayDestiny.direction * maxRange, Color.magenta);

        if(Input.GetButtonDown("Fire1"))
        {
            alreadyShoot = true;

            revolverShoot.Play();

            player.m_MouseLook.AddRecoil(Random.Range(-horizontalRecoil, horizontalRecoil), verticalRecoil);
            
            muzzleFlash.Play();

            RaycastHit myHit;
            if (Physics.Raycast(myRayDestiny.origin, myRayDestiny.direction * maxRange, out myHit))
            {
                BallAmmo go = Instantiate(prefabBallAmmo, cannonRocketBall.position, cannonRocketBall.rotation);
                if (go != null)
                    go.GetComponent<Rigidbody>().AddExplosionForce(forceBallThrow, 
                        (cannonRocketBall.position - (cannonRocketBall.transform.forward * 3)), radiusExplosion, upwardModifier, ForceMode.Impulse);

                actualMagazine--;
            }
            UpdateUI();
        }
    }
}
