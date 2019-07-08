using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgSwitch : MonoBehaviour
{
    public GameObject bgDream;
    public GameObject bgNightmare;

    public Color dreamColor;
    public Color nightmareColor;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CustomPlayerController.Instance.onDream)
        {
            bgDream.SetActive(true);
            bgNightmare.SetActive(false);
            Camera.main.backgroundColor = dreamColor;
        }
        else
        {
            bgDream.SetActive(false);
            bgNightmare.SetActive(true);
            Camera.main.backgroundColor = nightmareColor;
        }
    }
}
