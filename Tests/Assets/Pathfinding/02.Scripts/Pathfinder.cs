using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public Vector2 destination;
    [SerializeField] private LayerMask _pathMask;

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
                path = null;
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

}
