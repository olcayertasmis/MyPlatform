using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointFollower : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    private int currentWayPointIndex = 0;

    [SerializeField] private float speed = 2f;

    private void Update()
    {
        PlatformMove();
    }

    private void PlatformMove()
    {
        var way = waypoints[currentWayPointIndex].position;

        if (Vector2.Distance(way, transform.position) < 1f)
        {
            currentWayPointIndex++;
            if (currentWayPointIndex >= waypoints.Length)
            {
                currentWayPointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, way, Time.deltaTime * speed);
    }
}