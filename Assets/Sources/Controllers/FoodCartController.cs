using La.Cantina.Manager;
using La.Cantina.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace La.Cantina.Controllers
{
    public class FoodCartController : MonoBehaviour
    {
        private const int SLOT_NUMBER = 4;

        [SerializeField] private MeshRenderer[] _slotRenderers = new MeshRenderer[SLOT_NUMBER];

        private VegetableConfig[] _availableVegetables = new VegetableConfig[SLOT_NUMBER];

        private void Awake()
        {
            GameManager.instance.Initialized += OnGameInitialized;
        }

        private void OnGameInitialized(object sender, bool isReady)
        {
            int count = 0;

            while(count < _availableVegetables.Length)
            {
                RefillFood(count);

                count++;
            }
        }

        private void RefillFood(int index)
        {
            int rand    = UnityEngine.Random.Range(0, GameManager.instance.vegetableIdToConfig.Keys.Count);
            int count   = 0;

            foreach (KeyValuePair<uint, VegetableConfig> pair in GameManager.instance.vegetableIdToConfig)
            {
                if (rand == count)
                {
                    _availableVegetables[index] = pair.Value;
                    _slotRenderers[index].material = Resources.Load<Material>("Materials/" + pair.Value.name.Replace(" ", ""));

                    UnityEngine.Debug.Log("Refill cart slot " + index + " with " + pair.Value.name);
                }

                count++;
            }
        }

        public void TakeFood(int index)
        {

        }
    }
}