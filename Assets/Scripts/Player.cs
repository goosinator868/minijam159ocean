using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    private GameObject selectedObject;
    private Packable selectedPackable;
    private Vector3 offset;
    [SerializeField] private LayerMask packableLayerMask;
    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private Tilemap bagTilemap;
    [SerializeField] private Grid grid;
    public GameObject[] packableObjects;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Pick up object
        if (Input.GetMouseButtonDown(0)) {

            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition, packableLayerMask);

            if (targetObject && targetObject.GetComponentInParent<Packable>()) {
                selectedObject = targetObject.transform.parent.gameObject;
                selectedPackable = selectedObject.GetComponentInParent<Packable>();
                Vector3 mouseSnapTo = selectedPackable.GetMouseSnapTo();
                offset = new Vector3(0 - mouseSnapTo.x, 0 - mouseSnapTo.y, mouseSnapTo.z - mousePosition.z); //selectedObject.transform.position - mousePosition;
            }
        }

        // Move selected object
        if (selectedObject) {
            selectedObject.transform.position = mousePosition + offset;
            if (Input.GetKeyDown(KeyCode.Space)) {
                selectedPackable.ChangeOrientation();
            }
        }
        
        // Put down object
        if (Input.GetMouseButtonUp(0) && selectedObject) {
            Collider2D gridObject = Physics2D.OverlapPoint(mousePosition, gridLayerMask);
            Vector3 worldPosition = new Vector3((float) Math.Round(mousePosition.x + offset.x), (float) Math.Round(mousePosition.y + offset.y), (float) Math.Round(mousePosition.y + offset.y));
            
            bool overlaps = false;
            Tilemap packableTilemap = selectedPackable.GetTilemap();
            HashSet<Vector3Int> packableWorldCellPositions = new HashSet<Vector3Int>();
            
            foreach (Vector3Int packablePosition in packableTilemap.cellBounds.allPositionsWithin) {
                if (packableTilemap.GetTile(packablePosition)) {
                    Vector3 worldPackablePosition = packableTilemap.CellToWorld(packablePosition);
                    Vector3Int worldPackableCellPosition = new Vector3Int((int) Math.Round(worldPackablePosition.x), (int) Math.Round(worldPackablePosition.y), (int) Math.Round(worldPackablePosition.z));
                    packableWorldCellPositions.Add(worldPackableCellPosition);
                }
            }

            foreach (GameObject packableObject in packableObjects) {
                Packable otherPackable = packableObject.GetComponent<Packable>();
                overlaps = otherPackable.ContainsAny(packableWorldCellPositions);
                Debug.Log(overlaps);
                if (overlaps) {
                    break;
                }
            }

            if (overlaps) {
                selectedPackable.ReturnToSetState();

                selectedObject = null;
                selectedPackable = null;
                return;
            }

            // Remove from bag
            if (!gridObject) {
                selectedPackable.UpdateSetState(worldPosition, selectedObject.transform.rotation);
                                    
                selectedObject = null;
                selectedPackable = null;
                return;
            }

            // ** Check if valid location in bag
            // Populate set of packable; if set is fully emptied, the location is valid
            packableWorldCellPositions.Clear();
            foreach (Vector3Int packablePosition in packableTilemap.cellBounds.allPositionsWithin) {
                if (packableTilemap.GetTile(packablePosition)) {
                    Vector3 worldPackablePosition = packableTilemap.CellToWorld(packablePosition);
                    Vector3Int worldPackableCellPosition = new Vector3Int((int) Math.Round(worldPackablePosition.x), (int) Math.Round(worldPackablePosition.y), (int) Math.Round(worldPackablePosition.z));
                    // Debug.Log("world: " + worldPackablePosition + " cell: " + worldPackableCellPosition);
                    packableWorldCellPositions.Add(worldPackableCellPosition);
                }
            }

            bool isInBounds = false;
            foreach (Vector3Int bagPosition in bagTilemap.cellBounds.allPositionsWithin) {
                Vector3Int worldBagCellPosition = grid.WorldToCell(bagTilemap.CellToWorld(bagPosition));
                // Check that bag tile is valid
                if (bagTilemap.GetTile(bagPosition)) {
                    foreach (Vector3Int packablePosition in packableWorldCellPositions) {
                        // Get world cell location of packable object
                        if (packablePosition.Equals(worldBagCellPosition)) {
                            // Remove if found
                            packableWorldCellPositions.Remove(packablePosition);
                            break;
                        }
                    }
                    // Removed every spot
                    if (packableWorldCellPositions.Count == 0) {
                        isInBounds = true;
                        break;
                    }
                }
            }

            if (!isInBounds) {
                selectedPackable.ReturnToSetState();

                selectedObject = null;
                selectedPackable = null;
                return;
            }

            selectedPackable.UpdateSetState(worldPosition, selectedObject.transform.rotation);

            selectedObject = null;
            selectedPackable = null;
            return;
            
        }

    }

}
