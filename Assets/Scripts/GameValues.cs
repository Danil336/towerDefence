using UnityEngine;

[CreateAssetMenu(fileName = "GameValues", menuName = "ProcedureGeneration/GameValues", order = 0)]
public class GameValues : ScriptableObject {
  public float towerRange = 20.0f;
  public float towerDamage = 10.0f;
  public float arrowSpeed = 50f;
  public float shootInterval = 1.0f;
  public float enemyMaxHealth = 30.0f;
  public float enemySpeed = 7f;
  public float enemySpawnInterval = 1.5f;
  public int money = 0;
  public int mainHP = 100;
}