using UnityEngine;


[DisallowMultipleComponent]
public class PredictivePathFollowing : PathFollowing
{
    public string pathName = "MonzaPath";
    public float spacing = 5;
    public float resolution = 1;
    public int predictionOffset = 5;
    public float predictionTime = 0.1f;
    public float pathRadius = 3f;

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
        //Compute the future position of the agent
        Vector3 prediction = transform.position + (status.linearSpeed * status.movementDirection * predictionTime);
        
        //Find the point on the path closest to the prediction
        int min = 0;
        for (int i = 0; i < pathPoints.Length; i++)
        {
            if ((pathPoints[i] - prediction).magnitude < (pathPoints[min] - prediction).magnitude)
            {
                min = i;
            }

        }
        
        //If the prediction is off the path (+ pathRadius) the new destination is put back on the path plus an offset otherwise the current prediction is a valid destination
        if ((pathPoints[min] - prediction).magnitude < pathRadius && !Mathf.Approximately(status.linearSpeed,0f))
        {
            destination = prediction;
        }
        else
        {
            destination = pathPoints[(min + predictionOffset) % pathPoints.Length];
        }

        return destination;
    }

    public void OnDrawGizmos()
    {
        //Draws the destination as a circle 
        UnityEditor.Handles.DrawSolidDisc(destination, new Vector3(0.0f, 1.0f, 0.0f), 5);
    }
}
