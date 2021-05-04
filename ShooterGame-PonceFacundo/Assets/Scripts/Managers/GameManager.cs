using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Get() { return instance; }

    [SerializeField] private int pointsPlayer;
    [SerializeField] private int highScorePlayer;
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
        highScorePlayer = PlayerPrefs.GetInt("HighScore", 0);
    }
    void EndGame()
    {
        if (highScorePlayer < pointsPlayer || highScorePlayer <= 0)
            highScorePlayer = pointsPlayer;

        PlayerPrefs.SetInt("HighScore", highScorePlayer);
        PlayerPrefs.Save();

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
    public int GetHighScorePlayer() { return highScorePlayer; }
}
