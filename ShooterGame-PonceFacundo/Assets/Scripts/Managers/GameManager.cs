using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Get() { return instance; }

    [SerializeField] private int pointsPlayer;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Player.playerDead += EndGame;
    }
    void EndGame()
    {
        Player.playerDead -= EndGame;
        if (SceneLoader.Get() != null)
            SceneLoader.Get().LoadScene("EndScene");
    }
    private void OnDisable()
    {
        Player.playerDead -= EndGame;
    }
    public void SetPointsPlayer(int points)
    {
        pointsPlayer = points;
    }
    public int GetPointsPlayer() { return pointsPlayer; }
}
