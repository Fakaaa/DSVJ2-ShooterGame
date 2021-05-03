using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] public float hp;
    public static int points;
    public static Action playerDead;

    public delegate void HandleUI(float points, float hp);
    public static HandleUI uiShow;

    public void Start()
    {
        hp = 100;
        points = 0;
        if (GameManager.Get() != null)
            GameManager.Get().SetPointsPlayer(points);
    }
    public void SetPoints(int amount)
    {
        points += amount;
        if (GameManager.Get() != null)
            GameManager.Get().SetPointsPlayer(points);
    }
    public void ReciveDamage(float dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            hp = 0;
            
            if(playerDead != null)
                playerDead();
        }
    }
    private void Update()
    {
        PassPlayerInfo();
    }
    public void PassPlayerInfo()
    {
        uiShow?.Invoke(points, hp);
    }
}
