using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class SquareLockMovement : MonoBehaviour
{
    private Tilemap myBase;

    public CheckNextSquareLock movementCheckRight;
    public CheckNextSquareLock movementCheckLeft;
    public CheckNextSquareLock movementCheckUp;
    public CheckNextSquareLock movementCheckDown;
    public CheckNextSquareLock movementCheckCenter;

    private Animator myAnimator;

    //this variable stores the player next position to move it after the walking animations
    private Vector3 playerNextPosition = Vector3.zero;

    [Header("Spawned parts")] 
    public GameObject lastSteppedGroundObj;
    public GameObject enemy;
    public GameObject lastSteppedGroundObjNightmare;
    public GameObject enemyNightmare;

    [Header("movement behavior")] 
    public bool callSquareMovementEvent;
    public bool isEnemy;

    public delegate void MoveDelegate();
    public event MoveDelegate moveEvent;

    private CheckNextSquareLock.CollisionType collision;

    public SpriteRenderer Graphics;
    [Header("SFX")] 
    public AudioSource jumpEffect;
    public AudioSource deathEffect;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        myBase = GameObject.FindWithTag("myBase").GetComponent<Tilemap>();
        //Physics2D.queriesStartInColliders = false;
        myAnimator = GetComponent<Animator>();
        transform.position = CalculateGridPos();
    }

    private void Update()
    {

    }

    /// <summary>
    /// move the player based on a set input
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction)
    {
        bool characterMoved = false;
        
        //preventing the character to move if he is current in movement
        if (playerNextPosition != Vector3.zero) return;
        
        //checking the direction and if theres ground to move
        if (direction == Vector2.right)
        {
            if (movementCheckRight.CheckSquare(ref collision))
            {
                if (!isEnemy)
                {
                    Graphics.flipX = false;
                }
                else
                {
                    Graphics.flipX = true;
                }
                playerNextPosition = movementCheckRight.CalculateNextGridPos(myBase);
                characterMoved = true;
            }
        }else if (direction == Vector2.left)
        {
            if (movementCheckLeft.CheckSquare(ref collision))
            {
                if (!isEnemy)
                {
                    Graphics.flipX = true;
                }
                else
                {
                    Graphics.flipX = false;
                }

                playerNextPosition = movementCheckLeft.CalculateNextGridPos(myBase);
                characterMoved = true;

            }

        }else if (direction == Vector2.up)
        {
            if (movementCheckUp.CheckSquare(ref collision))
            {
                playerNextPosition = movementCheckUp.CalculateNextGridPos(myBase);
                characterMoved = true;
            }

        }else if (direction == Vector2.down)
        {
            if (movementCheckDown.CheckSquare(ref collision))
            {
                playerNextPosition = movementCheckDown.CalculateNextGridPos(myBase);
                characterMoved = true;
            }

        }
        else
        {
            return;
        }
        //setting animations
        if (!characterMoved) return;

        if (!isEnemy)
            GetComponent<CustomPlayerController>().TakeStep();
        
        myAnimator.SetTrigger("walk");
        myAnimator.SetFloat("x", direction.x);
        myAnimator.SetFloat("y", direction.y);
    }

    public void AutomatedMove(GameObject direction)
    {
        float closestDistance;
        CheckNextSquareLock nextSquare;
        Vector3 myMoveDirection;

        //setting the base pos
        closestDistance = Vector3.Distance(movementCheckLeft.gameObject.transform.position, direction.transform.position);
        nextSquare = movementCheckLeft;
        myMoveDirection = Vector3.left;
        
        if (Vector3.Distance(movementCheckUp.gameObject.transform.position, direction.transform.position) <
            closestDistance)
        {
            closestDistance = Vector3.Distance(movementCheckUp.gameObject.transform.position, 
                direction.transform.position);
            nextSquare = movementCheckUp;
            myMoveDirection = Vector3.up;
        }
        else if (Vector3.Distance(movementCheckRight.gameObject.transform.position, direction.transform.position) <
                  closestDistance)
        {
            closestDistance = Vector3.Distance(movementCheckRight.gameObject.transform.position,
                direction.transform.position);
            nextSquare = movementCheckRight;
            myMoveDirection = Vector3.right;
        }
        else if (Vector3.Distance(movementCheckDown.gameObject.transform.position, direction.transform.position) <
                  closestDistance)
        {
            closestDistance = Vector3.Distance(movementCheckDown.gameObject.transform.position,
                direction.transform.position);
            nextSquare = movementCheckDown;
            myMoveDirection = Vector3.down;
        }
        
        //moving the enemy to the closest direction to the player
        Move(myMoveDirection);
        

        
    }

    /// <summary>
    /// this function is called and the end of the animation to update de player position, from de Move() method
    /// </summary>
    public void UpdatePosition()
    {
        
        switch (collision)
        {
            case CheckNextSquareLock.CollisionType.none:
                //call all movement events 
                if(callSquareMovementEvent)
                    moveEvent?.Invoke();
                if (!isEnemy)
                {
                    SpawnLastSteppedGround();
                }
        
                transform.position = playerNextPosition;
                playerNextPosition = Vector3.zero;

                break;
            case CheckNextSquareLock.CollisionType.obstacle:
                break;
            case CheckNextSquareLock.CollisionType.enemy:
                SpawnEnemy();
                GetComponent<_AliveInterace>().Death();
                deathEffect.Play();
                transform.position = new Vector3(.5f,.5f,0);
                playerNextPosition = Vector3.zero;

                break;
            case CheckNextSquareLock.CollisionType.diferentGround:
                SpawnEnemy();
                GetComponent<_AliveInterace>().Death();
                deathEffect.Play();
                transform.position = new Vector3(.5f,.5f,0);
                playerNextPosition = Vector3.zero;
                
                break;
            case CheckNextSquareLock.CollisionType.player:
                CustomPlayerController.Instance.Death();
                deathEffect.Play();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (isEnemy)
        {
            print("checking for player overlap");
            CustomPlayerController.Instance.CheckForEnemyOverlap();
        }
        else
        {
            jumpEffect.Play();
        }
        

    }

    public void OverlapCheck()
    {
        print("OverlapCheck() being called the responce to is: ");

        if (movementCheckCenter!=null)
        {
            print(movementCheckCenter.CheckCenterSquare());

            if (movementCheckCenter.CheckCenterSquare())
            {
                SpawnEnemy();
                GetComponent<_AliveInterace>().Death();
                deathEffect.Play();
                transform.position = new Vector3(.5f,.5f,0);
                playerNextPosition = Vector3.zero;
            }
        }

    }

    /// <summary>
    /// spawn the last stepped ground, if the player steps in the same ground he will die
    /// </summary>
    void SpawnLastSteppedGround()
    {
        //if this type of player dows not spawn a last stepped
        if (lastSteppedGroundObj == null) return;

        if (CustomPlayerController.Instance.onDream)
        {
            GameObject lastSteppedReference = Instantiate(lastSteppedGroundObj, transform.position, transform.rotation);
            lastSteppedReference.GetComponent<LastSteppedGround>().SetEventReference(this);

        }
        else
        {
            GameObject lastSteppedReference = Instantiate(lastSteppedGroundObjNightmare, transform.position, transform.rotation);
            lastSteppedReference.GetComponent<LastSteppedGround>().SetEventReference(this);

        }
        
    }

    void SpawnEnemy()
    {
        if (enemy == null) return;

        if (CustomPlayerController.Instance.onDream)
        {
            GameObject lastSteppedReference = Instantiate(enemy, transform.position, transform.rotation);
            lastSteppedReference.GetComponent<CustomEnemyController>().SetSpawnedEnemy(this);
        }
        else
        {
            GameObject lastSteppedReference = Instantiate(enemyNightmare, transform.position, transform.rotation);
            lastSteppedReference.GetComponent<CustomEnemyController>().SetSpawnedEnemy(this);
        }

    }

    Vector3 CalculateGridPos()
    {
        GridLayout gridLayout = myBase.GetComponentInParent<GridLayout>();
        Vector3Int cellPosition = myBase.WorldToCell(transform.position);
        //print(gridLayout.CellToWorld(cellPosition));
        //gridLayout.CellToLocalInterpolated()
        
        return myBase.GetCellCenterWorld(cellPosition);

    }
}
