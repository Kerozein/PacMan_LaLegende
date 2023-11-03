using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Tile
{
    public int cost { get;}

    private List<Tile> _neighbors;

    private HashSet<TileType> _types;

    public Tile()
    {
        _types = new HashSet<TileType>();
        _neighbors = new List<Tile>();
    }

    public void AddType(TileType type)
    {
        _types.Add(type);
    }

    public HashSet<TileType> GetTypes()
    {
        return _types;
    }

    public void AddNeighbor(Tile tile)
    {
        _neighbors.Add(tile);
    }

    public List<Tile> GetNeighbors()
    {
        return _neighbors;
    }
}

public class Graph
{
    private Dictionary<Vector2Int, Tile> _nodes;

    private int _height;
    private int _width;


    public Graph(int height, int width)
    {
        _height = height;
        _width = width;
        _nodes = new Dictionary<Vector2Int, Tile>();
    }

    public void AddType(Vector2Int pos, TileType type)
    {
        if (!_nodes.ContainsKey(pos))
        {
            _nodes.Add(pos, new Tile());
        }
        _nodes[pos].AddType(type);
    }

    public Tile GetTile(Vector2Int pos)
    {
        if (_nodes.ContainsKey(pos))
        {
            return _nodes[pos];
        }
        return null;
    }

    private bool IsValidNeighbor(Vector2Int position)
    {
        return IsInGrid(position) && IsGround(position);
    }

    private bool IsInGrid(Vector2Int position)
    {
        return _nodes.ContainsKey(position);
    }

    private bool IsGround(Vector2Int position)
    {
        return GetTile(position).GetTypes().Contains(TileType.Ground);
    }

    public List<Tile> GetNodes()
    {
        return _nodes.Values.ToList();
    }

    public void FillGraphNeighbors()
    {
        foreach (var node in _nodes)
        {
            Vector2Int position = node.Key;
            Tile tile = node.Value;

            Vector2Int up = position + new Vector2Int(0, 1);
            if (IsValidNeighbor(up)) tile.AddNeighbor(GetTile(up));

            Vector2Int down = position + new Vector2Int(0, -1);
            if (IsValidNeighbor(down)) tile.AddNeighbor(GetTile(down));

            Vector2Int left = position + new Vector2Int(-1, 0);
            if (IsValidNeighbor(left)) tile.AddNeighbor(GetTile(left));

            Vector2Int right = position + new Vector2Int(1, 0);
            if (IsValidNeighbor(right)) tile.AddNeighbor(GetTile(right));
        }
    }

    public Vector2Int GetTilePos(Tile tile)
    {
        if (_nodes.ContainsValue(tile))
        {
            var tuple = _nodes.FirstOrDefault(x => x.Value == tile);
            return tuple.Key;
        }

        return new Vector2Int(-1,-1);
    }
}
