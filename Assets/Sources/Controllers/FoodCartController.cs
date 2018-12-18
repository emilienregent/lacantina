using La.Cantina.Manager;
using La.Cantina.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace La.Cantina.Controllers
{
    public class FoodCartController : MonoBehaviour
    {
        private const int SLOT_NUMBER = 4;

        [SerializeField] private FoodSlotController[] _foodSlots = new FoodSlotController[SLOT_NUMBER];

        private VegetableConfig[] _availableVegetables = new VegetableConfig[SLOT_NUMBER];

        private void Awake()
        {
            if (GameManager.instance.isReady == false)
            {
                GameManager.instance.Initialized += OnGameInitialized;
            }
            else
            {
                OnGameInitialized(GameManager.instance, null);
            }
        }

        private void OnGameInitialized(object sender, EventArgs eventArgs)
        {
            int count = 0;

            while(count < _availableVegetables.Length)
            {
                _foodSlots[count].Refill();

                count++;
            }
        }

        private IEnumerator TakeAfter()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));

            int index = UnityEngine.Random.Range(0, SLOT_NUMBER);

            _foodSlots[index].Take();
        }
    }
}