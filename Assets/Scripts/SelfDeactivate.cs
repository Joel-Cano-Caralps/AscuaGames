using UnityEngine;

public class SelfDeactivate : MonoBehaviour
{
    [SerializeField] private float maxTime = 1f;
    private float currentTime = 0f;

    void OnEnable()
    {
        currentTime = maxTime;
    }
    void Update()
    {
        currentTime-=Time.deltaTime;
        if(currentTime<0){gameObject.SetActive(false);}
    }
}
