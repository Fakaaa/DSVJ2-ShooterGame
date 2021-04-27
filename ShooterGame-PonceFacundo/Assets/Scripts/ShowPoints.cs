using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPoints : MonoBehaviour
{
    [SerializeField] public Text pointsPlayer;
    void Start()
    {
        if (GameManager.Get() != null)
            pointsPlayer.text = "¡You Die! \n Points: " + GameManager.Get().GetPointsPlayer().ToString();
    }
}
