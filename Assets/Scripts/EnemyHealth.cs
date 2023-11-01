using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public GameValues GameValues;
    private Transform camera;
    private float maxHealth = 30f; 
    public float currentHealth; 
    public Slider healthBar;
    private bool canTakeDamage = true; 

    private void Start()
    {
        maxHealth = GameValues.enemyMaxHealth;

        camera = Camera.main.transform;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

    }

    private void LateUpdate() {
        healthBar.transform.LookAt(camera);
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {

            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }

            // фикс долбоеба через задержку
            StartCoroutine(DisableDamageForDuration(0.01f));
        }

        healthBar.value = currentHealth;
    }

    void Die()
    {

        Destroy(gameObject);
        Events.onKillEnemy?.Invoke(+20);

    }

    IEnumerator DisableDamageForDuration(float duration)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(duration);
        canTakeDamage = true;
    }
}