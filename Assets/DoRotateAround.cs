using UnityEngine;
using System.Collections;

public class DoRotateAround : MonoBehaviour
{
    public Transform Center;
    public float radius = 2.0f, radiusSpeed = 0.5f, rotationSpeed = 80.0f;
    public Vector3 Axis = new Vector3(0, 0, 1);

    void Start()
    {
        DoRotate();
    }

    public void DoRotate()
    {
        Center = GetComponent<Fleet>().getOrbiting().transform;
        transform.position = (transform.position - Center.position).normalized * radius + Center.position;
        radius = Random.Range(radius, radius * 2);
        radiusSpeed = Random.Range(radiusSpeed, radiusSpeed * 2);
        rotationSpeed = Random.Range(rotationSpeed, rotationSpeed * 2);
        Center = GetComponent<Fleet>().getOrbiting().transform;
        transform.RotateAround(Center.position, Axis, rotationSpeed * 15f);
        Vector3 desiredPosition = (transform.position - Center.position).normalized * radius + Center.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, 15f * radiusSpeed);
    }

    void Update()
    {
        

    }
}