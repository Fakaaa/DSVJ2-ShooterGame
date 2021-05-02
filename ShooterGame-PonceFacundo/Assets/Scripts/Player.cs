using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] public float hp;
    public static int points;
    public static Action playerDead;

    [SerializeField] public Text pointsText;
    [SerializeField] public Text hpText;

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
        ShowPlayerUI();
    }
    public void ShowPlayerUI()
    {
        pointsText.text = "Points: " + points;
        hpText.text = "HP:" + hp;
    }
}
