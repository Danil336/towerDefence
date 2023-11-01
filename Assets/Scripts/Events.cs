using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Events {

  public static Action onSomeEvent;
  public static Action<int> onKillEnemy;
  public static Action<int> onCastleTakeDamage;
  public static Action<int> onSpendedMoney;

}
