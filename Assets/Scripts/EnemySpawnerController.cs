using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemies;

    [SerializeField] private List<Vector2Int> _enemiesSpawnPosition;

    [SerializeField] private float releaseTiming = 10f;

    private List<GameObject> instanciateEnemies;

    private int _lastGhostReleased = 0;

    private Vector3 cellSize;

    private GraphController graphGenerator;

    void Start()
    {
        instanciateEnemies = new List<GameObject>();
        graphGenerator = GameObject.Find("Grid").GetComponent<GraphController>();
        cellSize = graphGenerator.grid.cellSize;
        for(int i = 0 ; i < _enemies.Count ; i++) SpawnGhost(i);
        InvokeRepeating("StartGhostMovement",0,releaseTiming);
    }

    void SpawnGhost(int ghostIndex)
    {
        if (_enemies.Count > ghostIndex && _enemiesSpawnPosition.Count > ghostIndex)
        {
            Vector3 pos = new Vector3((float)(_enemiesSpawnPosition[ghostIndex].x * cellSize.x + graphGenerator.bounds.Item1.x + 0.5),
                (float)(_enemiesSpawnPosition[ghostIndex].y * cellSize.y + graphGenerator.bounds.Item1.y + 0.5));
            GameObject enemy = Instantiate(_enemies[ghostIndex], pos, Quaternion.identity);
            enemy.name = "Enemy_" + ghostIndex;
            instanciateEnemies.Add(enemy);
        }
    }

    void StartGhostMovement()
    {
        if (_lastGhostReleased < instanciateEnemies.Count)
        {
            instanciateEnemies[_lastGhostReleased].GetComponent<EnemyController>().StartMovement();
            _lastGhostReleased++;
        }
    }

}
