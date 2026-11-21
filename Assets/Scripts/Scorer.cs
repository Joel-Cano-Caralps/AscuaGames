using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Scorer : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private int scoreAmmount = 1;
    [SerializeField] private GameObject floatingNumb;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            gameController.Score(scoreAmmount);
            if(floatingNumb != null){floatingNumb.SetActive(true);}
        }
    }
}
