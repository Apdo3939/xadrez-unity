using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    [HideInInspector]
    public Moviment moviment;
    public Tile tile = new Tile();
    public bool wasMoved;
    public bool maxTeam;
    virtual protected void start()
    {
        if (transform.parent.name == "GoldPieces")
        {
            maxTeam = true;
        }
    }
    void OnMouseDown()
    {
        InputController.instance.tileClicked(this, transform.parent.GetComponent<Player>());
    }
}
