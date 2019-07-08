using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
[RequireComponent(typeof(SquareLockMovement))]
public class CustomPlayerController : MonoBehaviour, _AliveInterace
{
    private SquareLockMovement characterMovement;

    public int life = 2;
    public int stepsToTransport = 10;
    public int currentSteptsToTransport;
    public bool initialDreamState;
    public bool onDream; //if he is not in the dream, he is in the nigthmare

    public SpriteRenderer playerGraphics;
    public Sprite dreamPlayer;
    public Sprite nightmarePlayer;
    
    public delegate void DeathDelegate();
    public event DeathDelegate deathEvent;

    public static CustomPlayerController Instance;
    

    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        onDream = initialDreamState;
        currentSteptsToTransport = stepsToTransport;
        characterMovement = GetComponent<SquareLockMovement>();
    }

    public void TakeStep()
    {
        if (currentSteptsToTransport == 1)
        {
            onDream = !onDream;
            currentSteptsToTransport--;
            return;

        }
        if (currentSteptsToTransport <= 0)
        {
            currentSteptsToTransport = stepsToTransport;
            return;
        }
        currentSteptsToTransport--;
        
    }

    public void CheckForEnemyOverlap()
    {
        print("overlap on player being called");

        GetComponent<SquareLockMovement>().OverlapCheck();
    }

    public void Death()
    {
        deathEvent?.Invoke();
        onDream = initialDreamState;
        currentSteptsToTransport = stepsToTransport;
        life--;

        print("character died");

        if (life < 0)
        {
            SceneManager.LoadScene(0);

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (onDream)
        {
            playerGraphics.sprite = dreamPlayer;
        }
        else
        {
            playerGraphics.sprite = nightmarePlayer;
            
        }
        if (Input.GetButtonDown("Horizontal"))
        {
            Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            characterMovement.Move(moveDirection);
            return;
        }
        
        if (Input.GetButtonDown("Vertical"))
        {
            Vector2 moveDirection = new Vector2(0, Input.GetAxisRaw("Vertical"));
            characterMovement.Move(moveDirection);
        }
    }
}
