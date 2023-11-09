using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Node
{
    public Coord coord;
    public int layer;
    public int itemID;

    public Node(Coord coord, int layer, int itemID)
    {
        this.coord = coord;
        this.layer = layer;
        this.itemID = itemID;
    }
}

public struct Pair
{
    public Coord parent;
    public Coord child;

    public Pair(Coord parent, Coord child)
    {
        this.parent = parent;
        this.child = child;
    }
}

[Serializable]
public struct Coord
{
    public static Coord up => new Coord(0, 1);
    public static Coord right => new Coord(1, 0);
    public static Coord down => new Coord(0, -1);
    public static Coord left => new Coord(-1, 0);

    public int x, y;
    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // 연산자 정의/오버로딩
    public static bool operator ==(Coord a, Coord b)
        => (a.x == b.x) && (a.y == b.y);

    public static bool operator !=(Coord a, Coord b)
        => !(a == b);

    public static Coord operator +(Coord a, Coord b)
        => new Coord(a.x + b.x, a.y + b.y);

    public static Coord operator -(Coord a, Coord b)
        => new Coord(a.x - b.x, a.y - b.y);
}

public class MapInfo : MonoBehaviour
{
    public static Node[,] map;
    private int _totalY;
    private int _totalX;
    private static BoxCollider2D _bound;
    private static Grid _grid;
    [SerializeField] private LayerMask _mapMask;

    private static Vector2 _origin;
    private static Vector2 _end;

    public static Coord VectorToCoord(Vector2 point)
    {
        return new Coord(Mathf.RoundToInt((point.x - _origin.x) / _grid.cellSize.x),
                         Mathf.RoundToInt((point.y - _origin.y) / _grid.cellSize.y));
    }

    public static Node GetNode(Vector2 point)
    {
        Coord coord = VectorToCoord(point);
        return map[coord.y, coord.x];
    }

    public static Vector2 CoordToVector(Coord coord)
    {
        return new Vector2(coord.x * _grid.cellSize.x + _origin.x,
                           coord.y * _grid.cellSize.y + _origin.y);
    }


    private void Awake()
    {
        _bound = GetComponent<BoxCollider2D>();
        _grid = GetComponent<Grid>();
        SetUp();
    }

    private void SetUp()
    {
        _origin = (Vector2)transform.position
                  + _bound.offset
                  - _bound.size / 2.0f
                  + (Vector2)_grid.cellSize / 2.0f;

        _end = (Vector2)transform.position
               + _bound.offset
               + _bound.size / 2.0f
               - (Vector2)_grid.cellSize / 2.0f;

        float cellSizeX = _grid.cellSize.x;
        float cellSizeY = _grid.cellSize.y;
        _totalY = (int)((_end.y - _origin.y) / cellSizeY) + 1;
        _totalX = (int)((_end.x - _origin.x) / cellSizeX) + 1;
        map = new Node[_totalY, _totalX];


        int towerNodeLayer = LayerMask.NameToLayer("TowerNode");
        int enemyPathLayer = LayerMask.NameToLayer("EnemyPath");

        Vector2 point = _origin;
        for (int i = 0; i < _totalY; i++)
        {
            point = _origin + Vector2.up * cellSizeY * i;
            for (int j = 0; j < _totalX; j++)
            {
                Collider2D col =
                    Physics2D.OverlapPoint(point + Vector2.right * cellSizeX * j, _mapMask);

                int layer = 0;

                if (col != null)
                {
                    if (col.gameObject.layer == towerNodeLayer)
                        layer = towerNodeLayer;
                    else if (col.gameObject.layer == enemyPathLayer)
                        layer = enemyPathLayer;
                }

                map[i, j] = new Node(new Coord(j, i), layer, 0);
            }
        }

        Debug.Log($"[MapInfo] : Successfully set up !");
    }
}
