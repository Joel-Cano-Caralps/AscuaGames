using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] float WaitSec, WaitSec2;
    [SerializeField] GameObject PanelTuto;
    [SerializeField] PlayerController player;

    void Start()
    {

        if(PlayerPrefs.GetInt("FirstTimeTuto", 0) == 0)
        {
            player.enabled = false;
            StartCoroutine(stopTimeTuto());
            PlayerPrefs.SetInt("FirstTimeTuto", 1);
            PlayerPrefs.Save();
        }
    }

    IEnumerator stopTimeTuto()
    {
        yield return new WaitForSeconds(WaitSec);
        PanelTuto.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(WaitSec2);        
        Time.timeScale = 1;
        PanelTuto.SetActive(false);
        player.enabled = true;
    }
}
