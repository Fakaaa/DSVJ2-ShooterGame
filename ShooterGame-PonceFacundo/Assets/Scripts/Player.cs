using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] public float hp;
    public static int points;
    public static Action playerDead;

    public delegate void HandleUI(float points, float hp);
    public static HandleUI updateData;

    [SerializeField] public Gun.TypeGun myCurrentGun;

    [SerializeField] public Gun [] myGuns;

    public void Start()
    {
        hp = 100;
        points = 0;
        PassPlayerInfo();
        if (GameManager.Get() != null)
            GameManager.Get().SetPointsPlayer(points);
    }
    public void SetPoints(int amount)
    {
        points += amount;
        PassPlayerInfo();
        if (GameManager.Get() != null)
            GameManager.Get().SetPointsPlayer(points);
    }
    public void ReciveDamage(float dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            hp = 0;
            PassPlayerInfo();

            if (playerDead != null)
                playerDead();
        }
        else
            PassPlayerInfo();
    }
    private void Update()
    {
        CheckGun();
    }
    public void PassPlayerInfo()
    {
        updateData?.Invoke(points, hp);
    }
    public void ChooseGun()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            myCurrentGun = Gun.TypeGun.Pistol;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            myCurrentGun = Gun.TypeGun.BallGun;
        }
    }
    public void CheckGun()
    {
        ChooseGun();

        if (myGuns[0] != null && myGuns[1] != null)
        {
            switch (myCurrentGun)
            {
                case Gun.TypeGun.Pistol:
                    myGuns[0].gameObject.SetActive(true);
                    myGuns[1].gameObject.SetActive(false);
                    break;
                case Gun.TypeGun.BallGun:
                    myGuns[1].gameObject.SetActive(true);
                    myGuns[0].gameObject.SetActive(false);
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ghost")
        {
            ReciveDamage(10);
        }
    }
}
