using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FindPlayer : MonoBehaviour
{
    private Graph _graph;
    [HideInInspector] private GameObject _player;

    private void Start()
    {
        _graph = GameObject.Find("Grid").GetComponent<GenerateGraph>().graph;
        _player = GameObject.Find("Player");
        Tile start = _graph.GetTile(GetComponent<EnemyController>().graphPosition);
        Tile end = _graph.GetTile(_player.GetComponent<PlayerController>().graphPosition);
        Queue<Tile> path = Dijkstra(start, end);
    }

    Queue<Tile> Dijkstra(Tile start, Tile goal)
    {
        Dictionary<Tile, Tile> NextTileToGoal = new Dictionary<Tile, Tile>();
        Dictionary<Tile, int> costToReachTile = new Dictionary<Tile, int>();//Total Movement Cost to reach the tile

        PriorityQueue<Tile> frontier = new PriorityQueue<Tile>();
        frontier.Enqueue(goal, 0);
        costToReachTile[goal] = 0;

        while (frontier.count > 0)
        {
            Tile curTile = frontier.Dequeue();
            if (curTile == start)
                break;

            foreach (var tupleNeighbor in _graph.GetNeighbors(curTile))
            {
                Tile neighbor = _graph.GetTile(tupleNeighbor.Item2);
                int newCost = costToReachTile[curTile] + neighbor.cost;
                if (costToReachTile.ContainsKey(neighbor) == false || newCost < costToReachTile[neighbor])
                {
                    costToReachTile[neighbor] = newCost;
                    int priority = newCost;
                    frontier.Enqueue(neighbor, priority);
                    NextTileToGoal[neighbor] = curTile;
                }
            }
        }

        //Get the Path

        //check if tile is reachable
        if (NextTileToGoal.ContainsKey(start) == false)
        {
            return null;
        }

        Queue<Tile> path = new Queue<Tile>();
        Tile pathTile = start;
        while (goal != pathTile)
        {
            pathTile = NextTileToGoal[pathTile];
            path.Enqueue(pathTile);
        }
        return path;
    }

}
