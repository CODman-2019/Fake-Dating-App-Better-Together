using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    public bool startmoving;
    public float timeLerp;
    public float timeSinceStarted;
    public Vector2 startpos, endpos;

    // Start is called before the first frame update
    void Start()
    {
        startmoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 Lerp(Vector3 start, Vector3 end, float startTime, float lerptime = 1f)
    {
        float timeSince = Time.time - lerptime;

        float percent = timeSince / lerptime;

        var resilt = Vector3.Lerp(start, end, percent);

        return resilt;
    }
}
