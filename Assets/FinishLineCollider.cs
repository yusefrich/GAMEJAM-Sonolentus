using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FinishLineCollider : MonoBehaviour
{
    public int nextScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("OnTrigger");
            PersistentSoundTrack.instance.PlayNextLevelSFX();
            SceneManager.LoadScene(nextScene);

        }
    }
}
