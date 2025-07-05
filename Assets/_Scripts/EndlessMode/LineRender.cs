using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    [SerializeField] List<Transform> points = new();
    [SerializeField] LineRenderer lr;

    public void DrawLine(Transform a, Transform b)
    {
        SetUp(a,b);
        Debug.Log("a =" + a.name);
        Debug.Log("b =" + b.GetComponent<RectTransform>().transform);
        for(int i = 0; i < points.Count; ++i)
        {
            lr.SetPosition(i, points[i].position);
        }
    }
    void SetUp(Transform a, Transform b)
    {
        points.Clear();
        points.Add(a);
        points.Add(b);
        lr.positionCount = points.Count;
    }
}
