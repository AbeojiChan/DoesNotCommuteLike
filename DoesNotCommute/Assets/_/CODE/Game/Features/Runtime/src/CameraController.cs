using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Publics

    public Vector3 m_offset = new Vector3(0f, 15f, -10f);
    public float m_smoothSpeed = 5f;

    #endregion


    #region Unity API

    private void LateUpdate()
    {
        FollowTarget();
    }

    #endregion


    #region Main API

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;

        if (_target != null)
        {
            transform.position = _target.position + m_offset;
        }
    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

    private Transform _target;

    private void FollowTarget()
    {
        if (_target == null)
            return;

        Vector3 desiredPosition = _target.position + m_offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, m_smoothSpeed * Time.deltaTime);

    }

    #endregion
}