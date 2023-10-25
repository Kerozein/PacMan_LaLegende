using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemies;

    [SerializeField] private List<Vector2Int> _enemiesSpawnPosition;

    [HideInInspector] private int _nbEnemies;

    void Start()
    {
        _nbEnemies = _enemiesSpawnPosition.Count;
        GenerateGraph graphGenerator = GameObject.Find("Grid").GetComponent<GenerateGraph>();
        Vector3 cellSize = graphGenerator.grid.cellSize;
        for (int i = 0; i < _nbEnemies; i++)
        {
            Vector3 pos = new Vector3((float)(_enemiesSpawnPosition[i].x * cellSize.x + graphGenerator.bounds.Item1.x + 0.5),
                (float)(_enemiesSpawnPosition[i].y * cellSize.y + graphGenerator.bounds.Item1.y + 0.5));
            GameObject enemy = Instantiate(_enemies[i], pos, Quaternion.identity);
            enemy.name = "Enemy_"+i;
            enemy.GetComponent<EnemyController>().graphPosition = _enemiesSpawnPosition[i];
        }
    }

}
