using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packable : MonoBehaviour
{
    private Vector3 setPosition;
    private Quaternion setRotation;
    [SerializeField] private int selectedOrientationIndex;
    [SerializeField] private GameObject[] orientation;

    // Start is called before the first frame update
    void Start()
    {
        setPosition = transform.position;
        setRotation = transform.rotation;
    }

    public void ReturnToSetState() {
        transform.position = setPosition;
        transform.rotation = setRotation;
    }

    public void UpdateSetState(Vector3 position, Quaternion rotation) {
        setPosition = position;
        setRotation = rotation;
        transform.position = setPosition;
        transform.rotation = setRotation;
    }
    public void ChangeOrientation() {
        orientation[selectedOrientationIndex].SetActive(false);
        selectedOrientationIndex++;
        if (selectedOrientationIndex >= orientation.Length) {
            selectedOrientationIndex = 0;
        }

        orientation[selectedOrientationIndex].SetActive(true);
    }

    public bool AttemptToClaimTiles() {
        return true;
    }
}
