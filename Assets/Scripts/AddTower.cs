using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTower : MonoBehaviour
{
    Rigidbody rb;
    Vector3 mousePosition;
    Collider collider;
    Transform objectTransform;
    float yOffset = 0.1f; 
    float maxHeightAboveGround = 3.0f;
    float maxDistanceToTarget = 15f; 

    private GameObject OnTowerZone;
    private GameObject CorrectPlace;
    private GameObject OnTowerZone1;
    private GameObject CorrectPlace1;

    private Transform CorrectPlaceTransform;
    private Transform CorrectPlaceTransform1;

    public GameObject attackRadiusVisualization;

    private bool isMouseDown = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        CorrectPlace = GameObject.Find("CorrectPlace");
        CorrectPlace1 = GameObject.Find("CorrectPlace1");

        CorrectPlaceTransform = CorrectPlace.transform;
        CorrectPlaceTransform1 = CorrectPlace1.transform;

        collider = GetComponent<Collider>();
        objectTransform = transform;
    }

    private void Update()
    {
        PlaceCorrect();
    }

    private Vector3 GetMousePos() 
    {
        return Camera.main.WorldToScreenPoint(objectTransform.position);
    }

    private void OnMouseDown() {
        mousePosition = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag() {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        newPosition.y = objectTransform.position.y + yOffset; // Смещение по Y

        ShowAttackRadius();

        // Проверяем максимальную высоту над землей
        RaycastHit hit;
        Ray ray = new Ray(newPosition, Vector3.down);
        if (Physics.Raycast(ray, out hit)) {
            float distanceToGround = hit.distance;
            if (distanceToGround > maxHeightAboveGround) {
                newPosition.y -= distanceToGround - maxHeightAboveGround;
            }
        }

        isMouseDown = true;
        
        Collider[] collidersInNewPosition = Physics.OverlapBox(newPosition + collider.bounds.center, collider.bounds.extents, Quaternion.identity);

        bool canMove = true;
        foreach (var col in collidersInNewPosition)
        {
            if (col != collider && col.gameObject.layer != LayerMask.NameToLayer("IgnoreCollision")) {
                canMove = false;
                break;
            }
        }

        if (canMove) {
            objectTransform.position = newPosition;
        }

        rb.isKinematic = true;
    }

    private void OnMouseUp() {
        RaycastHit hit;
        Ray ray = new Ray(objectTransform.position, Vector3.down); // Выпускаем луч вниз из текущей позиции объекта

        HideAttackRadius();

        if (Physics.Raycast(ray, out hit)) {
            Vector3 finalPosition = hit.point;
            finalPosition.y = objectTransform.position.y; // Сохраняем высоту объекта
            objectTransform.position = finalPosition; // Перемещаем объект к точке на поверхности с учетом высоты
        }

        // Проверяем коллизии с объектами на самом верху
        Vector3 objectCenter = objectTransform.position + collider.bounds.center;
        Collider[] collidersOnTop = Physics.OverlapBox(objectCenter, collider.bounds.extents, Quaternion.identity);

        float highestY = objectTransform.position.y;
        foreach (var col in collidersOnTop)
        {
            if (col != collider && col.gameObject.layer != LayerMask.NameToLayer("IgnoreCollision")) {
                if (col.bounds.max.y > highestY) {
                    highestY = col.bounds.max.y;
                }
            }
        }

        if (highestY > objectTransform.position.y) {
            Vector3 newPosition = objectTransform.position;
            newPosition.y = highestY + yOffset;
            objectTransform.position = newPosition;
        }

        rb.isKinematic = false;
        isMouseDown = false;
    }

    private void PlaceCorrect() {
        if (isMouseDown)
        {
            float distanceToTarget = Vector3.Distance(objectTransform.position, CorrectPlaceTransform.position);
            float distanceToTarget2 = Vector3.Distance(objectTransform.position, CorrectPlaceTransform1.position);

            if (distanceToTarget <= maxDistanceToTarget)
            {
                objectTransform.position = CorrectPlaceTransform.position;
            }

            if (distanceToTarget2 <= maxDistanceToTarget)
            {
                objectTransform.position = CorrectPlaceTransform1.position;
            }
        }
    }

    private void ShowAttackRadius()
    {
        if (attackRadiusVisualization != null)
        {
            attackRadiusVisualization.SetActive(true); 
        }
    }

    private void HideAttackRadius()
    {
        if (attackRadiusVisualization != null)
        {
            attackRadiusVisualization.SetActive(false); 
        }
    }
}