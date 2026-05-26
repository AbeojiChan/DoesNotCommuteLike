using UnityEngine;

public class CarControl : MonoBehaviour
{
    #region Publics

   public float m_forwardSpeed = 15f;
   public float m_turnSpeed =150f;

    #endregion


    #region Unity API

    private void Update()
    {
        ApplyMovement();
    }

    #endregion


    #region Main API

    public void StopEngine()
    {
        _isEngineOn = false;
        _currentSteer = 0f;
    }
    public void ApplyMovement()
    {
        if (!_isEngineOn)
            return;
        transform.position += transform.forward * m_forwardSpeed * Time.deltaTime;
        transform.Rotate(0f, _currentSteer * m_turnSpeed * Time.deltaTime, 0f);
    }
    public void SetSteeringInput(float steerValue)
    {
        _currentSteer = steerValue;
    }
    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

    private float _currentSteer;
    private bool _isEngineOn = true;

    #endregion
}
