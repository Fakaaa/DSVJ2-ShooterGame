using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp;
    public int points;

    private static Player instance;

    public static Player Get() { return instance; }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPoints(int amount)
    {
        points += amount;
    }
    public void ReciveDamage(float dmg)
    {
        if (hp > 0)
            hp -= dmg;
        else
            hp = 0;
    }
    public bool CheckAlive()
    {
        if (hp > 0)
            return true;
        else
            return false;
    }
}
