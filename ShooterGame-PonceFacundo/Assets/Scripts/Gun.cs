using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] private ParticleSystem prefabBulletImpact;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Text ammo;
    [SerializeField] public Camera viewPlayer;
    [SerializeField] private float maxRange;

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
    private void Start()
    {
        shootTimer = 0;
        alreadyShoot = false;
        actualMagazine = clipSize;
    }

    private void Update()
    {
        ShowAmmo();
        
        Shoot();

        if(Input.GetKeyDown(KeyCode.R))
        {
            ReloadGun();
        }    
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
            myRayDestiny = viewPlayer.ScreenPointToRay(mousePos);
            Debug.DrawRay(myRayDestiny.origin, myRayDestiny.direction * maxRange, Color.red);

            if (GetTypeShoot())
            {
                alreadyShoot = true;

                revolverShoot.Play();

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
        reload.Play();

        actualMagazine = clipSize;
    }
    public void ShowAmmo()
    {
        ammo.text = actualMagazine.ToString() + "/" + clipSize.ToString();
    }
}
