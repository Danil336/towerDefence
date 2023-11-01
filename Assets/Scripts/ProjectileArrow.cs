using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileArrow : MonoBehaviour
{
    public GameValues GameValues;
    private Transform target;
    private Rigidbody arrowRigidbody;
    GameObject arrowComponent;
    private float arrowSpeed = 50f;
    private float damage = 5f;
    // private bool hasHit = false;

    private void Start()
    {
        damage = GameValues.towerDamage;
        arrowSpeed = GameValues.arrowSpeed;

        arrowComponent = gameObject;
        arrowRigidbody = GetComponent<Rigidbody>();

        AimArrowToEnemy();
    }

    private void Update()
    {
        if (target != null) {
            AimArrowToEnemy();
        }
    }

    private void AimArrowToEnemy() {

        Vector3 direction = (target.position - transform.position).normalized;
        arrowRigidbody.velocity = direction * arrowSpeed;

        Vector3 directionToTarget = target.position - transform.position;
        transform.forward = -directionToTarget.normalized;
        
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Обработка попадания стрелы в цель (врага)
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); 
            }

            Destroy(arrowComponent);

        } else{
            Destroy(arrowComponent);
        }
    }
}
