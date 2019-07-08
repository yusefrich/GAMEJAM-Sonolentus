using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SquareLockMovement))]
public class CustomEnemyController : MonoBehaviour, _AliveInterace
{
    //private SquareLockMovement characterMovement;

    private CustomPlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = CustomPlayerController.Instance;
    }

    // Update is called once per frame
    void CalculateMovement()
    {
        GetComponent<SquareLockMovement>().AutomatedMove(player.gameObject);
    }

    public void SetSpawnedEnemy(SquareLockMovement whoSpawnedMe)
    {
        whoSpawnedMe.moveEvent += CalculateMovement;
    }

    public void Death()
    {
        
    }
}
