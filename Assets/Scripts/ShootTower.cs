using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTower : MonoBehaviour
{
    public GameValues GameValues;
    private Vector3 shootFromPosition;
    [SerializeField] GameObject ArrowPref;

    private GameObject closestEnemy;
    public GameObject topOfTower;
    public GameObject gun;
    public GameObject attackRadiusVisualization;
    private float detectionRadius = 20f;
    private float previousDetectionRadius = -1.0f;
    public float shootInterval = 1f;
    private float timer = 2.0f;

    private void Start() {
        
        detectionRadius = GameValues.towerRange;
        shootInterval = GameValues.shootInterval;

    }

    void Update()
    {

        FindAndShoot();

        SetNewVisualisationRadius();

    }

    void FindAndShoot() {
        if(shootFromPosition != transform.Find("Verh").Find("Stvol").Find("ShotFromPosition").position) {  // может быть подругому в зависимости от наследников
            shootFromPosition = transform.Find("Verh").Find("Stvol").Find("ShotFromPosition").position;
        }

        // Найти ближайшего противника в радиусе обнаружения
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        closestEnemy = null;
        float closestDistance = detectionRadius;
        
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestEnemy = collider.gameObject;
                    closestDistance = distance;
                }
            }
        }

        // Если есть ближайший противник, стрелять
        if (closestEnemy != null)
        {
            LookAtEnemy();
            
            timer += Time.deltaTime;
            if (timer >= shootInterval)
            {
                Shoot();
                timer = 0.0f;
            }
        }
    }

    void SetNewVisualisationRadius() {
        if (detectionRadius != previousDetectionRadius && attackRadiusVisualization != null) // изменение радиуса визуализации атаки
        {
            float newScale = (float)detectionRadius / 1.5f; // подобрал методом тыка
            attackRadiusVisualization.transform.localScale = new Vector3(newScale, attackRadiusVisualization.transform.localScale.y, newScale);
            previousDetectionRadius = detectionRadius;
        }
    }

    void Shoot()
    {
        if (ArrowPref != null && closestEnemy != null)
        {
            // Создаем стрелу на позиции shootFromPosition
            GameObject newArrow = Instantiate(ArrowPref, shootFromPosition, Quaternion.identity);

            // Устанавливаем цель (врага) для стрелы
            ProjectileArrow collisionHandler = newArrow.GetComponent<ProjectileArrow>();
            collisionHandler.SetTarget(closestEnemy.transform);
        }
    }

    void LookAtEnemy() {

        // topOfTower.transform.LookAt(closestEnemy.transform); // вся эта хуйня изза димы

        Vector3 targetDirection = closestEnemy.transform.position - topOfTower.transform.position;

        topOfTower.transform.rotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);

        Vector3 currentRotation = topOfTower.transform.rotation.eulerAngles;
        currentRotation.x = -90f;
        topOfTower.transform.rotation = Quaternion.Euler(currentRotation);

        currentRotation.z += 180f; 
        topOfTower.transform.rotation = Quaternion.Euler(currentRotation);


        // Vector3 directionToEnemy = gun.transform.position - closestEnemy.transform.position;
        // gun.transform.LookAt(gun.transform.position + directionToEnemy);

    }

    private void OnDrawGizmos() // радиус атаки в среде разработки
    {
        Gizmos.color = Color.red; 
        Vector3 positionXZ = new Vector3(transform.position.x, 8f, transform.position.z); 

        int segments = 36;
        float angleIncrement = 360f / segments; 
        float lineWidth = 0.1f;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleIncrement;
            float x1 = positionXZ.x + Mathf.Cos(Mathf.Deg2Rad * angle) * (detectionRadius - lineWidth);
            float z1 = positionXZ.z + Mathf.Sin(Mathf.Deg2Rad * angle) * (detectionRadius - lineWidth);

            float x2 = positionXZ.x + Mathf.Cos(Mathf.Deg2Rad * (angle + angleIncrement)) * (detectionRadius - lineWidth);
            float z2 = positionXZ.z + Mathf.Sin(Mathf.Deg2Rad * (angle + angleIncrement)) * (detectionRadius - lineWidth);

            Gizmos.DrawLine(new Vector3(x1, positionXZ.y, z1), new Vector3(x2, positionXZ.y, z2));
        }
    }

}
