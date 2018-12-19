using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace La.Cantina.UI
{
    public class ChildCanvasController : MonoBehaviour
    {
        private const int FEEDBACK_TIMER = 1;

        private Canvas _canvas = null;
        [SerializeField] private GameObject _slider     = null;
        [SerializeField] private GameObject _incident   = null;
        [SerializeField] private Image      _icon       = null;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();

            _slider.SetActive(false);
            _incident.SetActive(false);
        }

        public void EnableTimer(bool enabled)
        {
            _slider.SetActive(enabled);
        }

        public void EnableFeedback(string path, bool infinite = false)
        {
            _icon.sprite = Resources.Load<Sprite>("Images/" + path);
            _canvas.enabled = true;
            _slider.SetActive(false);
            _incident.SetActive(true);

            if (infinite == false)
            {
                StartCoroutine("DisableFeedbackAfterDelay");
            }
        }

        public void DisableFeedback()
        {
            _slider.SetActive(true);
            _incident.SetActive(false);
        }

        private IEnumerator DisableFeedbackAfterDelay()
        {
            yield return new WaitForSeconds(FEEDBACK_TIMER);

            DisableFeedback();
        }
    }
}