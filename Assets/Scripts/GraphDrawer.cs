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
    private GraphController _graphController;
    [HideInInspector] private GameObject _player;

    private void Start()
    {
        _graphController = GameObject.Find("Grid").GetComponent<GraphController>();
        _graph = _graphController.graph;
        _player = GameObject.Find("Player");
    }

    private void OnDrawGizmos()
    {
        Tile start = _graph.GetTile(GetComponent<EnemyController>().enemyPosition);
        Tile end = _graph.GetTile(_player.GetComponent<PlayerController>().playerPosition);
        Queue<Tile> path = Toolbox.Dijkstra(start, end, _graphController.simplifiedGraph);
        if(path != null)
            Toolbox.DrawPath(path, _graphController, Color.red);
    }

}
