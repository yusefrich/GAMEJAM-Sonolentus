using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSoundTrack : MonoBehaviour
{
    public static PersistentSoundTrack instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    public AudioClip dreamSound;
    public AudioClip nightmareSound;

    public GameObject nextLevelSFX;

    bool playingDreamSound;
    
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
                
            //if not, set instance to this
            instance = this;
            
        //If instance already exists and it's not this:
        else if (instance != this)
                
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    
            
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
            
    }

    public void PlayNextLevelSFX()
    {
        nextLevelSFX.GetComponent<AudioSource>().Play();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (CustomPlayerController.Instance.onDream && !playingDreamSound)
        {
            playingDreamSound = true;
            GetComponent<AudioSource>().clip = dreamSound;
            GetComponent<AudioSource>().Play();
        }
        else if (!CustomPlayerController.Instance.onDream && playingDreamSound)
        {
            playingDreamSound = false;
            GetComponent<AudioSource>().clip = nightmareSound;
            GetComponent<AudioSource>().Play();
            
        }
    }
}
