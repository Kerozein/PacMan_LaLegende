using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerController : MonoBehaviour
{

    [SerializeField] private GameObject _player;

    [SerializeField] private Vector2Int playerSpawnPosition;

    void Start()
    {
        GraphController graphGenerator = GameObject.Find("Grid").GetComponent<GraphController>();
        Vector3 cellSize = graphGenerator.grid.cellSize;
        Vector3 pos = new Vector3((float)(playerSpawnPosition.x * cellSize.x + graphGenerator.bounds.Item1.x + 0.5),
            (float)(playerSpawnPosition.y * cellSize.y + graphGenerator.bounds.Item1.y + 0.5));
        GameObject player = Instantiate(_player,pos,Quaternion.identity);
        player.name = "Player";
    }
}
