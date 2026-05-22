using System;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    #region Publics

    public GameObject m_car;
    public float m_speed;
    public Transform spawnPosition;

    #endregion


    #region Unity API

    public void Start() => SpawnOneCar();

    #endregion


    #region Main API

    private void SpawnOneCar()
    {
        if (m_car == null)
        {
            Debug.Log("Careful it's empty you fat head");
            return;
        }

        Instantiate(m_car, spawnPosition.position, Quaternion.Euler(0, 90, 0));


    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected
    #endregion
}
