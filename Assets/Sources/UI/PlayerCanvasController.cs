using System.Collections;
using UnityEngine;

namespace La.Cantina.UI
{
    public class PlayerCanvasController : MonoBehaviour
    {
        private const int FEEDBACK_TIMER = 1;

        private Canvas _canvas = null;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();

            _canvas.enabled = false;
        }

        public void EnableFeedback(bool infinite = false)
        {
            _canvas.enabled = true;

            if (infinite == false)
            {
                StartCoroutine("DisableFeedbackAfterDelay");
            }
        }

        public void DisableFeedback()
        {
            _canvas.enabled = false;
        }

        private IEnumerator DisableFeedbackAfterDelay()
        {
            yield return new WaitForSeconds(FEEDBACK_TIMER);

            _canvas.enabled = false;
        }
    }
}