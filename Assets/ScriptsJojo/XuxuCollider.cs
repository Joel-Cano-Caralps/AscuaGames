using UnityEngine;

public class XuxuCollider : MonoBehaviour
{
    Xuxu xuxu;
    Vector3 velocidadRotacion = new Vector3(0f, 90f, 0f);
    float MoreTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoreTime = 2f;
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
            xuxu.GetComponent<Timer>().MoreTime(MoreTime);
            Destroy(gameObject);
        }
    }
}
