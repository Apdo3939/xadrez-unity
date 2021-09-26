using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieSelectionState : State
{
    public override void Enter()
    {
        Debug.Log("Select piece clicked moved...");
        List<Tile> moves = Board.instance.selectedPiece.moviment.GetValidMoves();
        Highlights.instance.SelectTiles(moves);
        InputController.instance.tileClicked += OnHighLightClicked;
        InputController.instance.returnClicked += OnReturnClicked;
    }

    public override void Exit()
    {
        Highlights.instance.DeSelectTiles();
        InputController.instance.tileClicked -= OnHighLightClicked;
        InputController.instance.returnClicked -= OnReturnClicked;
    }

    void OnHighLightClicked(object sender, object args)
    {
        HighlightsClick highlight = sender as HighlightsClick;
        if (highlight == null)
        {
            return;
        }
        Vector3 v3pos = highlight.transform.position;
        Vector2Int pos = new Vector2Int((int)v3pos.x, (int)v3pos.y);
        Tile tileClicked = highlight.tile;

        Debug.Log(tileClicked.pos);
        Board.instance.selectedHighlight = highlight;
        machine.ChangeTo<PieceMovementState>();//change the next state


    }

    void OnReturnClicked(object sender, object args)
    {
        machine.ChangeTo<PieceSelectionState>();
    }
}
