using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai
{

    private Board board;
    private Scoringmove scoringmove;
    private string activePlayer;
    public int MAX_DEPTH;
    public const int MINUS_INFINITE = -99999;
    public const int INFINITE = 99999;


    public void play(Board _board)
    {
        board = _board;

        //evaluate_best_play(board.rows,board.columns);
        //move();
    }
    public void move() //Realiza su movimiento
    {
        board.boardElements[scoringmove.move.row, scoringmove.move.column].GetComponent<Line>().state = Line.State.pressed;
    }

    public void evaluate_best_play(int rows, int columns) //criterio: coge el primero que encuentra sin presionar
    {
        /*Scoringmove best_play = new Scoringmove();

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if ((row + 1) % 2 != 0)//si la fila es impar
                {
                    if ((column + 1) % 2 == 0)//si la columna es par(horizontal)
                    {
                        if (board.boardElements[row,column].GetComponent<Line>().pressed==false)
                        {
                            best_play.move.row = row;
                            best_play.move.column = column;
                        }

                    }
                }
                if ((row + 1) % 2 == 0)//si la fila es par
                {
                    if ((column + 1) % 2 != 0)//si la columna es impar (vertical)
                    {
                        if (board.boardElements[row, column].GetComponent<Line>().pressed == false)
                        {
                            best_play.move.row = row;
                            best_play.move.column = column;
                        }
                    }

                }

            }
        }
        scoringmove = best_play;
        */
    }


    Scoringmove Minimax(Board board, byte depth)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        Scoringmove.Move bestMove = new Scoringmove.Move() { row = 0, column = 0 };
        int bestScore = 0;
        Scoringmove scoringMove; // score, movimiento
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            scoringMove = new Scoringmove(board.Evaluate(activePlayer), new Scoringmove.Move() { row = 0, column = 0 });
        }
        else
        {
            if (board.activePlayer == activePlayer) bestScore = MINUS_INFINITE;
            else bestScore = INFINITE;

            Scoringmove.Move[] possibleMoves;
            possibleMoves = board.PossibleMoves();

            foreach (Scoringmove.Move move in possibleMoves)
            {
                newBoard = board.GenerateNewBoardFromMove(move);

                // Recursividad
                scoringMove = Minimax(newBoard, (byte)(depth + 1));

                // Actualizar mejor score
                if (board.activePlayer == activePlayer)
                {
                    if (scoringMove.score > bestScore)
                    {
                        bestScore = scoringMove.score;
                        bestMove = move;
                    }
                }
                else
                {
                    if (scoringMove.score < bestScore)
                    {
                        bestScore = scoringMove.score;
                        bestMove = move;
                    }
                }
            }
            scoringMove = new Scoringmove(bestScore, bestMove);
        }
        return scoringMove;
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