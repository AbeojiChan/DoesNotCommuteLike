using UnityEngine;

public class GoalZone : MonoBehaviour
{
    #region Publics
    #endregion


    #region Unity API

    private void OnTriggerEnter(Collider other)
    {
        DetectArrival(other);
    }
    #endregion


    #region Main API

    private void DetectArrival(Collider other)
    {
        CarControl arrivingCar = other.GetComponent<CarControl>();

        if (arrivingCar != null && m_gameManager != null)
        {
            m_gameManager.NotifyCarArrived(arrivingCar);
        }
    }
    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

    [SerializeField] private GameManager m_gameManager;

    #endregion
}
