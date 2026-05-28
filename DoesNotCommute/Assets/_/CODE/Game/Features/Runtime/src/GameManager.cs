using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Publics
    #endregion


    #region Unity API

    private void OnEnable()
    {
        if (m_steerAction != null)
        {
            m_steerAction.action.Enable();
        }
        if (m_rewindAction != null)
        {
            m_rewindAction.action.Enable();
            m_rewindAction.action.performed += OnRewindPerformed;
        }
    }

    private void OnDisable()
    {
        if (m_steerAction != null)
        {
            m_steerAction.action.Disable();
        }
        if (m_rewindAction != null)
        {
            m_rewindAction.action.Disable();
            m_rewindAction.action.performed -= OnRewindPerformed;
        }
    }

    private void Update()
    {
        if (_isCountingDown)
        {
            ProcessCountdown();
        }
        else
        {
            TransmitCommand();

            if (_activeCar != null)
            {
                _timeSpentThisTurn += Time.deltaTime;
            }

            if (_goTextTimer > 0f)
            {
                _goTextTimer -= Time.deltaTime;
                if (_goTextTimer <= 0f && m_countdownText != null)
                {
                    m_countdownText.gameObject.SetActive(false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isCountingDown || _activeCar == null) return;
        _currentRunRecords.Add(new MoveSnapshot(_activeCar.transform.position, _activeCar.transform.rotation));
    }

    #endregion


    #region Main API

    public void RegisterActiveCar(CarControl newCar)
    {
        _activeCar = newCar;
        Debug.Log($"GameManager: New target locked -> {newCar.gameObject.name}");

        _isCountingDown = true;
        _currentDelayTimer = m_startDelay;

     
        Time.timeScale = 0f;

        if (m_countdownText != null)
        {
            m_countdownText.gameObject.SetActive(true);
        }

        _currentRunRecords = new List<MoveSnapshot>();
        _timeSpentThisTurn = 0f;
        _collectedPickupsThisTurn.Clear();

        if (m_cameraController != null)
        {
            m_cameraController.SetTarget(newCar.transform);
        }
    }

    public void NotifyCarArrived(CarControl arrivedCar)
    {
        if (arrivedCar == _activeCar)
        {
            Debug.Log($"GameManager: Car {arrivedCar.gameObject.name} has arrived.");

            _activeCar.StopEngine();
            _activeCar = null;

            foreach (TimePickup pickup in _collectedPickupsThisTurn)
            {
                if (pickup != null) pickup.CommitDestruction();
            }
            _collectedPickupsThisTurn.Clear();

            if (m_carSpawner != null)
            {
                m_carSpawner.ArchiveAndDeployNext(_currentRunRecords);
            }
            else
            {
                Debug.LogWarning("GameManager: Next car unassigned.");
            }
        }
    }

    public void TriggerManualRewind()
    {
        if (_activeCar == null && !_isCountingDown) return;

        Debug.Log("GameManager: Temporal anomaly detected. Rewinding timeline.");

        Time.timeScale = 1f;

        if (!_isCountingDown)
        {
            float timeToRefund = _timeSpentThisTurn - 1f;
            Debug.Log($"GameManager: Time elapsed: {_timeSpentThisTurn:F1}s | Refund (with penalty): {timeToRefund:F1}s");
        }

        foreach (TimePickup pickup in _collectedPickupsThisTurn)
        {
            if (pickup != null) pickup.ResetPickup();
        }
        _collectedPickupsThisTurn.Clear();

        if (_activeCar != null)
        {
            _activeCar.StopEngine();
            _activeCar = null;
        }

        _currentRunRecords.Clear();
        _isCountingDown = false;

        if (m_carSpawner != null)
        {
            m_carSpawner.RetryCurrentCar();
        }
    }

    private void TransmitCommand()
    {
        if (_activeCar == null)
            return;

        float currentSteerValue = m_steerAction.action.ReadValue<float>();
        _activeCar.SetSteeringInput(currentSteerValue);
    }

    private void OnRewindPerformed(InputAction.CallbackContext context)
    {
        TriggerManualRewind();
    }

    private void ProcessCountdown()
    {

        _currentDelayTimer -= Time.unscaledDeltaTime;

        if (m_countdownText != null)
        {
            m_countdownText.text = Mathf.CeilToInt(_currentDelayTimer).ToString();
        }

        if (_currentDelayTimer <= 0f)
        {
            _isCountingDown = false;

            Time.timeScale = 1f;

            if (m_countdownText != null)
            {
                m_countdownText.text = "GO !";
                _goTextTimer = 1.0f;
            }

            _activeCar.StartEngine();

            if (m_carSpawner != null) m_carSpawner.TurnGreenLight();
        }
    }

    public bool IsActiveCar(CarControl car)
    {
        return car == _activeCar;
    }

    public void NotifyTimePickupCollected(TimePickup pickup)
    {
        _collectedPickupsThisTurn.Add(pickup);

        Debug.Log($"GameManager: Time pickup collected. Bonus: +{pickup.m_timeBonus}s");

    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

    [SerializeField] private InputActionReference m_steerAction;
    [SerializeField] private InputActionReference m_rewindAction;
    [SerializeField] private CameraController m_cameraController;
    [SerializeField] private SpawnManager m_carSpawner;
    [SerializeField] private TextMeshProUGUI m_countdownText;
    [SerializeField] private float m_startDelay = 1.5f;

    private float _timeSpentThisTurn = 0f;
    private CarControl _activeCar;
    private float _goTextTimer = 0f;
    private List<MoveSnapshot> _currentRunRecords = new List<MoveSnapshot>();
    private List<TimePickup> _collectedPickupsThisTurn = new List<TimePickup>();
    private float _currentDelayTimer = 0f;
    private bool _isCountingDown = false;

    #endregion


}