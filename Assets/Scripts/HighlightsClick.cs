using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightsClick : MonoBehaviour
{
    void OnMouseDown()
    {
        Board.instance.tileClicked(this, null);
    }
}
