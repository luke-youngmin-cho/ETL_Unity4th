using System;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
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

    [Serializable]
    public struct Coord
    {
        public int x, y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private Node[,] _map;
    private int _totalY;
    private int _totalX;
    private BoxCollider2D _bound;
    private Grid _grid;
    [SerializeField] private LayerMask _mapMask;

    private Vector2 _origin;
    private Vector2 _end;

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
        _map = new Node[_totalY, _totalX];


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

                _map[i, j] = new Node(new Coord(j, i), layer, 0);
            }
        }

        Debug.Log($"[MapInfo] : Successfully set up !");
    }
}
