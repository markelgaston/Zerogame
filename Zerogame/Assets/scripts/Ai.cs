using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai
{

    private Board board;
    private Line move;
    private string activePlayer;
    public int MAX_DEPTH;
    public const int MINUS_INFINITE = -99999;
    public const int INFINITE = 99999;


    public void play(Board _board)
    {
        board = _board;

        Minimax(_board, 0);
        //evaluate_best_play(board.rows,board.columns);
        //move();
    }
    public void Move(Line move) //Realiza su movimiento
    {
        GameController.Instance.End_Turn(move);
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


    Line Minimax(Board board, byte depth)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        Line bestMove;
        int bestScore = 0;
        Line scoringMove = null; // score, movimiento
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            scoringMove = board.Evaluate(activePlayer);
        }
        else
        {
            if (board.activePlayer == activePlayer) bestScore = MINUS_INFINITE;
            else bestScore = INFINITE;

            List<Line> possibleMoves = new List<Line>();
            possibleMoves = board.PossibleMoves();

            foreach (Line move in possibleMoves)
            {
                newBoard = board.GenerateNewBoardFromMove(move);

                // Recursividad
                scoringMove = Minimax(newBoard, (byte)(depth + 1));

                // Actualizar mejor score
                if (board.activePlayer == activePlayer)
                {
                    if (scoringMove.Score > bestScore)
                    {
                        bestScore = scoringMove.Score;
                        bestMove = move;
                    }
                }
                else
                {
                    if (scoringMove.Score < bestScore)
                    {
                        bestScore = scoringMove.Score;
                        bestMove = move;
                    }
                }
            }
            return scoringMove;
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