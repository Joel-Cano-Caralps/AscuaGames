using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BallBomb : MonoBehaviour
{
    [SerializeField] private float delayTime = 5f;
    [SerializeField] private GameObject particles;

    [Header("Blast")]
    [SerializeField] private float blastRadius = 5f;
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

            player.Stun(stunTime, true);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
