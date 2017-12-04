using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai
{
    private Board board;
    private int activePlayer;
    public int MAX_DEPTH = 5;
    public const int MINUS_INFINITE = -99999;
    public const int INFINITE = 99999;


    public void Play(Board _board, int actPlayer)
    {
        board = _board;
        activePlayer = actPlayer;

        ScoringSquare ss = Minimax(_board, 0);
        Move(ss);
        //evaluate_best_play(board.rows,board.columns);
        //move();
    }
    public void Move(ScoringSquare move) //Realiza su movimiento
    {
        GameController.Instance.AIEnded(move);
    }

    void ObserveBoard() 
    {
        board = new Board(GameController.Instance.rows, GameController.Instance.columns);
        int rows = GameController.Instance.rows;
        int columns = GameController.Instance.columns;

        for(int i = 0; i < rows * columns; i++) {
                //board.squares[i] = spaceText.text; // Setear squares desde board
        }

        //board.activePlayer = this.activePlayer;
        //board.zobristKeys = this.zobristKeys;
        //board.CalculateHashValue();
    }

    ScoringSquare Minimax(Board board, int depth)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        int bestMove = 0;
        int bestScore = 0;
        ScoringSquare scoringSquare; // score, movimiento
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            scoringSquare = new ScoringSquare(board.Evaluate(activePlayer), 0);
        }
        else
        {
            if (board.activePlayer == activePlayer) bestScore = MINUS_INFINITE;
            else bestScore = INFINITE;

            List<Square> possibleMoves = new List<Square>();
            possibleMoves = board.PossibleMoves();

            foreach (Square move in possibleMoves)
            {
                newBoard = board.GenerateNewBoardFromMove(move);

                // Recursividad
                scoringSquare = Minimax(newBoard, (depth + 1));
                
                // Actualizar mejor score
                if (board.activePlayer == activePlayer)
                {
                    if (scoringSquare.Score > bestScore)
                    {
                        bestScore = scoringSquare.Score;
                        bestMove = move.Index;
                        Debug.Log("mayor");
                    }
                }
                else
                {
                    if (scoringSquare.Score < bestScore)
                    {
                        bestScore = scoringSquare.Score;
                        bestMove = move.Index;
                        Debug.Log("menor");
                    }
                }
            }
            scoringSquare = new ScoringSquare(bestScore, bestMove);
        }
        return scoringSquare;
       
    }
    /*
    ScoringMove Negamax(Board board, byte depth)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        int bestMove = 0;
        int bestScore = 0;
        ScoringMove scoringMove; // score, movimiento
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            if (depth % 2 == 0)
            {
                scoringMove = new ScoringMove(board.Evaluate(activePlayer), 0);
            }
            else
            {
                scoringMove = new ScoringMove(-board.Evaluate(activePlayer), 0);
            }
        }
        else
        {
            bestScore = MINUS_INFINITE;

            int[] possibleMoves;
            possibleMoves = board.PossibleMoves();

            foreach (int move in possibleMoves)
            {
                newBoard = board.GenerateNewBoardFromMove(move);

                // Recursividad
                scoringMove = Negamax(newBoard, (byte)(depth + 1));

                int invertedScore = -scoringMove.score;

                // Actualizar mejor score
                if (invertedScore > bestScore)
                {
                    bestScore = invertedScore;
                    bestMove = move;
                }
            }
            scoringMove = new ScoringMove(bestScore, bestMove);
        }
        return scoringMove;
    }

    ScoringMove NegamaxAB(Board board, byte depth, int alfa, int beta)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        int bestMove = 0;
        int bestScore = 0;

        ScoringMove scoringMove; // score, movimiento
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            if (depth % 2 == 0)
            {
                scoringMove = new ScoringMove(board.Evaluate(activePlayer), 0);
            }
            else
            {
                scoringMove = new ScoringMove(-board.Evaluate(activePlayer), 0);
            }
        }
        else
        {
            bestScore = MINUS_INFINITE;

            int[] possibleMoves;
            possibleMoves = board.PossibleMoves();

            foreach (int move in possibleMoves)
            {
                newBoard = board.GenerateNewBoardFromMove(move);

                // Recursividad
                scoringMove = NegamaxAB(newBoard, (byte)(depth + 1), -beta, -Math.Max(alfa, bestScore));

                int invertedScore = -scoringMove.score;

                // Actualizar mejor score
                if (invertedScore > bestScore)
                {
                    bestScore = invertedScore;
                    bestMove = move;
                }
                if (bestScore >= beta)
                {
                    scoringMove = new ScoringMove(bestScore, bestMove);
                    return scoringMove;
                }
            }
            scoringMove = new ScoringMove(bestScore, bestMove);
        }
        return scoringMove;
    }

    ScoringMove AspirationSearch(Board board)
    {
        int alfa, beta;
        ScoringMove move;
        string aspirationPath = "Aspiration Path: ";

        if (previousScore != MINUS_INFINITE)
        {
            alfa = previousScore - windowRange;
            beta = previousScore + windowRange;
            while (true)
            {
                move = NegamaxAB(board, 0, alfa, beta);
                if (move.score <= alfa)
                {
                    aspirationPath += "Fail soft alfa.";
                    alfa = MINUS_INFINITE;
                }
                else if (move.score >= beta)
                {
                    aspirationPath += "Fail soft beta.";
                    beta = INFINITE;
                }
                else
                {
                    aspirationPath += "Success";
                    break;
                }
            }
        }
        else
        {
            aspirationPath += "Normal Negamax";
            move = NegamaxAB(board, 0, MINUS_INFINITE, INFINITE);
        }
        previousScore = move.score;
        aspirationPathText.text = aspirationPath;
        return move;
    }
    */
}