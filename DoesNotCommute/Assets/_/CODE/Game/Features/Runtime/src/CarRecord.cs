using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CarControl))]
public class GhostController : MonoBehaviour
{
    #region Publics
    #endregion


    #region Unity API

    private void Update()
    {
        PlayCassette();
    }

    #endregion


    #region Main API

  
    public void InitializeGhost(List<SteerRecord> records)
    {
        _cassette = records;
        _currentIndex = 0;
        _currentPlaybackTime = 0f;
        _carControl = GetComponent<CarControl>();

        if (_cassette != null && _cassette.Count > 0)
        {
            _carControl.SetSteeringInput(_cassette[0].steerValue);
        }
    }

    public void StartPlayback()
    {
        _isWaitingForGreenLight = false;

        if (_carControl != null)
        {
            _carControl.StartEngine();
        }
    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

    private CarControl _carControl;
    private List<SteerRecord> _cassette;
    private int _currentIndex = 0;
    private float _currentPlaybackTime = 0f;
    private bool _isWaitingForGreenLight = true;

    private void PlayCassette()
    {
        if (_isWaitingForGreenLight || _cassette == null || _carControl == null || _currentIndex >= _cassette.Count)
            return;

        if (_cassette == null || _carControl == null || _currentIndex >= _cassette.Count)
            return;

        _currentPlaybackTime += Time.deltaTime;

        if (_currentIndex + 1 < _cassette.Count)
        {
            if (_currentPlaybackTime >= _cassette[_currentIndex + 1].timestamp)
            {
          
                _currentIndex++;

                _carControl.SetSteeringInput(_cassette[_currentIndex].steerValue);
            }
        }
        else
        {

            _carControl.StopEngine();
            
            _cassette = null;
        }
    }

    #endregion
}