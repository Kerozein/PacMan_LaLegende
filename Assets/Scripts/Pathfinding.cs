using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    enum TileType
    {
        Ground,
        Wall
    }

    class Tile
    {
        public HashSet<TileType> types;

        /*public List<Vector2> GetVoisins()
        {

        }*/
    }

    [SerializeField] private Grid gameGrid;
    [SerializeField] private Tile[,] grid;
    [SerializeField] private Tuple<Vector2, Vector2> bounds;
    [SerializeField] private int Rows;
    [SerializeField] private int Columns;

    private void Start()
    {
        bounds = GetGridBounds();
        Rows = (int)(bounds.Item2[0] - bounds.Item1[0]);
        Columns = (int)(bounds.Item2[1] - bounds.Item1[1]);
        grid = new Tile[Rows, Columns];
        FillGrid();
    }

    private Tuple<Vector2, Vector2> GetGridBounds()
    {
        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;

        for (int i = 0; i < gameGrid.transform.childCount; i++)
        {
            Tilemap t = gameGrid.transform.GetChild(i).GetComponent<Tilemap>();
            if (t != null)
            {
                BoundsInt bounds = t.cellBounds;
                if (bounds.xMin < min.x) min.x = bounds.xMin;
                if (bounds.xMax > max.x) max.x = bounds.xMax;

                if (bounds.yMin < min.y) min.y = bounds.yMin;
                if (bounds.yMax > max.y) max.y = bounds.yMax;
            }
        }

        return new Tuple<Vector2,Vector2>(min, max);
    }

    private void FillGrid()
    {
        int ground = LayerMask.NameToLayer("Ground");
        int wall = LayerMask.NameToLayer("Wall");

        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                grid[x, y] = new Tile();
                grid[x, y].types = new HashSet<TileType>();
                for (int i = 0; i < gameGrid.transform.childCount; i++)
                {
                    Tilemap tilemap = gameGrid.transform.GetChild(i).GetComponent<Tilemap>();
                    Vector3Int cellpos = tilemap.WorldToCell(new Vector3(bounds.Item1.x + x * gameGrid.cellSize.x,
                        bounds.Item1.y + x * gameGrid.cellSize.y));
                    TileBase tile = tilemap.GetTile(cellpos);
                    if (tile != null)
                    {
                        int layer = tilemap.gameObject.layer;
                        if (layer == ground) grid[x, y].types.Add(TileType.Ground);
                        if (layer == wall) grid[x, y].types.Add(TileType.Wall);
                    }

                }
            }
        }
    }
}
