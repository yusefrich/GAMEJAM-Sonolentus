using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastSteppedGround : MonoBehaviour
{
    private int stepsToTake = 3;
    public GameObject x_small;
    public GameObject x_med;
    public GameObject x_big;
    

    private SquareLockMovement currentMovementLockSpawnedOf;

    private void Start()
    {
        GetComponent<Animator>().SetTrigger("pop");
        CustomPlayerController.Instance.deathEvent += OnPlayerDeath;
    }

    /// <summary>
    /// to get the reference from the event called in the squareLockMovement()
    /// </summary>
    /// <param name="movementLockSpawnedOf"></param>
    public void SetEventReference(SquareLockMovement movementLockSpawnedOf)
    {
        print("reference set");
        currentMovementLockSpawnedOf = movementLockSpawnedOf;
        movementLockSpawnedOf.moveEvent += DecreaseTimer;
    }

    public void OnPlayerDeath()
    {
        currentMovementLockSpawnedOf.moveEvent -= DecreaseTimer;
        CustomPlayerController.Instance.deathEvent -= OnPlayerDeath;

        Destroy(gameObject);
    }

    public void DecreaseTimer()
    {
        stepsToTake--;

        GetComponent<Animator>().SetTrigger("pop");

        if (stepsToTake > 0)
        {
            switch (stepsToTake)
            {
                case 2:
                    x_big.SetActive(false);
                    break;
                case 1:
                    x_med.SetActive(false);
                    break;
            }
            
            return;
        }
        currentMovementLockSpawnedOf.moveEvent -= DecreaseTimer;
        CustomPlayerController.Instance.deathEvent -= OnPlayerDeath;

        Destroy(gameObject);
    }
}
