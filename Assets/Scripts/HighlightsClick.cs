using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightsClick : MonoBehaviour
{
    public Tile tile;

    void OnMouseDown()
    {
        InputController.instance.tileClicked(this, null);
    }
}
