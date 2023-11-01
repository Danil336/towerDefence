
using System.Text.RegularExpressions;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Transform EnemyTransform;
    public GameValues GameValues;
    private GameObject[] movePoints;
    private float speed = 7.0f;
    private int currentPointIndex = 0;
    Vector3 moveDirection = new Vector3(0, 0, 0);
    Vector3 targetPosition = new Vector3(0, 0, 0);

    void Start()
    {

        speed = GameValues.enemySpeed;
        EnemyTransform = transform;

        movePoints = GameObject.FindGameObjectsWithTag("MovePoint");
        // Сортировка массива точек по числу внутри скобок
        System.Array.Sort(movePoints, ComparePointsByOrder);

        MoveToNextPoint();

        Zalupa();

    }

    void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy() 
    {
        EnemyTransform.Translate(moveDirection * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {

            currentPointIndex++;

            Zalupa();

            if (currentPointIndex >= movePoints.Length)
            {
                Destroy(gameObject);
                Events.onCastleTakeDamage?.Invoke(5); // 5 урона, если враг пропущен
            }
        }
    }

    void Zalupa() {

        if(currentPointIndex < movePoints.Length) {
            targetPosition = movePoints[currentPointIndex].transform.position;
            moveDirection = (targetPosition - transform.position).normalized;
        }   

    }


    int ComparePointsByOrder(GameObject point1, GameObject point2)
    {
        string pattern = @"\((\d+)\)$"; // Регулярное выражение для извлечения числа в скобках в конце строки
        int a = ExtractNumberFromName(point1.name, pattern);
        int b = ExtractNumberFromName(point2.name, pattern);
        return a - b;
    }

    int ExtractNumberFromName(string name, string pattern)
    {
        Match match = Regex.Match(name, pattern);
        if (match.Success)
        {
            string numberStr = match.Groups[1].Value;
            if (int.TryParse(numberStr, out int number))
            {
                return number;
            }
        }
        return 0;
    }

    void MoveToNextPoint()
    {
        Vector3 nextPoint = movePoints[currentPointIndex].transform.position;
        nextPoint.y = -transform.position.y; // Устанавливаем ту же высоту, чтобы враг не поднимался или опускался
    }
}