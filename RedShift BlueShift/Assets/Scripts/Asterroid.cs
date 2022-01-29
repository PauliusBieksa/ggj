using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asterroid : MonoBehaviour
{
    Vector3 axisOfRotation;

    // Start is called before the first frame update
    void Start()
    {
        axisOfRotation = Random.insideUnitSphere * 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axisOfRotation);
    }
}
