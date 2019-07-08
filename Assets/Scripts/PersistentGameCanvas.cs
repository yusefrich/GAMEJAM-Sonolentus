using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistentGameCanvas : MonoBehaviour
{
    public GameObject[] hearths;

    public Sprite dreamHearts;
    public Sprite nightmareHearts;

    public GameObject startScreen;

    public GameObject headerDream;
    public GameObject headerNightmare;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        
        for (int i = 0; i < hearths.Length; i++)
        {
            hearths[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
            startScreen.SetActive(false);
        }
        
        print(CustomPlayerController.Instance.life);
        for (int i = 0; i < hearths.Length; i++)
        {
            if (CustomPlayerController.Instance.onDream)
            {
                headerDream.SetActive(true);
                headerNightmare.SetActive(false);
                hearths[i].GetComponent<Image>().sprite = dreamHearts;
            }
            else
            {
                headerDream.SetActive(false);
                headerNightmare.SetActive(true);
                hearths[i].GetComponent<Image>().sprite = nightmareHearts;
            }

            if (i > CustomPlayerController.Instance.life)
            {
                hearths[i].SetActive(false);
            }
        }
    }
}
