using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packable : MonoBehaviour
{
    private Vector3 setPosition;
    private Quaternion setRotation;

    // Start is called before the first frame update
    void Start()
    {
        setPosition = transform.position;
        setRotation = transform.rotation;
    }

    public void ReturnToSetState() {
        transform.position = setPosition;
        transform.rotation = setRotation;
        Debug.Log("Return to previous state:" + setPosition);
    }

    public void UpdateSetState(Vector3 position, Quaternion rotation) {
        setPosition = position;
        setRotation = rotation;
        transform.position = setPosition;
        transform.rotation = setRotation;
        Debug.Log("New location:" + setPosition);
    }
}
