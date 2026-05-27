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
    }

    private void OnDisable()
    {
        if (m_steerAction != null)
        {
            m_steerAction.action.Disable();
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

    #endregion


    #region Main API

    public void RegisterActiveCar(CarControl newCar)
    {
        _activeCar = newCar;
        Debug.Log("GameManager : new target locked -> " + newCar.gameObject.name);

        _isCountingDown = true;
        _currentDelayTimer = m_startDelay;

        if (m_countdownText != null)
        {
            m_countdownText.gameObject.SetActive(true);
        }

        _currentRunTime = 0f;
        _currentRunRecords = new List<SteerRecord>();
        _lastRecordedSteer = 0f;

        _currentRunRecords.Add(new SteerRecord(0f, 0f));

        if (m_cameraController != null)
        {
            m_cameraController.SetTarget(newCar.transform);
        }
    }

    public void NotifyCarArrived(CarControl arrivedCar)
    {
        if (arrivedCar == _activeCar)
        {
            Debug.Log($" Car {arrivedCar.gameObject.name} has arrived!");


            _activeCar.StopEngine();
            _activeCar = null;


            if (m_carSpawner != null)
            {
                m_carSpawner.ArchiveAndDeployNext(_currentRunRecords);
            }
            else
            {
                Debug.LogWarning("GameManager : Next car unassigned");
            }
        }
    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected


    [SerializeField] private InputActionReference m_steerAction;
    [SerializeField] private CameraController m_cameraController;
    [SerializeField] private SpawnManager m_carSpawner;
    [SerializeField] private TextMeshProUGUI m_countdownText;

    private CarControl _activeCar;
    private float _goTextTimer = 0f;
    private List<SteerRecord> _currentRunRecords = new List<SteerRecord>();
    private float _currentRunTime = 0f;
    private float _lastRecordedSteer = 0f;

    [SerializeField] private float m_startDelay = 1.5f;
    private float _currentDelayTimer = 0f;
    private bool _isCountingDown = false;

    private void TransmitCommand()
    {
        if (_activeCar == null)
            return;

        _currentRunTime += Time.deltaTime;

        float currentSteerValue = m_steerAction.action.ReadValue<float>();

        _activeCar.SetSteeringInput(currentSteerValue);

        if (currentSteerValue != _lastRecordedSteer)
        {
            _currentRunRecords.Add(new SteerRecord(_currentRunTime, currentSteerValue));
            _lastRecordedSteer = currentSteerValue;


        }
    }
    private void ProcessCountdown()
    {
        _currentDelayTimer -= Time.deltaTime;
        if (m_countdownText != null)
        {
            m_countdownText.text = Mathf.CeilToInt(_currentDelayTimer).ToString();
        }

        if (_currentDelayTimer <= 0f)
        {
            _isCountingDown = false;

            if (m_countdownText != null)
            {
                m_countdownText.text = "GO !";
                _goTextTimer = 1.0f;
            }

            _activeCar.StartEngine();

            
            _currentRunRecords.Add(new SteerRecord(0f, 0f));

            
            if (m_carSpawner != null) m_carSpawner.TurnGreenLight();
        }
    }


    #endregion

}