using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckNextSquareLock : MonoBehaviour
{
    public RayDirection myDirection;
    public enum RayDirection
    {
        up, down, right, left
    }
    public enum CollisionType
    {
        none, obstacle, enemy, diferentGround, player
    }

    public LayerMask EnemyLayer;
    public bool isEnemy;
    public bool centerCheck;
    

    private Vector3 rayDirection;
    // Start is called before the first frame update
    void Start()
    {
        //Physics2D.queriesStartInColliders = false;
        switch (myDirection)
        {
            case RayDirection.up:
                rayDirection = transform.up;
                break;
            case RayDirection.down:
                rayDirection = -transform.up;
                break;
            case RayDirection.right:
                rayDirection = transform.right;
                break;
            case RayDirection.left:
                rayDirection = -transform.right;
                break;
            default:
                rayDirection = transform.right;
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Update()
    {
        CollisionType collitionReference = CollisionType.none;
        if (centerCheck)
        {
            bool teste = CheckCenterSquare();
            
        }
        else
        {
            bool teste = CheckSquare(ref collitionReference);
            
        }
    }

    // Update is called once per frame
    public bool CheckSquare(ref CollisionType collitionReference)
    {
        if (!isEnemy)
        {
            
            RaycastHit2D hitInfoEnemy = Physics2D.Raycast(transform.position, rayDirection, 1, EnemyLayer);
            if (hitInfoEnemy.collider != null)
            {
                collitionReference = CollisionType.enemy;
                Debug.DrawLine(transform.position, hitInfoEnemy.point, Color.magenta);
    
                return true;
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position  + rayDirection * 1, Color.cyan);
            }
        
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, rayDirection, 1);
        if (hitInfo.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);

            if (isEnemy)
            {
                collitionReference = CollisionType.none;
                return true;
            }
            
            
            //pegar o colisor do hit
            //print(hitInfo.collider.bounds.max);
            if (hitInfo.collider.CompareTag("DreamGround") && CustomPlayerController.Instance.onDream)
            {
                collitionReference = CollisionType.none;
                
            }
            else if (hitInfo.collider.CompareTag("NightmareGround") && !CustomPlayerController.Instance.onDream)
            {
                collitionReference = CollisionType.none;
            }
            else
            {
                collitionReference = CollisionType.diferentGround;
            }
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position  + rayDirection * 1, Color.green);
            collitionReference = CollisionType.none;
            return false;
        }
        

    }

    public bool CheckCenterSquare()
    {
        RaycastHit2D hitInfoEnemy = Physics2D.Raycast(transform.position, rayDirection, .5f, EnemyLayer);
        if (hitInfoEnemy.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfoEnemy.point, Color.magenta);
    
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position  + rayDirection * .5f, Color.cyan);
            return false;
        }

    }

    public Vector3 CalculateNextGridPos(Tilemap myBase)
    {
        GridLayout gridLayout = myBase.GetComponentInParent<GridLayout>();
        Vector3Int cellPosition = myBase.WorldToCell(transform.position);
        //print(gridLayout.CellToWorld(cellPosition));
        //gridLayout.CellToLocalInterpolated()
        
        return myBase.GetCellCenterWorld(cellPosition);

    }
}
