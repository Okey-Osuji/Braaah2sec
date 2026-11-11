
using UnityEngine;

public class ChairSound : MonoBehaviour
{
    public AudioClip knockSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        audioSource.PlayOneShot(knockSound);
    }
}
