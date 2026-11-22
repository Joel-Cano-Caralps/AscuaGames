using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Scorer : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private int scoreAmmount = 1;
    [SerializeField] private GameObject floatingNumb, particle;
    [SerializeField] private Collider coll;
    [SerializeField] public MeshRenderer ScorerMaterial;
    bool on;

    void Awake()
    {
        if (coll == null) coll = GetComponent<Collider>();
        coll.enabled = false; // apagado hasta que haya una pelota
    }

    public void CheckMatch(MeshRenderer ball)
    {
        coll.enabled = ScorerMaterial.sharedMaterial == ball.sharedMaterial;
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            gameController.Score(scoreAmmount);
            if(floatingNumb != null){floatingNumb.SetActive(true);}
            if(particle != null){particle.SetActive(true);}
        }
    }
}
