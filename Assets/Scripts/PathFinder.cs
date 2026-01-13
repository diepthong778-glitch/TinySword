using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    public static List<Waypoint> FindPath(Waypoint start, Waypoint goal)
    {
        var dist = new Dictionary<Waypoint, float>();
        var prev = new Dictionary<Waypoint, Waypoint>();
        var unvisited = new List<Waypoint>();

        Waypoint[] all = Object.FindObjectsOfType<Waypoint>();

        foreach (var wp in all)
        {
            dist[wp] = Mathf.Infinity;
            prev[wp] = null;
            unvisited.Add(wp);
        }

        dist[start] = 0f;

        while (unvisited.Count > 0)
        {
            Waypoint current = GetClosest(unvisited, dist);
            unvisited.Remove(current);

            if (current == goal)
                break;

            foreach (var neighbor in current.neighbors)
            {
                float alt =
                    dist[current] +
                    Vector2.Distance(
                        current.transform.position,
                        neighbor.transform.position);

                if (alt < dist[neighbor])
                {
                    dist[neighbor] = alt;
                    prev[neighbor] = current;
                }
            }
        }

        return ReconstructPath(prev, goal);
    }

    static Waypoint GetClosest(
        List<Waypoint> nodes,
        Dictionary<Waypoint, float> dist)
    {
        Waypoint best = null;
        float bestDist = Mathf.Infinity;

        foreach (var n in nodes)
        {
            if (dist[n] < bestDist)
            {
                bestDist = dist[n];
                best = n;
            }
        }
        return best;
    }

    static List<Waypoint> ReconstructPath(
        Dictionary<Waypoint, Waypoint> prev,
        Waypoint goal)
    {
        var path = new List<Waypoint>();
        Waypoint current = goal;

        while (current != null)
        {
            path.Insert(0, current);
            current = prev[current];
        }
        return path;
    }
}
