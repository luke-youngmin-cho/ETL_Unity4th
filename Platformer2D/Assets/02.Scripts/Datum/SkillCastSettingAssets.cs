using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Datum
{
    public class SkillCastSettingAssets : MonoBehaviour
    {
        public static SkillCastSettingAssets instance;

        public SkillCastSetting this[string name] => _skillCastSettings[name];
        private Dictionary<string, SkillCastSetting> _skillCastSettings;
        [SerializeField] private List<SkillCastSetting> _skillCastSettingList;

        private void Awake()
        {
            instance = this;

            _skillCastSettings = new Dictionary<string, SkillCastSetting>();
            foreach (var data in _skillCastSettingList)
            {
                _skillCastSettings.Add(data.name, data);
            }
            _skillCastSettingList = null;
        }
    }
}