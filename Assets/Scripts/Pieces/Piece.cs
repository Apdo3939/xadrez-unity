using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Tile tile = new Tile();
    void OnMouseDown()
    {
        Debug.Log("Clik na piece " + transform);
    }

    void Start()
    {
        Board.instance.AddPiece(transform.parent.name, this);
    }
}
