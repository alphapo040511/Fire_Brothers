using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiretruckMove : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float rotationSpeed = 100f;
    public float arriveDistance = 0.5f;

    public Waypoint waypoint;

    private Rigidbody rb;
    private int waypointIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoint == null)
        {
            GettingWaypoint();
            return;
        }
        Movement();
        Rotation();
        CheckWaypointArrival();
    }


    //� ������� ���������� ���� �̵� ����� ���濹��
    private void Movement()
    {
        Vector3 direction = (waypoint.transform.position - transform.position);
        direction.y = 0;

        rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);
    }

    private void Rotation()
    {
        Vector3 direction = (waypoint.transform.position - transform.position);
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));
    }

    private void GettingWaypoint()
    {
        Waypoint point = TrailsManager.instance.GetWaypoint(waypointIndex);
        
        if(point != null)
        {
            waypointIndex++;
            waypoint = point;
        }
        else
        {
            waypoint = null;
            Debug.Log("��������Ʈ�� ã�� �� �����ϴ�.");
        }

    }

    private void CheckWaypointArrival()
    {
        if(GetDistance() <= arriveDistance)
        {
            GettingWaypoint();
        }
    }

    private float GetDistance()
    {
        Vector3 direction = (waypoint.transform.position - transform.position);
        direction.y = 0;

        return direction.magnitude;
    }
}
