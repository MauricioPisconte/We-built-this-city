using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] buildings;
    
    public void InstantiateBuilding()
    {
        Instantiate(buildings[Random.Range(0, buildings.Length)], transform.position, Quaternion.identity);
    }
}
