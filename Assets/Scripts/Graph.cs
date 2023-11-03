using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Tile
{
    private Vector2Int _position;

    public int cost { get;}

    private List<Tile> _neighbors;
    private List<Link> _links;

    private HashSet<TileType> _types;

    public Tile(Vector2Int position)
    {
        _types = new HashSet<TileType>();
        _neighbors = new List<Tile>();
        _links = new List<Link>();
        _position = position;
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

    public void AddLink(Link link)
    {
        _links.Add(link);
    }

    public void RemoveLink(Link link)
    {
        _links.Remove(link);
    }

    public List<Tile> GetNeighbors()
    {
        return _neighbors;
    }

    public List<Link> GetLinksNeighbors()
    {
        return _links;
    }

    public Vector2Int GetPosition()
    {
        return _position;
    }
}

public class Link
{
    private Tile _start;
    private Tile _end;
    public int cost { get; set; }

    public Link(Tile start, Tile end)
    {
        _start = start;
        _end = end;
        cost = 1;
    }

    public Tile GetStart()
    {
        return _start;
    }
    public Tile GetEnd()
    {
        return _end;
    }

    public void SetEnd(Tile end)
    {
        _end = end;
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

    public void Init()
    {
        FillGraphNeighbors();
    }

    public void AddType(Vector2Int pos, TileType type)
    {
        if (!_nodes.ContainsKey(pos))
        {
            _nodes.Add(pos, new Tile(pos));
        }
        _nodes[pos].AddType(type);
    }

    public Tile GetTile(Vector2Int pos)
    {

        Tile tile;
        if(_nodes.TryGetValue(pos, out tile))
            return tile;
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

    public void Simplify()
    {
        foreach (var node in _nodes)
        {
            foreach (var link in node.Value.GetLinksNeighbors())
            {
                Tile start = link.GetStart();
                foreach (var neighbor in start.GetNeighbors())
                {
                    if (neighbor.GetNeighbors().Count == 2)
                    {
                        int cost = link.cost;
                        Direction direction = GetDirection(start, neighbor);
                        Tile end = neighbor;
                        while (end != null)
                        {
                            Link oldLink = GetLink(start, end);
                            if (oldLink != null)
                            {
                                start = end;
                                cost += oldLink.cost;
                                neighbor.RemoveLink(oldLink);
                                Tile tryEnd = GetVoisin(end, direction);
                                if (tryEnd != null)
                                {
                                    end = tryEnd;
                                }
                                else break;
                            }
                            else break;
                        }
                        link.SetEnd(end);
                        link.cost = cost;
                    }
                }
            }
            
        }
    }

    public Direction GetDirection(Tile start, Tile end)
    {
        Direction direction = Direction.Null;
        if (start.GetPosition().x > end.GetPosition().x) direction = Direction.East;
        if (start.GetPosition().x < end.GetPosition().x) direction = Direction.West;
        if (start.GetPosition().y > end.GetPosition().y) direction = Direction.North;
        if (start.GetPosition().y < end.GetPosition().y) direction = Direction.South;
        return direction;
    }

    public Link GetLink(Tile start, Tile end)
    {
        foreach (var link in start.GetLinksNeighbors())
        {
            if(link.GetEnd() == end) return link;
        }
        return null;
    }

    public Tile GetVoisin(Tile start, Direction direction)
    {
        Vector2Int offset;
        switch (direction)
        {
            case Direction.North:
                offset = Vector2Int.up;
                break;
            case Direction.South:
                offset = Vector2Int.down;
                break;
            case Direction.West:
                offset = Vector2Int.left;
                break;
            case Direction.East: 
                offset = Vector2Int.right;
                break;
            default:
                offset = Vector2Int.zero;
                break;
        }

        if (_nodes.ContainsKey(start.GetPosition() + offset))
        {
            return _nodes[start.GetPosition() + offset];
        }
        return null;
    }

    private void FillGraphNeighbors()
    {
        var e = _nodes.GetEnumerator();
        while (e.MoveNext())
        {
            Vector2Int position = e.Current.Key;
            Tile tile = e.Current.Value;

            Vector2Int up = position + new Vector2Int(0, 1);
            if (IsValidNeighbor(up)) tile.AddNeighbor(GetTile(up));

            Vector2Int down = position + new Vector2Int(0, -1);
            if (IsValidNeighbor(down)) tile.AddNeighbor(GetTile(down));

            Vector2Int left = position + new Vector2Int(-1, 0);
            if (IsValidNeighbor(left)) tile.AddNeighbor(GetTile(left));

            Vector2Int right = position + new Vector2Int(1, 0);
            if (IsValidNeighbor(right)) tile.AddNeighbor(GetTile(right));
        }
        FillLink();
    }

    private void FillLink()
    {
        foreach (var node in _nodes)
        {
            foreach (var neighbor in node.Value.GetNeighbors())
            {
                node.Value.AddLink(new Link(node.Value, neighbor));
            }
        }
    }
}
