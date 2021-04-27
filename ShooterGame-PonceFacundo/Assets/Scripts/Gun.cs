using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] private ParticleSystem prefabBulletImpact;
    [SerializeField] private Text ammo;
    [SerializeField] public Camera viewPlayer;
    [SerializeField] private float maxRange;

    public int clipSize;
    public int actualMagazine;
    public int maxAmmoToCarry;
    private Ray myRayDestiny;

    private AudioSource revolverShoot;
    private void Start()
    {
        actualMagazine = clipSize;
        revolverShoot = gameObject.GetComponent<AudioSource>();
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

    public void Shoot()
    {
        if (actualMagazine > 0 && !revolverShoot.isPlaying)
        {
            Vector3 mousePos = Input.mousePosition;
            myRayDestiny = viewPlayer.ScreenPointToRay(mousePos);
            Debug.DrawRay(myRayDestiny.origin, myRayDestiny.direction * maxRange, Color.red);

            if (Input.GetButtonDown("Fire1"))
            {
                if(!revolverShoot.isPlaying)
                    revolverShoot.Play();
                RaycastHit myHit; 
                if(Physics.Raycast(myRayDestiny.origin, myRayDestiny.direction * maxRange, out myHit))
                {
                    Instantiate(prefabBulletImpact, myHit.point, Quaternion.LookRotation(myHit.normal));
                    actualMagazine--;
                }
            }
        }
    }
    public void ReloadGun()
    {
        actualMagazine = clipSize;
    }

    public void ShowAmmo()
    {
        ammo.text = actualMagazine.ToString() + "/" + clipSize.ToString();
    }
}
