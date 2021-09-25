using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    [HideInInspector]
    public Moviment moviment;
    public Tile tile = new Tile();
    public bool wasMoved;
    void OnMouseDown()
    {
        Board.instance.tileClicked(this, transform.parent.GetComponent<Player>());
    }
}
