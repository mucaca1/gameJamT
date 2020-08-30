using System;
using UnityEngine;

public class GoToMIddle : MonoBehaviour
{

    private Vector3 startPosition;
    private Vector3 endPosition = new Vector3(0f, -1.49f, -3f);

    private float eTime;
    private Vector3 distance;
    private void Awake()
    {
        startPosition = this.transform.position;
    }

    private void Start()
    {
        eTime = 0;
        distance = endPosition - startPosition;
    }

    private void Update()
    {
        GoToPos(0.5f);
    }

    private void GoToPos(float time)
    {
        if (eTime > time)
            return;
        eTime += Time.deltaTime;
        
        float percentage = (eTime / time);
        
        transform.position = new Vector3(startPosition.x + (distance.x * percentage),
            startPosition.y + (distance.y * percentage), endPosition.z);
    }
}