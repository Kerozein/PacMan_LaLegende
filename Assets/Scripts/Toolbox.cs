using System.Collections.Generic;
using UnityEngine;

public static class Toolbox
{
    public static Vector2Int ConvertWorldPosToGraphPos(Vector3 pos, GraphController graphGen)
    {
        var bounds = graphGen.bounds;
        return new Vector2Int((int)(pos.x - bounds.Item1.x), (int)(pos.y - bounds.Item1.y));
    }

    public static Vector3 ConvertGraphPosToLocalPos(Vector2Int pos , GraphController graphGen)
    {
        var bounds = graphGen.bounds;
        return new Vector3(bounds.Item1.x + pos.x, bounds.Item1.y + pos.y);
    }

    public static void DrawPath(Queue<Tile> path, GraphController graphGen, Color color)
    {
        if(path == null) return;

        Gizmos.color = color;
        while(path.Count > 0)
        {
            var pos = ConvertGraphPosToLocalPos(path.Dequeue().position, graphGen);
            var posOffset = new Vector2(pos.x + 0.5f, pos.y + 0.5f);
            Gizmos.DrawSphere(posOffset, 0.2f);
        }
    }

    public static Queue<Tile> Dijkstra(Tile start, Tile goal, Graph _graph)
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
