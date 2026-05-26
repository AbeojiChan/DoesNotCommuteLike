using UnityEngine;
using UnityEngine.InputSystem;

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
        TransmitCommand();
    }

    #endregion


    #region Main API

    public void RegisterActiveCar(CarControl newCar)
    {
        _activeCar = newCar;
        Debug.Log("GameManager : new target locked -> " + newCar.gameObject.name);

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
                m_carSpawner.SpawnNextCar();
            }
            else
            {
                Debug.LogWarning("GameManager : Impossible de relancer la boucle, m_carSpawner n'est pas assigné !");
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

  
    private CarControl _activeCar;

    private void TransmitCommand()
    {
        if (_activeCar == null)
            return;

        float currentSteerValue = m_steerAction.action.ReadValue<float>();
        _activeCar.SetSteeringInput(currentSteerValue);
    }

    #endregion
}