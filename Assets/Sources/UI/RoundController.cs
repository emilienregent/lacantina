using La.Cantina.Data;
using La.Cantina.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundController : MonoBehaviour
{
    private float _elapsedTime = 0f;
    private int countdown = 3;

    [SerializeField] private SettingsScriptableObject _settings = null;
    [SerializeField] private Image _title = null;
    [SerializeField] private Text _countdown = null;

    [SerializeField] private GameObject _rootNormal = null;
    [SerializeField] private GameObject _rootGo = null;

    [SerializeField] private Sprite[] _titles = new Sprite[3];

    private void Start()
    {
        UnityEngine.Debug.Log("Load round " + _settings.numRounds);

        _title.sprite = _titles[_settings.numRounds];

        _settings.numRounds++;

        _countdown.enabled = false;

        _rootNormal.SetActive(true);
        _rootGo.SetActive(false);
    }

    private void Update()
    {
        // Each second
        _elapsedTime += Time.deltaTime;

        if (countdown >= 0 && _elapsedTime >= 1f)
        {
            _elapsedTime = _elapsedTime % 1f;

            _countdown.enabled = true;
            _countdown.text = countdown.ToString();

            if (countdown == 0)
            {
                _rootNormal.SetActive(false);
                _rootGo.SetActive(true);
            }

            countdown--;
            
            if (countdown < 0)
            {
                OnTimerEnded();
            }
        }
    }

    private void OnTimerEnded()
    {
        SceneManager.LoadScene((int)SceneEnum.GAME);
    }
}
