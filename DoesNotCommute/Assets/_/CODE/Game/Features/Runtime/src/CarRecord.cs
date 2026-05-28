using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class GhostController : MonoBehaviour
{
    #region Publics
    #endregion


    #region Unity API

    private void FixedUpdate()
    {
      
        if (!_isPlaying || _cassette == null || _currentIndex >= _cassette.Count)
            return;

     
        transform.position = _cassette[_currentIndex].position;
        transform.rotation = _cassette[_currentIndex].rotation;

        _currentIndex++;
    }

    #endregion


    #region Main API

    public void InitializeGhost(List<MoveSnapshot> records)
    {
        _cassette = records;
        _currentIndex = 0;
        _isPlaying = false;

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody != null)
        {
           
            _rigidbody.isKinematic = true;
        }

        if (_cassette != null && _cassette.Count > 0)
        {
            transform.position = _cassette[0].position;
            transform.rotation = _cassette[0].rotation;
        }
    }

    public void StartPlayback()
    {
        _isPlaying = true;
    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

    private List<MoveSnapshot> _cassette;
    private int _currentIndex = 0;
    private bool _isPlaying = false;
    private Rigidbody _rigidbody;

    #endregion
}