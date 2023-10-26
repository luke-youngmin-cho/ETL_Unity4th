using UnityEngine;

namespace Platformer.Datum
{
    [CreateAssetMenu(fileName = "new SkillCastSetting", menuName = "Platformer/ScriptableObjects/SkillCastSetting")]
    public class SkillCastSetting : ScriptableObject
    {
        public int targetMax; // 최대 타게팅 수
        public LayerMask targetMask; // 타겟 검출 마스크 
        public float damageGain; // 공격 계수
        public Vector2 castCenter; // 타겟 감지 형상(사각형) 범위 중심
        public Vector2 castSize; // 타겟 감지 형상 크기
        public float castDistance; // 타겟 감지 형상 빔 거리 
    }
}