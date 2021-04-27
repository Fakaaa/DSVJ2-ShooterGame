using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Get() { return instance; }

    [SerializeField] private UnityEvent endGame;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if(Player.Get() != null)
        {
            if(!Player.Get().CheckAlive())
            {
                endGame?.Invoke();  //Gracias a theo y la ludom dare :3
            }
        }
    }
}
