using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit_get : MonoBehaviour
{
    public AudioClip fruit_get;
    private AudioSource audioSource;
    private bool hasBeenPickedUp = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenPickedUp && other.CompareTag("Player"))
        {
            if (fruit_get != null && audioSource != null)
            {
                audioSource.PlayOneShot(fruit_get);
            }
            hasBeenPickedUp = true;
        }
    }
}
