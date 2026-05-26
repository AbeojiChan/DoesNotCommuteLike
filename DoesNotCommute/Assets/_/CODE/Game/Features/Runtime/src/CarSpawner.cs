using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    #region Publics
    #endregion


    #region Unity API

    public void Start()
    {
        SpawnNextCar();
    }

    #endregion


    #region Main API

    public void SpawnNextCar()
    {
       
        if (_currentSequenceIndex >= m_spawnSequence.Length)
        {
            Debug.Log(" Level finished.");
            
            return;
        }

        
        SpawnData currentData = m_spawnSequence[_currentSequenceIndex];

        
        if (currentData.carPrefab == null || currentData.spawnPoint == null)
        {
            Debug.LogWarning($"SpawnManager: index missing data {_currentSequenceIndex} you fat head");
            return;
        }

        if (m_gameManager == null)
        {
            Debug.LogError("SpawnManager: There's no Game Manager you dummy");
            return;
        }

        
        GameObject spawnedCar = Instantiate(currentData.carPrefab, currentData.spawnPoint.position, currentData.spawnPoint.rotation);
        CarControl carControl = spawnedCar.GetComponent<CarControl>();

        if (carControl != null)
        {
           
            m_gameManager.RegisterActiveCar(carControl);
            _currentSequenceIndex++;
        }
        else
        {
            Debug.LogError($"The prefab {currentData.carPrefab.name} has no component CarControl attached");
        }
    }

    #endregion


    #region Tools and Utilities

    [Serializable]
    private struct SpawnData
    {
        public GameObject carPrefab;
        public Transform spawnPoint;
    }

    #endregion


    #region Private and Protected

    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private SpawnData[] m_spawnSequence;
    private int _currentSequenceIndex = 0;

    #endregion
}