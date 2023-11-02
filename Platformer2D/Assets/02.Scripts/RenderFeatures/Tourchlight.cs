using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Platformer.GameElements.RenderFeature
{
    [RequireComponent(typeof(Light2D))]
    public class Tourchlight : MonoBehaviour
    {
        private Light2D _light;
        [SerializeField] private float _waveSpeed;
        [SerializeField] private float _fallOffMin = 0.5f;
        [SerializeField] private float _fallOffMax = 0.6f;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
        }

        private void Update()
        {
            _light.falloffIntensity 
                = Mathf.Clamp(Mathf.Abs(Mathf.Sin(_waveSpeed * Time.time)), _fallOffMin, _fallOffMax) ;
        }
    }
}