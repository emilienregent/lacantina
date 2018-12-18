using La.Cantina.Types;
using UnityEngine;
using UnityEngine.UI;

namespace La.Cantina.Controllers
{
    public class FoodSlotController : MonoBehaviour
    {
        private static float TIME_BEFORE_REFILL = 3f;

        [SerializeField] private Image _refillImage  = null;

        private bool            _isAvailable    = false;
        private MeshRenderer    _meshRenderer   = null;
        private float           _elapsedTime    = 0f;

        public bool isAvailable { get { return _isAvailable; } }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Refill(VegetableConfig vegetableConfig)
        {
            _isAvailable = true;

            _meshRenderer.material = Resources.Load<Material>("Materials/" + vegetableConfig.name.Replace(" ", ""));
        }

        public void Take()
        {
            _isAvailable    = false;
            _elapsedTime    = 0f;

            _refillImage.fillAmount   = 0f;
            _refillImage.enabled      = true;
        }

        private void Update()
        {
            if (_isAvailable == false)
            {
                if (_elapsedTime >= TIME_BEFORE_REFILL)
                {
                    _elapsedTime            = TIME_BEFORE_REFILL;
                    _isAvailable            = true;
                    _refillImage.enabled    = false;
                }

                if (_refillImage != null)
                    _refillImage.fillAmount = _elapsedTime / TIME_BEFORE_REFILL;

                _elapsedTime += Time.deltaTime;
            }
        }
    }
}