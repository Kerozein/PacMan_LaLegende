using System.Collections.Generic;
using UnityEngine;

public static class Toolbox
{
    public static Vector2Int ConvertWorldPosToGraphPos(Vector3 pos, GraphController graphGen)
    {
        var bounds = graphGen.bounds;
        var lgX = bounds.Item2.x - bounds.Item1.x;
        var lgY = bounds.Item2.y - bounds.Item1.y;
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
}
