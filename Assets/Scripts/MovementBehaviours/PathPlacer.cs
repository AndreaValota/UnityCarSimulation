using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class mainly used to test the path and visualize the points in the scene 
public class PathPlacer : MonoBehaviour {

    public float spacing = .1f;
    public float resolution = 1;

	
	void Start () {

        //Placec spheres on the path on evenly spaced points
        Vector3[] points = FindObjectOfType<PathCreator>().path.CalculateEvenlySpacedPoints(spacing, resolution);
        foreach (Vector3 p in points)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.position = p;
            g.transform.localScale = Vector3.one * spacing * .5f;
        }
	}
	
	
}
