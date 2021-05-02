using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] private ParticleSystem prefabBulletImpact;
    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private Text ammo;
    [SerializeField] private Text typeShoot;

    [SerializeField] public Camera viewPlayer;
    [SerializeField] private float maxRange;

    [SerializeField] float verticalRecoil;
    [SerializeField] float horizontalRecoil;

    public int clipSize;
    public int actualMagazine;
    public int maxAmmoToCarry;
    private Ray myRayDestiny;

    [SerializeField]private AudioSource revolverShoot;
    [SerializeField]private AudioSource reload;

    [SerializeField] public float fireRate;

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
    }
    private void Update()
    {
        ShowGunUI();
        
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
            Vector3 mousePos = Input.mousePosition;
            mousePos.x += horizontalRecoil;
            mousePos.y += verticalRecoil;
            myRayDestiny = viewPlayer.ScreenPointToRay(mousePos);
            Debug.DrawRay(myRayDestiny.origin, myRayDestiny.direction * maxRange, Color.red);

            horizontalRecoil = 0;
            verticalRecoil = 0;

            if (GetTypeShoot())
            {
                alreadyShoot = true;

                revolverShoot.Play();

                AddRecoil(Random.Range(-45, 45), Random.Range(-30,30));

                muzzleFlash.Play();

                RaycastHit myHit; 
                
                if(Physics.Raycast(myRayDestiny.origin, myRayDestiny.direction * maxRange, out myHit))
                {
                    Instantiate(prefabBulletImpact, myHit.point, Quaternion.LookRotation(myHit.normal));
                    
                    if(myHit.collider.tag == "Enemy")
                    {
                        if (FindObjectOfType<Player>() != null)
                            FindObjectOfType<Player>().SetPoints(100);
                        myHit.rigidbody.AddExplosionForce(20, transform.position, 15, 4, ForceMode.Impulse);
                        myHit.collider.gameObject.GetComponent<Enemy>().CreateExplosion();
                    }

                    actualMagazine--;
                }
            }
        }
    }
    public void ReloadGun()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reload.Play();

            actualMagazine = clipSize;
        }
    }
    public void ShowGunUI()
    {
        ammo.text = actualMagazine.ToString() + "/" + clipSize.ToString();

        switch (myActualTypeShoot)
        {
            case TypeShoot.SingleShoot:
                typeShoot.text = "FireMode - SingleShoot";
                break;
            case TypeShoot.Automatic:
                typeShoot.text = "FireMode - Automatic";
                break;
        }
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
        }
    }
    public void AddRecoil(float horizontal, float vertical)
    {
        horizontalRecoil += horizontal;
        verticalRecoil += vertical;
    }
}
