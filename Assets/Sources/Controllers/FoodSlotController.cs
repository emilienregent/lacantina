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

        [SerializeField] private Image _foodImage   = null;
        [SerializeField] private Image _refillImage = null;

        private bool            _isAvailable        = false;
        private float           _elapsedTime        = 0f;
        private VegetableConfig _vegetableConfig    = null;

        public bool isAvailable { get { return _isAvailable; } }

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

                    _foodImage.sprite = Resources.Load<Sprite>("Images/Food/food_" + _vegetableConfig.name.Replace(" ", "").ToLower());
                    _foodImage.enabled = true;

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

                _foodImage.enabled = false;

                _refillImage.fillAmount = 0f;
                _refillImage.enabled = true;

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