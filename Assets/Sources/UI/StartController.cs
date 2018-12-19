using UnityEngine;

namespace La.Cantina.UI
{
    public class StartController : MonoBehaviour
    {
        private const int TIME_BEFORE_START = 5;

        private int _playersCount = 0;
        private int _playersReady = 0;

        private void Update()
        {
            // TODO: Detect players plugged. Then increment _playersCount accordingly

            // TODO: Check player input A for ready and B for unready. Then increment _playersReady accordingly

            if (_playersReady == _playersCount)
            {
                // TODO: Play timer like in FoodSlotController.Update(). Then load the scene at the end
            }
        }
    }
}