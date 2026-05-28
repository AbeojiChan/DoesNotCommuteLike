using UnityEngine;

public class TimePickup : MonoBehaviour
{
    #region Publics

    public float m_timeBonus = 10f;

    #endregion


    #region Unity API

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return;

        CarControl car = other.GetComponent<CarControl>();
        if (car != null)
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            if (gameManager != null && gameManager.IsActiveCar(car))
            {
                Collect(gameManager);
            }
        }
    }

    #endregion


    #region Main API

    public void ResetPickup()
    {
        _isCollected = false;
        ToggleVisuals(true);
    }

    public void CommitDestruction()
    {
        Destroy(gameObject);
    }

    #endregion


    #region Private and Protected

    private bool _isCollected = false;

    private void Collect(GameManager gameManager)
    {
        _isCollected = true;
        ToggleVisuals(false);

        gameManager.NotifyTimePickupCollected(this);
    }

    private void ToggleVisuals(bool show)
    {
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            renderer.enabled = show;
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = show;
        }
    }

    #endregion
}