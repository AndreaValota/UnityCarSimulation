using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ChaseTheRabbit : PathFollowing
{
    public string pathName = "MonzaPath";
    public float spacing = 5;
    private float resolution = 1;
    public int predictionOffset = 5;

    private Path path;
    private Vector3[] pathPoints;
    private Vector3 destination;

    public void Start()
    {
        //Find the path and compute equally spaced points on it 
        path = GameObject.Find(pathName).GetComponent<PathCreator>().path;
        pathPoints = path.CalculateEvenlySpacedPoints(spacing, resolution);
    }

    public override Vector3 GetDestination(MovementStatus status)
    {
        //Find the point on the path closest to the current position
        int min = 0;
        for (int i = 0; i < pathPoints.Length; i++)
        {
            if ((pathPoints[i] - transform.position).magnitude < (pathPoints[min] - transform.position).magnitude)
            {
                min = i;
            }

        }
        
        //Set the destination further along the path
        destination = pathPoints[(min + predictionOffset) % pathPoints.Length];
        return destination;
    }

    public void OnDrawGizmos()
    {
        //Draws the destination as a circle 
        UnityEditor.Handles.DrawSolidDisc(destination, new Vector3(0.0f, 1.0f, 0.0f), 5);
    }
}
