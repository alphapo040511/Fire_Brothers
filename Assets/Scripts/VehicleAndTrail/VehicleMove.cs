using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMove : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float rotationSpeed = 100f;
    public float arriveDistance = 0.5f;

    public Waypoint waypoint;

    [SerializeField] private bool canMove = false;
    private Rigidbody rb;
    private int waypointIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waypoint == null)
        {
            GettingWaypoint();
            rb.velocity = Vector3.zero;
            return;
        }

        if (!canMove || !waypoint.isAccessible)
        {
            rb.velocity = Vector3.zero;
            return;
        }


        Movement();
        Rotation();
        CheckWaypointArrival();
    }


    private void Movement()
    {
        Vector3 direction = (waypoint.transform.position - transform.position);
        direction.y = 0;

        rb.velocity = direction.normalized * moveSpeed;
    }

    private void Rotation()
    {
        Vector3 direction = (waypoint.transform.position - transform.position);
        direction.y = 0;


        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float t = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, t));
    }

    private void GettingWaypoint()
    {
        Waypoint point = TrailsManager.instance?.GetWaypoint(waypointIndex);
        
        if(point != null)
        {
            waypointIndex++;
            waypoint = point;
        }
        else
        {
            waypoint = null;
            Debug.Log("웨이포인트를 찾을 수 없습니다.");
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

    public void OnCanMoveChanged(bool able)
    {
        canMove = able;
    }
}
