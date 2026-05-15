using UnityEngine;
using System.Collections.Generic;

public class HeartGraphDrawer : MonoBehaviour
{
    public WheelchairGrab wheelchairScript;

    [Header("Line Renderer")]
    public LineRenderer lineRenderer;

    [Header("ECG Settings")]
    public int resolution = 300;
    public float scrollSpeed = 2f;
    public float amplitude = 1f;

    private List<Vector3> points = new List<Vector3>();

    private float timer;
    private float beatInterval;

    private float xPos = 0;

    void Start()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.widthMultiplier = 0.03f;

        for (int i = 0; i < resolution; i++)
        {
            points.Add(new Vector3(i * 0.05f, 0, 0));
        }
    }

    void Update()
    {
        if (wheelchairScript == null) return;

        beatInterval = 60f / wheelchairScript.currentHR;

        timer += Time.deltaTime;

        if (timer >= beatInterval)
        {
            AddECGBeat();
            timer = 0;
        }
        else
        {
            AddFlatlinePoint();
        }

        DrawGraph();
    }

    void AddFlatlinePoint()
    {
        AddPoint(0);
    }

    void AddECGBeat()
    {
        // P wave
        AddPoint(0.1f);
        AddPoint(0.2f);
        AddPoint(0.1f);

        // Q
        AddPoint(-0.3f);

        // R
        AddPoint(1.2f);

        // S
        AddPoint(-0.5f);

        AddPoint(0);

        // T wave
        AddPoint(0.4f);
        AddPoint(0.2f);
        AddPoint(0);
    }

    void AddPoint(float y)
    {
        xPos += 0.05f;

        points.Add(new Vector3(xPos, y * amplitude, 0));

        if (points.Count > resolution)
        {
            points.RemoveAt(0);
        }
    }

    void DrawGraph()
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i] += Vector3.left * scrollSpeed * Time.deltaTime;
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}