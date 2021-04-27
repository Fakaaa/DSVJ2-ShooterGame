using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] public float pointsGived;
    [SerializeField] private AudioSource takeBox;

    private void OnTriggerEnter(Collider other)
    {
        takeBox.Play();
        if (other.gameObject.tag == "Player")
        {
            if (FindObjectOfType<Player>() != null)
                FindObjectOfType<Player>().SetPoints(200);
        }
        Destroy(gameObject);
    }
}
