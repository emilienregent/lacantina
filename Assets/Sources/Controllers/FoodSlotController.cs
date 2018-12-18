using La.Cantina.Manager;
using La.Cantina.Types;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace La.Cantina.Controllers
{
    public class FoodSlotController : MonoBehaviour
    {
        private static float TIME_BEFORE_REFILL = 3f;

        [SerializeField] private Image _refillImage  = null;

        private bool            _isAvailable        = false;
        private MeshRenderer    _meshRenderer       = null;
        private float           _elapsedTime        = 0f;
        private VegetableConfig _vegetableConfig    = null;

        public bool isAvailable { get { return _isAvailable; } }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public VegetableConfig Refill()
        {
            int rand    = Random.Range(0, GameManager.instance.vegetableIdToConfig.Keys.Count);
            int count   = 0;

            foreach (KeyValuePair<uint, VegetableConfig> pair in GameManager.instance.vegetableIdToConfig)
            {
                if (rand == count)
                {
                    _isAvailable        = true;
                    _vegetableConfig    = pair.Value;

                    _meshRenderer.material = Resources.Load<Material>("Materials/" + _vegetableConfig.name.Replace(" ", ""));

                    break;
                }

                count++;
            }

            return _vegetableConfig;
        }

        public VegetableConfig Take()
        {
            if (_isAvailable == true)
            {
                _isAvailable = false;
                _elapsedTime = 0f;

                _refillImage.fillAmount = 0f;
                _refillImage.enabled = true;

                _meshRenderer.material = Resources.Load<Material>("Materials/Empty");

                return _vegetableConfig;
            }

            return null;
        }

        private void Update()
        {
            if (_isAvailable == false)
            {
                if (_elapsedTime >= TIME_BEFORE_REFILL)
                {
                    _elapsedTime            = TIME_BEFORE_REFILL;
                    _refillImage.enabled    = false;

                    Refill();
                }

                if (_refillImage != null)
                    _refillImage.fillAmount = _elapsedTime / TIME_BEFORE_REFILL;

                _elapsedTime += Time.deltaTime;
            }
        }
    }
}