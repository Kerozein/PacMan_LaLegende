using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateGraph : MonoBehaviour
{
    [HideInInspector] public Grid grid;
    [HideInInspector] public Graph graph;
    [HideInInspector] public Tuple<Vector2, Vector2> bounds;
    [HideInInspector] private int Rows;
    [HideInInspector] private int Columns;

    private void Start()
    {
        grid = GetComponent<Grid>();
        bounds = GetGridBounds();
        Rows = (int)(bounds.Item2[0] - bounds.Item1[0]);
        Columns = (int)(bounds.Item2[1] - bounds.Item1[1]);
        graph = new Graph(Rows, Columns);
        FillGrid();
    }

    private Tuple<Vector2, Vector2> GetGridBounds()
    {
        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;

        for (int i = 0; i < grid.transform.childCount; i++)
        {
            Tilemap t = grid.transform.GetChild(i).GetComponent<Tilemap>();
            if (t != null)
            {
                BoundsInt bounds = t.cellBounds;
                if (bounds.xMin < min.x) min.x = bounds.xMin;
                if (bounds.xMax > max.x) max.x = bounds.xMax;

                if (bounds.yMin < min.y) min.y = bounds.yMin;
                if (bounds.yMax > max.y) max.y = bounds.yMax;
            }
        }

        return new Tuple<Vector2, Vector2>(min, max);
    }
    private void FillGrid()
    {
        int ground = LayerMask.NameToLayer("Ground");
        int wall = LayerMask.NameToLayer("Wall");

        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                for (int i = 0; i < grid.transform.childCount; i++)
                {
                    Tilemap tilemap = grid.transform.GetChild(i).GetComponent<Tilemap>();
                    Vector3Int cellpos = tilemap.WorldToCell(new Vector3(bounds.Item1.x + x * grid.cellSize.x,
                        bounds.Item1.y + y * grid.cellSize.y));
                    TileBase tile = tilemap.GetTile(cellpos);
                    if (tile != null)
                    {
                        int layer = tilemap.gameObject.layer;
                        if (layer == ground) graph.AddType(new Vector2Int(x, y), TileType.Ground);
                        else if (layer == wall) graph.AddType(new Vector2Int(x, y), TileType.Wall);
                    }

                }
            }
        }
    }
}
