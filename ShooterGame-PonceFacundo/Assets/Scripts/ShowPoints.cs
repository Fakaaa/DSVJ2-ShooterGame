using UnityEngine;
using UnityEngine.UI;

public class ShowPoints : MonoBehaviour
{
    [SerializeField] public Text pointsPlayer;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GameManager.Get() != null)
            pointsPlayer.text = "¡You Die! \n Points: " + GameManager.Get().GetPointsPlayer();
    }
}
