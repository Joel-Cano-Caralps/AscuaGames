using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider timeSlider;
    public float tiempoGame;
    bool stopTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stopTimer = false;
        timeSlider.maxValue = tiempoGame;
        timeSlider.value = tiempoGame;
    }

    // Update is called once per frame
    void Update()
    {
        tiempoGame -= Time.deltaTime;

        if(tiempoGame <= 0)
        {
            stopTimer = true;
            timeSlider.gameObject.SetActive(false);
        }

        if(!stopTimer)
        {
            timeSlider.value = tiempoGame;
        }
    }

    public bool GetTimerStop()
    {
        return stopTimer;
    }    
}
