using UnityEngine;
using UnityEngine.UI;

namespace Test.DataSerialization
{
    public class UserPreferencesUI : MonoBehaviour
    {
        public bool BGMOnOff
        {
            get => PlayerPrefs.GetInt("BGMOnOff", 1) == 1 ? true : false;
            set => PlayerPrefs.SetInt("BGMOnOff", value ? 1 : 0);
        }

        public float BGMVolume
        {
            get => PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            set => PlayerPrefs.SetFloat("BGMVolume", Mathf.Clamp(value, 0.0f, 1.0f));
        }

        [SerializeField] private Toggle _BGMToggle;
        [SerializeField] private Slider _BGMSlider;

        private void Awake()
        {
            _BGMToggle.isOn = BGMOnOff;
            _BGMSlider.value = BGMVolume;

            _BGMToggle.onValueChanged.AddListener(value =>
            {
                BGMOnOff = value;
            });

            _BGMSlider.onValueChanged.AddListener(value =>
            {
                BGMVolume = value;
            });
        }
    }
}