using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    private SpriteRenderer spriteRenderer;

    public void Init(bool isOffset) {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = isOffset ? baseColor : offsetColor;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
