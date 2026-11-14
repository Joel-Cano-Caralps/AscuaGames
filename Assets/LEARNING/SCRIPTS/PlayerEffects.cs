using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustParticles;

    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();

    }

    void Update()
    {
        if (playerController.CurrentState == PlayerController.PlayerState.Running)
        {
            if (!dustParticles.isPlaying)
            {
                dustParticles.Play();
            }
        }
        else
        {
            if (dustParticles.isPlaying)
            {
                dustParticles.Stop();
            }
        }
    }
}
