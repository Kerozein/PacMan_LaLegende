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
}
