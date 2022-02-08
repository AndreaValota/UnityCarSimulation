using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path {

    [SerializeField, HideInInspector]
    List<Vector3> points;
    [SerializeField, HideInInspector]
    bool isClosed;

    public Path(Vector3 centre)
    {
        points = new List<Vector3>
        {
            centre+Vector3.left,
            centre+(Vector3.left+Vector3.up)*.5f,
            centre + (Vector3.right+Vector3.down)*.5f,
            centre + Vector3.right
        };
    }

    public Vector3 this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public bool IsClosed
    {
        get
        {
            return isClosed;
        }
        set
        {
            if (isClosed != value)
            {
                isClosed = value;

				if (isClosed)
				{
                    //If the path is closed, add the control points needed to create the bezier curve between the first and last anchor point
					points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
					points.Add(points[0] * 2 - points[1]);
				}
				else
				{
					points.RemoveRange(points.Count - 2, 2);
				}
            }
        }
    }

    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }

    public int NumSegments
    {
        get
        {
            return points.Count/3;
        }
    }

    //Add a new segment and all his anchor and control points
    public void AddSegment(Vector3 anchorPos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPos) * .5f);
        points.Add(anchorPos);
    }

    public void SplitSegment(Vector3 anchorPos, int segmentIndex)
    {
        points.InsertRange(segmentIndex * 3 + 2, new Vector3[] { Vector3.zero, anchorPos, Vector3.zero });
    }

    public void DeleteSegment(int anchorIndex)
    {
        if (NumSegments > 2 || !isClosed && NumSegments > 1)
        {
            if (anchorIndex == 0)
            {
                if (isClosed)
                {
                    points[points.Count - 1] = points[2];
                }
                points.RemoveRange(0, 3);
            }
            else if (anchorIndex == points.Count - 1 && !isClosed)
            {
                points.RemoveRange(anchorIndex - 2, 3);
            }
            else
            {
                points.RemoveRange(anchorIndex - 1, 3);
            }
        }
    }

    public Vector3[] GetPointsInSegment(int i)
    {
        return new Vector3[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[LoopIndex(i * 3 + 3)] };
    }

    public void MovePoint(int i, Vector3 pos)
    { 
        Vector3 deltaMove = pos - points[i];

        points[i] = pos;


        if (i % 3 == 0)
        {
            if (i + 1 < points.Count || isClosed)
            {
                points[LoopIndex(i + 1)] += deltaMove;
            }
            if (i - 1 >= 0 || isClosed)
            {
                points[LoopIndex(i - 1)] += deltaMove;
            }
        }
        else
        {
            bool nextPointIsAnchor = (i + 1) % 3 == 0;
            int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
            int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

            if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
            {
                float dst = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                Vector3 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * dst;
            }
        }
    }

    //Set all the points on the XZ plane (set the Y value to 1)
    public void SetToXZPlane()
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = new Vector3(points[i].x, 1, points[i].z);
        }
    }

    //Calculate points on the path evenly spaced based on the spacing parameter 
    public Vector3[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector3> evenlySpacedPoints = new List<Vector3>();
        evenlySpacedPoints.Add(points[0]);
        Vector3 previousPoint = points[0];
        float dstSinceLastEvenPoint = 0;

        for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
        {
            //Estimate curve lenght and computer the number of 
            Vector3[] p = GetPointsInSegment(segmentIndex);
            float controlNetLength = Vector3.Distance(p[0], p[1]) + Vector3.Distance(p[1], p[2]) + Vector3.Distance(p[2], p[3]);
            float estimatedCurveLength = Vector3.Distance(p[0], p[3]) + controlNetLength / 2f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
            float t = 0;
            while (t <= 1)
            {
                t += 1f/divisions;
                Vector3 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                dstSinceLastEvenPoint += Vector3.Distance(previousPoint, pointOnCurve);

                while (dstSinceLastEvenPoint >= spacing)
                {
                    float overshootDst = dstSinceLastEvenPoint - spacing;
                    Vector3 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    dstSinceLastEvenPoint = overshootDst;
                    previousPoint = newEvenlySpacedPoint;
                }

                previousPoint = pointOnCurve;
            }
        }

        return evenlySpacedPoints.ToArray();
    }


    int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }

}