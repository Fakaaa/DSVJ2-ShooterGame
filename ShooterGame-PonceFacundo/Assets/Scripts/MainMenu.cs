using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public GameObject panelScore;
    [SerializeField] public Text theHighscore;
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
        theHighscore.text = "Best score:  " + GameManager.Get().GetHighScorePlayer();
    }
}
