using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Pathfinder : MonoBehaviour
{
    public enum Option
    {
        DFS,
        BFS,
    }
    public Option option;

    public Vector2 destination;
    [SerializeField] private LayerMask _pathMask;
    public IEnumerable<Vector2> path;
    private LineRenderer _lineRenderer;
    [SerializeField] private Vector3 _lineRendererOffset = Vector3.back;
    [SerializeField] private float _lineRenderSpeed = 1.0f;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        TryGetPath(option);
    }

    public bool TryGetPath(Option option)
    {
        bool result = false;
        switch (option)
        {
            case Option.DFS:
                result = TryGetDFSPath(out path);
                break;
            case Option.BFS:
                result = TryGetBFSPath(out path);
                break;
            default:
                break;
        }
        return result;
    }

    public bool TryGetDFSPath(out IEnumerable<Vector2> path)
    {
        Node[,] map = MapInfo.map;
        bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)]; // �湮 Ȯ�� �迭 (������ ��δ� �ٽ� ���ư��� �ȵǹǷ�)
        Stack<Coord> stack = new Stack<Coord>(); // ���� Ž�� ��� ����
        List<Pair> pairs = new List<Pair>(); // �湮�Ϸ��� ��ν� ���
        Coord start = MapInfo.VectorToCoord(transform.position); // ���� �� ��ġ�� ���� �� ��ǥ
        Coord end = MapInfo.VectorToCoord(destination); // ������ ��ġ�� ���� �� ��ǥ

        stack.Push(start);
        int[,] dir = new int[2, 4]
        {
           //��,��,��,��
            { 0,-1, 0, 1 }, // y
            {-1, 0, 1, 0 }, // x
        };

        while (stack.Count > 0)
        {
            Coord current = stack.Pop(); // �� ������ ������ Ž������
            visited[current.y, current.x] = true; // ��� ������ Ž�� �Ѱŷ� üũ

            // Ž���Ϸ�
            if (current == end)
            {
                path = BacktrackPath(start, end, pairs);
                return true;
            }
            else
            {
                // Ž������ ���� �ݴ�� Ž�����ÿ� �ױ�
                for (int i = dir.GetLength(1) - 1; i >= 0; i--)
                {
                    Coord next = current + new Coord(dir[1, i], dir[0, i]);

                    // ���� ���� �ʰ��ϴ��� Ȯ��
                    if (next.x < 0 || next.x >= map.GetLength(1) ||
                        next.y < 0 || next.y >= map.GetLength(0))
                        continue;

                    // �̹� �湮 �� �������
                    if (visited[next.y, next.x])
                        continue;

                    // �������� ���� ������
                    if ((1 << map[next.y, next.x].layer & _pathMask) == 0)
                        continue;

                    stack.Push(next);
                    pairs.Add(new Pair(current, next));
                }
            }
        }

        path = null;
        return false;
    }

    public bool TryGetBFSPath(out IEnumerable<Vector2> path)
    {
        Node[,] map = MapInfo.map;
        bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)]; // �湮 Ȯ�� �迭 (������ ��δ� �ٽ� ���ư��� �ȵǹǷ�)
        Queue<Coord> queue = new Queue<Coord>(); // ���� Ž�� ��� ��⿭
        List<Pair> pairs = new List<Pair>(); // �湮�Ϸ��� ��ν� ���
        Coord start = MapInfo.VectorToCoord(transform.position); // ���� �� ��ġ�� ���� �� ��ǥ
        Coord end = MapInfo.VectorToCoord(destination); // ������ ��ġ�� ���� �� ��ǥ

        queue.Enqueue(start);
        int[,] dir = new int[2, 4]
        {
           //��,��,��,��
            { 0,-1, 0, 1 }, // y
            {-1, 0, 1, 0 }, // x
        };

        while (queue.Count > 0)
        {
            Coord current = queue.Dequeue(); // �� ������ ������ Ž������
            visited[current.y, current.x] = true; // ��� ������ Ž�� �Ѱŷ� üũ

            // Ž���Ϸ�
            if (current == end)
            {
                path = BacktrackPath(start, end, pairs);
                return true;
            }
            else
            {
                // Ž������ ���� ��� ��⿭ ���
                for (int i = 0; i < dir.GetLength(1); i++)
                {
                    Coord next = current + new Coord(dir[1, i], dir[0, i]);

                    // ���� ���� �ʰ��ϴ��� Ȯ��
                    if (next.x < 0 || next.x >= map.GetLength(1) ||
                        next.y < 0 || next.y >= map.GetLength(0))
                        continue;

                    // �̹� �湮 �� �������
                    if (visited[next.y, next.x])
                        continue;

                    // �������� ���� ������
                    if ((1 << map[next.y, next.x].layer & _pathMask) == 0)
                        continue;

                    queue.Enqueue(next);
                    pairs.Add(new Pair(current, next));
                }
            }
        }

        path = null;
        return false;
    }

    private List<Vector2> BacktrackPath(Coord start, Coord end, List<Pair> pairs)
    {
        List<Vector2> path = new List<Vector2>();

        int index = pairs.FindLastIndex(pair => pair.child == end);

        if (index < 0)
            throw new Exception("[Pathfinder] : ��� ������ ����. �ùٸ������� Ž��");

        path.Add(MapInfo.CoordToVector(pairs[index].child));
        while (index > 0 &&
               pairs[index].parent != start)
        {
            path.Add(MapInfo.CoordToVector(pairs[index].parent));
            index = pairs.FindLastIndex(pair => pair.child == pairs[index].parent);
        }
        path.Add(MapInfo.CoordToVector(start));
        path.Reverse();
        StartCoroutine(C_DrawPreviewLine(path));
        return path;
    }

    private IEnumerator C_DrawPreviewLine(List<Vector2> path)
    {
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, path[0]);
        
        for (int i = 0; i < path.Count - 1; i++)
        {
            float timeMark = Time.time;
            float t = 0;
            while (t < 1)
            {
                t = _lineRenderSpeed * (Time.time - timeMark);
                Vector3 pos = Vector3.Lerp(path[i], path[i + 1], t) +
                              _lineRendererOffset;

                _lineRenderer.SetPosition(i, pos);
                yield return null;
            }

            if (i < path.Count - 2)
                _lineRenderer.positionCount++;
        }
    }
}
