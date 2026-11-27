using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Xuxu : MonoBehaviour
{
    [SerializeField] GameObject[] spawnXuxu;
    public GameObject XuxuPrefab;


    void Start()
    {
        CorrSpawn();
    }

    public void CorrSpawn()
    {
        StartCoroutine(SpawnXuxu());
    }

    public IEnumerator SpawnXuxu()
    {
        yield return new WaitForSeconds(Random.Range(7f, 12f));
        Instantiate(XuxuPrefab, spawnXuxu[Random.Range(0, 4)].transform.position, Quaternion.identity);

    }
}
