using TMPro;
using UnityEngine;
public class SimpleTimer : MonoBehaviour
{
    #region Publics

    public TextMeshProUGUI m_timerText;

    #endregion


    #region Unity API

    private void Update()
    {
        ProcessTimer();
    }
    #endregion


    #region Main API

    public void StartTimer()
    {
        _isRunning = true;
    }
    public void StopTimer()
    {
        _isRunning  =false;
    }
    private void ProcessTimer()
    {
        if (!_isRunning)
            return;
        _setTime -= Time.deltaTime;
        UpdateUIText();
        if(_setTime <= 0f)
        {
            _setTime = 0f;
            _isRunning = false;
            TimerEnded();
        }
    }

    public void AddTime(float bonusTime)
    {
        _setTime += bonusTime;

        UpdateUIText();

    }

    private void UpdateUIText()
    {
        if (m_timerText == null) return;
        int minutes = Mathf.FloorToInt(_setTime / 60f);
        int seconds = Mathf.FloorToInt(_setTime % 60f);
        m_timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TimerEnded()
    {
        //HandleGameOver();
        Debug.Log("Time is over");
    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private float _setTime = 30f;
    private bool _isRunning = true;
    #endregion
}
