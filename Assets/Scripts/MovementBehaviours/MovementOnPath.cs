using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementOnPath : MonoBehaviour
{
    public float spacing = 5f;
    public float resolution = 1;
    private List<Vector3> discrete_path;
    public float speed = 5f;
    private int i=0;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] points = FindObjectOfType<PathCreator>().path.CalculateEvenlySpacedPoints(spacing, resolution);
        discrete_path = new List<Vector3>();
        foreach (Vector3 p in points)
        {
            discrete_path.Add(p);
        }

        StartCoroutine(move(0.5f));
    }

    void FixedUpdate()
    { 
         Vector3 destination = discrete_path[i];
         Vector3 verticalAdj = new Vector3(destination.x, transform.position.y, destination.z);
            
         transform.LookAt(verticalAdj);
         Rigidbody rb = GetComponent<Rigidbody>();
         rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    IEnumerator move(float waitTime)
    {
        var wait = new WaitForSeconds(waitTime);

        while (true)
        {
            /*Vector3 destination = discrete_path[i];
            Vector3 verticalAdj = new Vector3(destination.x, transform.position.y, destination.z);

            transform.LookAt(verticalAdj);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);*/
            i++;
            Debug.Log("Valore i: " + i);
            if (i > discrete_path.Count-1)
            {
                i = 0;
            }

            yield return wait;
        }


    }
}
