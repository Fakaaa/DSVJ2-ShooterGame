using UnityEngine.UI;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
    [SerializeField] private Text playerPoints;
    [SerializeField] private Text hpText;
    [SerializeField] private Text ammoPlayer;
    [SerializeField] private Text typeShootText;

    void Start()
    {
        Player.uiShow += ShowUIPlayer;
        Gun.myUI += ShowUIGun;
    }
    private void OnDisable()
    {
        Player.uiShow -= ShowUIPlayer;
        Gun.myUI -= ShowUIGun;
    }
    void ShowUIPlayer(float points, float hp)
    {
        playerPoints.text = "Points: " + points;
        hpText.text = "HP:" + hp;
    }
    void ShowUIGun(string ammo, int typeShoot)
    {
        ammoPlayer.text = ammo;
        switch ((Gun.TypeShoot)typeShoot)
        {
            case Gun.TypeShoot.SingleShoot:
                typeShootText.text = "FireMode - SingleShoot";
                break;
            case Gun.TypeShoot.Automatic:
                typeShootText.text = "FireMode - Automatic";
                break;
        }
    }
}
