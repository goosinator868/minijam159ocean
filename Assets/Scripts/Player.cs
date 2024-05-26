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
            Debug.Log(targetObject.GetComponent<Packable>());

            if (targetObject && targetObject.GetComponentInParent<Packable>()) {
                selectedObject = targetObject.transform.parent.gameObject;
                selectedPackable = selectedObject.GetComponentInParent<Packable>();
                offset = selectedObject.transform.position - mousePosition;
            }

            Collider2D gridObject = Physics2D.OverlapPoint(mousePosition, gridLayerMask);
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
            if (gridObject) {
                Vector3 tilePosition = new Vector3((int) Math.Round(mousePosition.x), (int) Math.Round(mousePosition.y), mousePosition.z - mousePosition.z);
                // TileBase tile = gridObject.GetComponent<Tilemap>().GetTile(tilePosition);
                selectedPackable.UpdateSetState(tilePosition, selectedObject.transform.rotation);
            } else {
                selectedPackable.ReturnToSetState();
            }

            selectedObject = null;
            selectedPackable = null;
        }

    }

}
