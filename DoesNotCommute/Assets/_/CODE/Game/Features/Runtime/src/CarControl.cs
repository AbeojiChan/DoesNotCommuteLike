using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarControl : MonoBehaviour
{
    #region Publics

    public float m_forwardSpeed = 15f;
    public float m_turnSpeed = 150f;
    public float m_bounceForce = 25f;
    public float m_stunDuration = 0.3f;

    #endregion


    #region Unity API

    private void Start()
    {
        
        _rigidbody = GetComponent<Rigidbody>();
        _currentForwardSpeed = m_forwardSpeed;

    }

    private void FixedUpdate()
    {
        if (_stunTimer > 0f)
        {
            _stunTimer -= Time.fixedDeltaTime;
            return;
        }

        ApplyMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isEngineOn) return;

        Vector3 impactNormal = collision.contacts[0].normal;
        if (Mathf.Abs(impactNormal.y) > 0.5f) return;

        _stunTimer = m_stunDuration;
        _rigidbody.linearVelocity = Vector3.zero;

        Vector3 bounceDirection = impactNormal;
        bounceDirection.y = 0f;
        bounceDirection.Normalize();

        _rigidbody.AddForce(bounceDirection * m_bounceForce, ForceMode.Impulse);

        
        if (!_isDamaged)
        {
            _isDamaged = true;
            _currentForwardSpeed /= 2f;
            Debug.Log($"💥 CRASH ! Le moteur est endommagé. Vitesse réduite à : {_currentForwardSpeed}");
        }
    }

    #endregion


    #region Main API

    public void StartEngine()
    {
        _isEngineOn = true;
    }

    public void StopEngine()
    {
        _isEngineOn = false;
        _currentSteer = 0f;

        
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public void SetSteeringInput(float steerValue)
    {
        _currentSteer = steerValue;
    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

    private Rigidbody _rigidbody;
    private float _currentSteer;
    private bool _isEngineOn = false;
    private float _stunTimer = 0f;
    private float _currentForwardSpeed;
    private bool _isDamaged = false;
    private void ApplyMovement()
    {
        if (!_isEngineOn || _rigidbody == null)
            return;


        Vector3 newVelocity = transform.forward * _currentForwardSpeed;
        newVelocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = newVelocity;


        float turnAmount = _currentSteer * m_turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
    }

    #endregion
}