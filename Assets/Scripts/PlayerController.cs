using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Vector2Int playerPosition;
    [SerializeField] [Range(1,3)] public float speed;
    
    [HideInInspector] SpriteRenderer _spriteRenderer;

    private GraphController _graphController;
    private Direction playerDirection;
    private void Start()
    {
        _graphController = GameObject.Find("Grid").GetComponent<GraphController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerDirection = Direction.East;
    }

    private void FixedUpdate()
    {
        playerPosition = Toolbox.ConvertWorldPosToGraphPos(gameObject.transform.position, _graphController);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput != 0)
        {
            if (verticalInput > 0.1 && playerDirection != Direction.North)
            {
                playerDirection = Direction.North;
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (verticalInput < -0.1 && playerDirection != Direction.South) 
            {
                playerDirection = Direction.South;
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            gameObject.transform.position += Vector3.up * verticalInput * speed * 0.01f;
        }
            
        else if (horizontalInput != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (horizontalInput > 0.1 && playerDirection != Direction.East)
            {
                playerDirection = Direction.East;
                _spriteRenderer.flipX = false;
            }
            else if (horizontalInput < -0.1 && playerDirection != Direction.West)
            {
                playerDirection = Direction.West;
                _spriteRenderer.flipX = true;
            }
            gameObject.transform.position += Vector3.right * horizontalInput * speed * 0.01f;
        }
            
    }
}
