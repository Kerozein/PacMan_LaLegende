using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Vector2Int enemyPosition;
    [SerializeField] public GameObject _player;
    [SerializeField] public float speed = 1;
    [SerializeField] private bool _movable = false;
    public GraphController _graphController;

    private void Start()
    {
        _graphController = GameObject.Find("Grid").GetComponent<GraphController>();
        _player = GameObject.Find("Player");
        enemyPosition = Toolbox.ConvertWorldPosToGraphPos(gameObject.transform.position, _graphController);
    }

    private void Update()
    {
        if (_movable)
        {
            enemyPosition = Toolbox.ConvertWorldPosToGraphPos(gameObject.transform.position, _graphController);
            Tile start = _graphController.graph.GetTile(GetComponent<EnemyController>().enemyPosition);
            Tile end = _graphController.graph.GetTile(_player.GetComponent<PlayerController>().playerPosition);
            Queue<Tile> path = Toolbox.AStar(start, end, _graphController.simplifiedGraph);
            if (path != null)
            {
                Tile tile = path.Dequeue();
                if (tile != null)
                {
                    Vector2Int tilePosition = tile.GetPosition();
                    Direction direction = Direction.Null;
                    if (tilePosition.x > enemyPosition.x) direction = Direction.East;
                    if (tilePosition.x < enemyPosition.x) direction = Direction.West;
                    if (tilePosition.y > enemyPosition.y) direction = Direction.North;
                    if (tilePosition.y < enemyPosition.y) direction = Direction.South;
                    MoveToward(direction);
                }
            }
        }
    }

    private void MoveToward(Direction direction)
    {
        if (direction == Direction.North)
        {
            gameObject.transform.position += Vector3.up * speed * Time.deltaTime;
        }

        if (direction == Direction.South)
        {
            gameObject.transform.position += Vector3.down * speed * Time.deltaTime;
        }

        if (direction == Direction.East)
        {
            gameObject.transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (direction == Direction.West)
        {
            gameObject.transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    public void StartMovement()
    {
        _movable = true;
    }

    public void StopMovement()
    {
        _movable = false;
    }

}
