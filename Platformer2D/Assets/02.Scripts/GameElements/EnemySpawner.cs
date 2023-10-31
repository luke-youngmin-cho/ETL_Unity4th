using Platformer.Controllers;
using Platformer.GameElements.Pool;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements
{
    public class EnemySpawner : MonoBehaviour
    {
        private const int TRY_COUNT_MAX = 20;
        [SerializeField] private PoolTag _poolTag;
        [SerializeField] private Vector2 _size;
        [SerializeField] private int _countLimit; // 최대 소환 수
        private int _spawnedCount; // 현재 소환된 수
        private int _spawningCount; // 현재 소환중인 수
        [SerializeField] private float _delay;
        [SerializeField] private LayerMask _spawnPointMask;
        private IObjectPool<GameObject> _pool;
        

        private void Start()
        {
            _pool = PoolManager.instance.GetPool(_poolTag);
            StartCoroutine(SpawnAll());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnAll()
        {
            while (_spawnedCount + _spawningCount < _countLimit)
            {
                _spawningCount++;
                StartCoroutine(Spawn());
            }
            yield return null;
        }

        private IEnumerator Spawn()
        {           
            yield return new WaitForSeconds(_delay);

            int count = TRY_COUNT_MAX;
            while (true)
            {
                Vector2 origin = (Vector2)transform.position
                             + new Vector2(Random.Range(-_size.x / 2.0f, +_size.x / 2.0f),
                                           Random.Range(-_size.y / 2.0f, +_size.y / 2.0f));

                RaycastHit2D hit =
                    Physics2D.Raycast(origin, Vector2.down, origin.y - (transform.position.y - _size.y / 2.0f), _spawnPointMask);

                Debug.DrawLine(origin,
                               origin + Vector2.down * (origin.y - (transform.position.y - _size.y / 2.0f)),
                               hit.collider ? Color.green : Color.red,
                               1.0f);

                if (hit.collider)
                {
                    EnemyController enemy = _pool.Get().GetComponent<EnemyController>();
                    enemy.GetComponent<GameObjectPool.PooledItem>().onReturnToPool = () =>
                    {
                        _spawnedCount--;
                        _spawningCount++;
                        StartCoroutine(Spawn());
                    };

                    enemy.SetUp();
                    enemy.transform.position = hit.point;
                    break;
                }

                count--;
                if (count <= 0)
                {
                    count = TRY_COUNT_MAX;
                    yield return null;
                }
            }

            _spawningCount--;
            _spawnedCount++;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _size);
        }
    }
}