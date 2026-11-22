using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BallBomb : MonoBehaviour
{
    [SerializeField] private float delayTime = 5f;
    [SerializeField] private GameObject particles;

    [Header("Blast")]
    [SerializeField] private float blastRadius = 5f;
    [SerializeField] private float blastForce = 15f;
    [SerializeField] private float upwardsModifier = 1f;
    [SerializeField] private float stunTime = 1.5f;
    [SerializeField] private LayerMask playerMask = ~0;

    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(delayTime);

        Instantiate(particles, transform.position, quaternion.identity);
        BlastPlayers();
        Destroy(gameObject);
    }

    void BlastPlayers()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, blastRadius, playerMask, QueryTriggerInteraction.Ignore);
        var processed = new System.Collections.Generic.HashSet<PlayerController>();

        foreach (Collider col in hits)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();
            if (player == null || !processed.Add(player)) continue;

            player.Stun(stunTime);

            if (player.TryGetComponent(out Rigidbody rb))
            {
                // Falloff: full force at center, none at the edge
                float falloff = 1f - Mathf.Clamp01(Vector3.Distance(transform.position, player.transform.position) / blastRadius);
                rb.AddExplosionForce(blastForce * falloff, transform.position, blastRadius, upwardsModifier, ForceMode.VelocityChange);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
