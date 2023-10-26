using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GraphDrawer : MonoBehaviour
{
    private Graph _graph;
    [HideInInspector] private GameObject _player;

    private void Start()
    {
        _graph = GameObject.Find("Grid").GetComponent<GraphController>().graph;
        _player = GameObject.Find("Player");
    }

    private void OnDrawGizmos()
    {
        Tile start = _graph.GetTile(GetComponent<EnemyController>().enemyPosition);
        Tile end = _graph.GetTile(_player.GetComponent<PlayerController>().playerPosition);
        Queue<Tile> path = Toolbox.Dijkstra(start, end, _graph);
        Toolbox.DrawPath(path, GetComponent<EnemyController>()._graphController, Color.red);
    }

}
