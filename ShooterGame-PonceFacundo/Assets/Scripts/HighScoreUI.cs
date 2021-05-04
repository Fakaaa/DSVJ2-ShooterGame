using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] public GameObject panelScore;
    [SerializeField] public Text theHighscore;
    [SerializeField] public Text lastScore;
    private bool isPanelActive;

    private void Start()
    {
        isPanelActive = false;
    }
    public void ActivatePanel()
    {
        isPanelActive = !isPanelActive;
        panelScore.SetActive(isPanelActive);
    }
    private void Update()
    {
        if (GameManager.Get() != null && theHighscore != null)
            theHighscore.text = "Best score:  " + GameManager.Get().GetHighScorePlayer();
        if(GameManager.Get() != null && lastScore != null)
            lastScore.text = "Last score:  " + GameManager.Get().GetPointsPlayer();
    }
}
