using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.GameElements
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public Transform owner;
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public LayerMask targetMask;
        private LayerMask _boundMask;

        private void Awake()
        {
            _boundMask = 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Ground");
        }

        private void FixedUpdate()
        {
            Vector3 expected = transform.position + velocity * Time.fixedDeltaTime; // 다음 프레임 예상 위치
            RaycastHit2D hit // 검출해야하는 모든 마스크에 대해 이동예상위치쪽으로 RayCast 수행
                = Physics2D.Raycast(transform.position, 
                                    expected - transform.position,
                                    Vector3.Distance(transform.position, expected),
                                    _boundMask | targetMask);      
            
            // 뭔가 검출됐다면
            if (hit.collider)
            {
                int layerFlag = 1 << hit.collider.gameObject.layer;

                if ((layerFlag & _boundMask) > 0) // 맵 경계에 부딪힌건지
                    OnHitBound(hit);
                else if ((layerFlag & targetMask) > 0) // 타겟과 부딪힌건지
                    OnHitTarget(hit);
            }

            transform.position = expected; // 예상위치로 이동
        }

        protected virtual void OnHitBound(RaycastHit2D hit)
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnHitTarget(RaycastHit2D hit)
        {
            gameObject.SetActive(false);
        }
    }
}
