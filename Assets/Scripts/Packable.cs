using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Packable : MonoBehaviour
{
    private Vector3 setPosition;
    private Vector3 startPosition;
    private Quaternion setRotation;
    private Quaternion startRotation;
    [SerializeField] private int selectedOrientationIndex;
    [SerializeField] private GameObject[] orientation;
    private Transform mouseSnapTo;
    private HashSet<Vector3Int> occupiedWorldPositions = new HashSet<Vector3Int>();

    // Start is called before the first frame update
    void Start()
    {
        setPosition = transform.position;
        setRotation = transform.rotation;
        startPosition = transform.position;
        startRotation = transform.rotation;
        mouseSnapTo = orientation[selectedOrientationIndex].transform.Find("MouseSnapTo").transform;
        UpdateOccupiedWorldPositions();
    }

    public void ReturnToStartState() {
        UpdateSetState(startPosition, startRotation);
        UpdateOccupiedWorldPositions();
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
        UpdateOccupiedWorldPositions();
    }
    public void ChangeOrientation() {
        orientation[selectedOrientationIndex].SetActive(false);
        selectedOrientationIndex++;
        if (selectedOrientationIndex >= orientation.Length) {
            selectedOrientationIndex = 0;
        }

        orientation[selectedOrientationIndex].SetActive(true);
        mouseSnapTo = orientation[selectedOrientationIndex].transform.Find("MouseSnapTo").transform;
        UpdateOccupiedWorldPositions();
    }

    public GameObject GetOrientation() {
        return orientation[selectedOrientationIndex];
    }

    public Tilemap GetTilemap() {
        return GetOrientation().GetComponent<Tilemap>();
    }

    public Vector3 GetMouseSnapTo() {
        return mouseSnapTo.localPosition;
    }

    public void UpdateOccupiedWorldPositions() {
        occupiedWorldPositions.Clear();

        foreach (Vector3Int packablePosition in GetTilemap().cellBounds.allPositionsWithin) {
            if (GetTilemap().GetTile(packablePosition)) {
                Vector3 worldPos = GetTilemap().CellToWorld(packablePosition);
                Vector3Int worldCellPos = GetTilemap().layoutGrid.WorldToCell(worldPos);
                occupiedWorldPositions.Add(worldCellPos);
            }
        }
    }

    public HashSet<Vector3Int> GetOccupiedWorldPositions() {
        return occupiedWorldPositions;
    }

    public bool ContainsAny(HashSet<Vector3Int> positions) {
        foreach(Vector3Int position in positions) {
            if (occupiedWorldPositions.Contains(position)) {
                return true;
            }
        }
        return false;
    }

    public void Rotate(bool clockwise) {
        
    }
}
