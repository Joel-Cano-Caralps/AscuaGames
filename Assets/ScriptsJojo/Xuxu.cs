using System.Collections;
using UnityEngine;

public class Xuxu : MonoBehaviour
{
    [SerializeField] GameObject[] spawnXuxu;
    public GameObject XuxuPrefab;

    void Update()
    {
        StartCoroutine(SpawnXuxu());
    }

    IEnumerator SpawnXuxu()
    {
        yield return new WaitForSeconds(Random.Range(7f, 13f));
        Instantiate(XuxuPrefab, spawnXuxu[Random.Range(0,3)].transform.position, Quaternion.identity);
    }
}
