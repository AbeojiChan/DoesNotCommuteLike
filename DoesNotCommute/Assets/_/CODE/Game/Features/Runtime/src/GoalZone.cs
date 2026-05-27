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

  
    public void Initialize(GameManager gm)
    {
        _gameManager = gm;
    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

   
    private GameManager _gameManager;

    private void DetectArrival(Collider other)
    {
        CarControl arrivingCar = other.GetComponent<CarControl>();

        if (arrivingCar != null && _gameManager != null)
        {
            _gameManager.NotifyCarArrived(arrivingCar);
        }
    }

    #endregion
}