using UnityEngine;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> neighbors = new List<Waypoint>();

    void Awake()
    {
        if (neighbors == null)
            neighbors = new List<Waypoint>();
    }
}