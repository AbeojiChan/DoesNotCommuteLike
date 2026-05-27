using UnityEngine;
using System;
using System.Collections.Generic;

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

    public void ArchiveAndDeployNext(List<SteerRecord> finishedCassette)
    {
        int finishedIndex = _currentSequenceIndex - 1;

        if (finishedIndex >= 0 && finishedIndex < m_spawnSequence.Length)
        {
            SpawnData finishedData = m_spawnSequence[finishedIndex];

            GhostArchive newArchive = new GhostArchive(
                finishedData.carPrefab,
                finishedData.spawnPoint,
                new List<SteerRecord>(finishedCassette)
            );

            _ghostArchives.Add(newArchive);
            Debug.Log($"Total ghosts in memory : {_ghostArchives.Count}");
        }

        ClearBoard();
        SpawnNextCar();
    }

    public void SpawnNextCar()
    {
        if (_currentSequenceIndex >= m_spawnSequence.Length)
        {
            Debug.Log(" Level finished.");
            return;
        }

        foreach (GhostArchive archive in _ghostArchives)
        {
            GameObject ghostCar = Instantiate(archive.carPrefab, archive.spawnPoint.position, archive.spawnPoint.rotation);
            _spawnedEntities.Add(ghostCar);

            GhostController ghostController = ghostCar.GetComponent<GhostController>();

            if (ghostController != null)
            {
                _activeGhosts.Add(ghostController);
                ghostController.InitializeGhost(archive.cassette);
            }
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
        _spawnedEntities.Add(spawnedCar);

        CarControl carControl = spawnedCar.GetComponent<CarControl>();

        if (carControl != null)
        {
            m_gameManager.RegisterActiveCar(carControl);
        }
        else
        {
            Debug.LogError($"The prefab {currentData.carPrefab.name} has no component CarControl attached");
        }


        if (currentData.goalPrefab != null && currentData.goalPoint != null)
        {
            GameObject spawnedGoal = Instantiate(currentData.goalPrefab, currentData.goalPoint.position, currentData.goalPoint.rotation);
            _spawnedEntities.Add(spawnedGoal);

            GoalZone goalZone = spawnedGoal.GetComponent<GoalZone>();

            if (goalZone != null && m_gameManager != null)
            {

                goalZone.Initialize(m_gameManager);
            }
        }


        _currentSequenceIndex++;
    }

    public void TurnGreenLight()
    {
        foreach (GhostController ghost in _activeGhosts)
        {
            if (ghost != null) ghost.StartPlayback();
        }
    }

    #endregion


    #region Tools and Utilities

    [Serializable]
    private struct SpawnData
    {
        public GameObject carPrefab;
        public Transform spawnPoint;
        public GameObject goalPrefab;
        public Transform goalPoint;
    }

    [Serializable]
    private struct GhostArchive
    {
        public GameObject carPrefab;
        public Transform spawnPoint;
        public List<SteerRecord> cassette;

        public GhostArchive(GameObject prefab, Transform point, List<SteerRecord> records)
        {
            carPrefab = prefab;
            spawnPoint = point;
            cassette = records;
        }
    }

    #endregion


    #region Private and Protected

    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private SpawnData[] m_spawnSequence;

    private int _currentSequenceIndex = 0;

    private List<GhostArchive> _ghostArchives = new List<GhostArchive>();
    private List<GameObject> _spawnedEntities = new List<GameObject>();
    private List<GhostController> _activeGhosts = new List<GhostController>();

    private void ClearBoard()
    {
        foreach (GameObject entity in _spawnedEntities)
        {
            if (entity != null)
            {
                Destroy(entity);
            }
        }

        _spawnedEntities.Clear();
        _activeGhosts.Clear(); 

        Debug.Log("Clean slate");
    }

    #endregion 
}