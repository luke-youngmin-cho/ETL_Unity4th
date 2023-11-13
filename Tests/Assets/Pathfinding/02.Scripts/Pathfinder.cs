using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public Vector2 destination;
    [SerializeField] private LayerMask _pathMask;
    public IEnumerable<Vector2> path;

    private void Start()
    {
        if (TryGetDFSPath(out path))
        {

        }
        else
        {
            Debug.LogWarning($"[Pathfinder] : DFS 탐색 실패");
        }
    }

    public bool TryGetDFSPath(out IEnumerable<Vector2> path)
    {
        Node[,] map = MapInfo.map;
        bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)]; // 방문 확인 배열 (지나온 경로는 다시 돌아가면 안되므로)
        Stack<Coord> stack = new Stack<Coord>(); // 다음 탐색 대상 스택
        List<Pair> pairs = new List<Pair>(); // 방문완료한 경로쌍 목록
        Coord start = MapInfo.VectorToCoord(transform.position); // 현재 내 위치에 대한 맵 좌표
        Coord end = MapInfo.VectorToCoord(destination); // 목적지 위치에 대한 맵 좌표

        stack.Push(start);
        int[,] dir = new int[2, 4]
        {
           //좌,하,우,상
            { 0,-1, 0, 1 }, // y
            {-1, 0, 1, 0 }, // x
        };

        while (stack.Count > 0)
        {
            Coord current = stack.Pop(); // 젤 위에거 꺼내서 탐색시작
            visited[current.y, current.x] = true; // 방금 꺼낸거 탐색 한거로 체크

            // 탐색완료
            if (current == end)
            {
                path = BacktrackPath(start, end, pairs);
                return true;
            }
            else
            {
                // 탐색방향 패턴 반대로 탐색스택에 쌓기
                for (int i = dir.GetLength(1) - 1; i >= 0; i--)
                {
                    Coord next = current + new Coord(dir[1, i], dir[0, i]);

                    // 맵의 범위 초과하는지 확인
                    if (next.x < 0 || next.x >= map.GetLength(1) ||
                        next.y < 0 || next.y >= map.GetLength(0))
                        continue;

                    // 이미 방문 한 경로인지
                    if (visited[next.y, next.x])
                        continue;

                    // 지나갈수 없는 길인지
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

    private static List<Vector2> BacktrackPath(Coord start, Coord end, List<Pair> pairs)
    {
        List<Vector2> path = new List<Vector2>();

        int index = pairs.FindLastIndex(pair => pair.child == end);

        if (index < 0)
            throw new Exception("[Pathfinder] : 경로 역추적 실패. 올바르지않은 탐색");

        path.Add(MapInfo.CoordToVector(pairs[index].child));
        while (index > 0 &&
               pairs[index].parent != start)
        {
            path.Add(MapInfo.CoordToVector(pairs[index].parent));
            index = pairs.FindLastIndex(pair => pair.child == pairs[index].parent);
        }
        path.Add(MapInfo.CoordToVector(start));
        path.Reverse();
        return path;
    }

}
