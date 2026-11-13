
using UnityEngine;

public class DoorSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip doorOpenSound;

    public void PlayDoorSound()
    {
        audioSource.PlayOneShot(doorOpenSound);
    }
}
