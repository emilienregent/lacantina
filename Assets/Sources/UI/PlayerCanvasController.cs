using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace La.Cantina.UI
{
    public class PlayerCanvasController : MonoBehaviour
    {
        private const int FEEDBACK_TIMER = 1;

        private Canvas _canvas = null;
        [SerializeField] private Image _icon = null;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();

            _canvas.enabled = false;
        }

        public void EnableFeedback(string path, bool infinite = false)
        {
            _icon.sprite = Resources.Load<Sprite>("Images/" + path);
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