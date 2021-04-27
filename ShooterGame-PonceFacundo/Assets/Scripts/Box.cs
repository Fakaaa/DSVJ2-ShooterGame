using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] public float pointsGived;
    private AudioSource takeBox;

    private void Start()
    {
        takeBox = gameObject.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            takeBox.Play();
            if (FindObjectOfType<Player>() != null)
                FindObjectOfType<Player>().SetPoints(200);
        }
        Destroy(gameObject);
    }
}
