using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour
{
    public Image stamina;

    public GameObject stepsLeftDreams;
    public GameObject stepsLeftNightmares;

    public Sprite staminaDream;
    public Sprite staminaNightmare;
    
    
    

    // Update is called once per frame
    void Update()
    {
        if (CustomPlayerController.Instance.onDream)
        {
            stamina.sprite = staminaDream;
            stepsLeftDreams.SetActive(true);
            stepsLeftNightmares.SetActive(false);
        }
        else
        {
            stamina.sprite = staminaNightmare;
            stepsLeftDreams.SetActive(false);
            stepsLeftNightmares.SetActive(true);
        }
        
        float stepsToTransport =  CustomPlayerController.Instance.stepsToTransport;
        float currentSteptsToTransport =  CustomPlayerController.Instance.currentSteptsToTransport;
        stamina.fillAmount = currentSteptsToTransport / stepsToTransport;

        stepsLeftDreams.GetComponent<Text>().text = CustomPlayerController.Instance.currentSteptsToTransport.ToString();
        stepsLeftNightmares.GetComponent<Text>().text = CustomPlayerController.Instance.currentSteptsToTransport.ToString();
    }
}
