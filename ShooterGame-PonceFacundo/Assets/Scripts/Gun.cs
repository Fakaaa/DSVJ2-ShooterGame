using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public List<GameObject> bulletsShooted;
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] public Camera viewPlayer;
    [SerializeField] private float maxRange;

    public int clipSize;
    public int actualMagazine;
    public int maxAmmoToCarry;

    private void Start()
    {
        bulletsShooted.Clear();
        actualMagazine = clipSize;
    }

    private void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (actualMagazine > 0)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray myRay = viewPlayer.ScreenPointToRay(mousePos);
            Debug.DrawRay(myRay.origin, myRay.direction * maxRange, Color.red);

            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit myHit; 
                if(Physics.Raycast(myRay.origin, myRay.direction * maxRange, out myHit))
                {
                    //bulletsShooted.Add();
                    actualMagazine--;
                }
            }
        }
    }
}
