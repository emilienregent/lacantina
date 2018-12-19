using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class EndPlayerController : MonoBehaviour
    {
        private const string POINTS_FORMAT = "{0} PTS";

        [SerializeField] private Text   _playerNumber   = null;
        [SerializeField] private Text   _playerPoints   = null;
        [SerializeField] private Image  _background     = null;

        public void SetPlayer(int number, int points, Color color)
        {
            _playerNumber.text = number.ToString();
            _playerPoints.text  = string.Format(POINTS_FORMAT, points.ToString());
            _background.color   = color;
        }
    }
}