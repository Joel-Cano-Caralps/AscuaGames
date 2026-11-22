using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitAudio;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        audioSource.PlayOneShot(hitAudio,0.5f);
    }
}
