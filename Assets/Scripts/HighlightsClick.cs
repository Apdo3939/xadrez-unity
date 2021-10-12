using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightsClick : MonoBehaviour
{
    public AvailableMoves move;

    void OnMouseDown()
    {
        InputController.instance.tileClicked(this, null);
    }
}
