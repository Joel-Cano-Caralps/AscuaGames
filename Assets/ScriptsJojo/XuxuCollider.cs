using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class XuxuCollider : MonoBehaviour
{

    Xuxu xuxu;
    Vector3 velocidadRotacion = new Vector3(0f, 90f, 0f);
    [SerializeField] private float timeToAdd = 2f;

    void Start()
    {
        xuxu = FindAnyObjectByType<Xuxu>();
    }

    void Update()
    {
        transform.Rotate(velocidadRotacion * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            xuxu.CorrSpawn();
            xuxu.GetComponent<Timer>().MoreTime(timeToAdd);
            Destroy(gameObject);
        }
    }
}
