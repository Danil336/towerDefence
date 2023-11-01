using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CastleHealth : MonoBehaviour
{
    public GameValues GameValues;
    private TextMeshProUGUI mainHealthBar;
    private int mainTowerHP = 100;

    void Start()
    {

        mainTowerHP = GameValues.mainHP;

        mainHealthBar = gameObject.GetComponent<TextMeshProUGUI>();
        mainHealthBar.text = mainTowerHP.ToString();

        Events.onCastleTakeDamage += TakeDamage;
        
    }

    private void TakeDamage(int damage) {

        mainTowerHP -= damage;
        mainHealthBar.text = mainTowerHP.ToString();

    }
}
