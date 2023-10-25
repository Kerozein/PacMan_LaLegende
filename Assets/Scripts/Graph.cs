using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2Int position { get; }
    private HashSet<TileType> _types;

    public Tile(Vector2Int pos)
    {
        position = pos;
        _types = new HashSet<TileType>();
    }

    public void AddType(TileType type)
    {
        _types.Add(type);
    }

    public HashSet<TileType> GetTypes()
    {
        return _types;
    }
}

public class Graph
{
    private Tile[,] _grid;

    public Graph(int height, int width)
    {
        _grid = new Tile[height, width];
        for (int x = 0; x < height; x++)
            for (int y = 0; y < width; y++)
                _grid[x, y] = new Tile(new Vector2Int(x,y));
        
    }

    public void AddType(Vector2Int pos, TileType type)
    {
        _grid[pos.x, pos.y].AddType(type);
    }

    public Tile GetTile(Vector2Int position)
    {
        return _grid[position.x, position.y];
    }

    
    public List<Tuple<Direction, Vector2Int>> GetNeighbors(Tile tile)
    {
        List<Tuple<Direction, Vector2Int>> neighbors = new List<Tuple<Direction, Vector2Int>>();
        
        Vector2Int up = tile.position + new Vector2Int(0,1);
        if(IsValidNeighbor(up)) neighbors.Add(new Tuple<Direction, Vector2Int>(Direction.North, up));

        Vector2Int down = tile.position + new Vector2Int(0,-1);
        if (IsValidNeighbor(down)) neighbors.Add(new Tuple<Direction, Vector2Int>(Direction.South, down));

        Vector2Int left = tile.position + new Vector2Int(-1,0);
        if (IsValidNeighbor(left)) neighbors.Add(new Tuple<Direction, Vector2Int>(Direction.West, left));

        Vector2Int right = tile.position + new Vector2Int(1,0);
        if (IsValidNeighbor(right)) neighbors.Add(new Tuple<Direction, Vector2Int>(Direction.East, right));

        return neighbors;
    }

    private bool IsValidNeighbor(Vector2Int position)
    {
        return IsInGrid(position) && IsGround(position);
    }

    private bool IsInGrid(Vector2Int position)
    {
        return position.x >= 0 && position.x < _grid.GetLength(0) && position.y >= 0 && position.y < _grid.GetLength(1);
    }

    private bool IsGround(Vector2Int position)
    {
        return _grid[position.x, position.y].GetTypes().Contains(TileType.Ground);
    }
}
