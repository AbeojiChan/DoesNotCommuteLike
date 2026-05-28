using UnityEngine;

public class CanvasCompass : MonoBehaviour
{
    #region Publics
    #endregion


    #region Unity API

    private void Start()
    {
        StopTracking();
    }

    private void Update()
    {
        if (!_isActive || _playerCar == null || _targetGoal == null || m_mainCamera == null)
            return;

        Vector3 carScreenPos = m_mainCamera.WorldToScreenPoint(_playerCar.position);
        Vector3 goalScreenPos = m_mainCamera.WorldToScreenPoint(_targetGoal.position);

        Vector2 direction = new Vector2(goalScreenPos.x - carScreenPos.x, goalScreenPos.y - carScreenPos.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        m_arrowRect.rotation = Quaternion.Euler(0f, 0f, angle + m_spriteAngleOffset);
    }

    #endregion


    #region Main API

    public void SetTracking(Transform car, Transform goal)
    {
        _playerCar = car;
        _targetGoal = goal;
        _isActive = true;
        gameObject.SetActive(true);
    }

    public void StopTracking()
    {
        _isActive = false;
        _playerCar = null;
        _targetGoal = null;
        gameObject.SetActive(false);
    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

    [SerializeField] private RectTransform m_arrowRect;
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private float m_spriteAngleOffset = -90f;

    private Transform _playerCar;
    private Transform _targetGoal;
    private bool _isActive = false;

    #endregion
}