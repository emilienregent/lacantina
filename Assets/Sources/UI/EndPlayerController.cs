using UnityEngine;
using UnityEngine.UI;

namespace La.Cantina.UI
{
    public class EndPlayerController : MonoBehaviour
    {
        private const string POINTS_FORMAT  = "{0} PTS";
        private const string NAME_FORMAT    = "PLAYER {0}";

        [SerializeField] private Text   _playerNumber   = null;
        [SerializeField] private Text   _playerPoints   = null;
        [SerializeField] private Image  _background     = null;

        public void SetPlayer(int number, int points, Color color)
        {
            _playerNumber.text  = string.Format(NAME_FORMAT, number.ToString());
            _playerPoints.text  = string.Format(POINTS_FORMAT, points.ToString());
            _background.color   = color;
        }

        public void DisablePlayer()
        {
            _background.enabled = false;
            _playerPoints.enabled = false;
            _playerNumber.enabled = false;
        }
    }
}