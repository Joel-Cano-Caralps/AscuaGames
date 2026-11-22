using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Scorer : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private int scoreAmmount = 1;
    [SerializeField] private GameObject floatingNumb, particle, particleSmoke;
    [SerializeField] private Collider coll;
    public int positionScore;
    public GameObject ball;
    public Material colorOutline;

    public void desactivateCol()
    {
        if(ball.GetComponent<CustomGravity>().positionScore != positionScore)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
            gameController.setNumBall(positionScore);
        }
    }

    public void setBall(GameObject b)
    {
        ball = b;
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
