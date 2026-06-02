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

    public void ArchiveAndDeployNext(List<MoveSnapshot> finishedCassette)
    {
        int finishedIndex = _currentSequenceIndex - 1;

        if (finishedIndex >= 0 && finishedIndex < m_spawnSequence.Length)
        {
            SpawnData finishedData = m_spawnSequence[finishedIndex];

            GhostArchive newArchive = new GhostArchive(
                finishedData.carPrefab,
                finishedData.spawnPoint,
                new List<MoveSnapshot>(finishedCassette)
            );

            _ghostArchives.Add(newArchive);
           
        }

        ClearBoard();
        SpawnNextCar();
    }

    public void SpawnNextCar()
    {
        if (_currentSequenceIndex >= m_spawnSequence.Length)
        {
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
      
            return;
        }

        if (m_gameManager == null)
        {
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
            return;
        }

        GameObject spawnedGoal = null;

        if (currentData.goalPrefab != null && currentData.goalPoint != null)
        {
            spawnedGoal = Instantiate(currentData.goalPrefab, currentData.goalPoint.position, currentData.goalPoint.rotation);
            _spawnedEntities.Add(spawnedGoal);

            GoalZone goalZone = spawnedGoal.GetComponent<GoalZone>();
            if (goalZone != null)
            {
                goalZone.Initialize(m_gameManager);
            }
        }
        if (m_uiCompass != null && spawnedCar != null && spawnedGoal != null)
        {
            m_uiCompass.SetTracking(spawnedCar.transform, spawnedGoal.transform);
        }
        else if (m_uiCompass != null)
        {
            m_uiCompass.StopTracking();
        }

        _currentSequenceIndex++;
    }

    public void RetryCurrentCar()
    {

        if (_currentSequenceIndex > 0)
        {
            _currentSequenceIndex--;
        }

        ClearBoard();
        SpawnNextCar();

    }

    public void TurnGreenLight()
    {
        foreach (GhostController ghost in _activeGhosts)
        {
            if (ghost != null) ghost.StartPlayback();
        }
    }

    private void ClearBoard()
    {
        if (_spawnedEntities == null)
        {
            _spawnedEntities = new List<GameObject>();
        }

        foreach (GameObject entity in _spawnedEntities)
        {
            if (entity != null)
            {
                Destroy(entity);
            }
        }

        _spawnedEntities.Clear();

        if (_activeGhosts != null)
        {
            _activeGhosts.Clear();
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
        public List<MoveSnapshot> cassette;

        public GhostArchive(GameObject prefab, Transform point, List<MoveSnapshot> records)
        {
            carPrefab = prefab;
            spawnPoint = point;
            cassette = records;
        }
    }

    #endregion


    #region Private and Protected

    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private CanvasCompass m_uiCompass;
    [SerializeField] private SpawnData[] m_spawnSequence;

    private int _currentSequenceIndex = 0;

    private List<GhostArchive> _ghostArchives = new List<GhostArchive>();
    private List<GameObject> _spawnedEntities = new List<GameObject>();
    private List<GhostController> _activeGhosts = new List<GhostController>();


    #endregion 
}