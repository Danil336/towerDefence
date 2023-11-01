using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public GameValues GameValues;
    private TextMeshProUGUI currentMoney;
    private int moneyValue = 0; 
    private float updateInterval = 2f;
    void Start()
    {

        moneyValue = GameValues.money;

        currentMoney = GameObject.Find("CurrentMoney").GetComponent<TextMeshProUGUI>();
        currentMoney.text = moneyValue.ToString();

        StartCoroutine(AddMoneyEveryTime(updateInterval));

        Events.onKillEnemy += ChangeMoneyInSomeCases;
        Events.onSpendedMoney += ChangeMoneyInSomeCases;

    }

    private IEnumerator AddMoneyEveryTime(float interval)
    {
        while (true)
        {
            ChangeMoney(1);

            yield return new WaitForSeconds(interval);
        }
    }

    private void ChangeMoneyInSomeCases(int amount) { // использую в другом скрипте при убийстве врага

        ChangeMoney(amount);

    }

    public void ChangeMoney(int amount) {

        moneyValue = int.Parse(currentMoney.text) + amount;
        currentMoney.text = moneyValue.ToString();

        GameValues.money = moneyValue;

    }
}
