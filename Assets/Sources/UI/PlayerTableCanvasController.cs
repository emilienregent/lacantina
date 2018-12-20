using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace La.Cantina.UI
{
    public class PlayerTableCanvasController : MonoBehaviour
    {
        private const int FEEDBACK_TIMER = 1;
        private const string POINTS_POSITIVE_FORMAT = "+{0}";
        private const string POINTS_NEGATIVE_FORMAT = "-{0}";

        [SerializeField] private Canvas _canvas = null;
        [SerializeField] private Text _points = null;

        public void UpdateScore(int points, bool positive = true)
        {
            _canvas.enabled = true;
            _points.text = positive ? string.Format(POINTS_POSITIVE_FORMAT, points) : string.Format(POINTS_NEGATIVE_FORMAT, points);
        }

        private IEnumerator DisableScoreAfterDelay()
        {
            yield return new WaitForSeconds(FEEDBACK_TIMER);

            DisableScore();
        }

        public void DisableScore()
        {
            _canvas.enabled = false;
        }
    }
}