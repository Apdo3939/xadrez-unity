using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieSelectionState : State
{
    public override void Enter()
    {
        Debug.Log("Select piece clicked moved...");
        List<AvailableMoves> moves = Board.instance.selectedPiece.moviment.GetValidMoves();
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
        Board.instance.selectedMove = highlight.move;
        machine.ChangeTo<PieceMovementState>();
    }
    void OnReturnClicked(object sender, object args)
    {
        machine.ChangeTo<PieceSelectionState>();
    }
}
