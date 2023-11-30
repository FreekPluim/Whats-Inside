using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererBehaviour: MonoBehaviour
{
    LineRenderer lr;
    Vector3[] pos;

    public int points;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();

       
        Debug.Log(lr.GetPosition(0));
    }

    private void Update()
    {
        Draw();
    }

    //adjust this to change speed
    public float speed = 1f;
    //adjust this to change how high it goes
    public float amplitude = 1;
    public float frequency = 1;
    public Vector2 xLimits = new Vector2(0, 1);

    private void Draw()
    {
        float xStart = xLimits.x;
        float Tau = 2 * Mathf.PI;
        float xFinish = xLimits.y;

        lr.positionCount = points;

        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude * Mathf.Sin((Tau * frequency * x) + (Time.timeSinceLevelLoad * speed));
            lr.SetPosition(currentPoint, new Vector3(x, y, 0));
        }
    }
}
