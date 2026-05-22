using UnityEditor.Rendering;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    #region Publics

    public GameObject m_car;
    public float m_speed;
    public Transform spawnPosition;

    #endregion


    #region Unity API

    public void Start()

    {
        SpawnOneCar();
    }

    public void Update()
    {
        ImpulseCar();
    }
    #endregion


    #region Main API

    private void SpawnOneCar()
    {
        if (m_car == null)
        {
            Debug.Log("Careful it's empty you fat head");
            return;
        }

        _spawnedCar = Instantiate(m_car, spawnPosition.position, Quaternion.Euler(0, 90, 0));
        Rigidbody rb = _spawnedCar.GetComponent<Rigidbody>();
    }

    private void ImpulseCar()
    {
        if (_spawnedCar == null)
            return;

        _spawnedCar.transform.position += _spawnedCar.transform.forward * m_speed * Time.deltaTime;
    }

    #endregion


    #region Tools and Utilities
    #endregion


    #region Private and Protected

    private GameObject _spawnedCar;

    #endregion
}
