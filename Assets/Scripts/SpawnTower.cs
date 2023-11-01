using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTower : MonoBehaviour
{
    public GameValues GameValues;
    // Button NewTowerButton;
    [SerializeField] GameObject towerPrefab;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] float yOffset = 6.0f;
    // private void Start() {
    //     NewTowerButton = GameObject.Find("NewTowerButton").GetComponent<Button>();
    // }

    public void SpawnNewTower()
    {
        if(GameValues.money >= 25) {

            Vector3 spawnPosition = spawnPoint.transform.position + Vector3.up * yOffset;
            GameObject newTower = Instantiate(towerPrefab, spawnPosition, Quaternion.identity);

            Events.onSpendedMoney.Invoke(-25);

        }

    }
}