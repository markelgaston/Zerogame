using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai {

    private Board board;
    private Scoringmove scoringmove;
    public int MAX_DEPTH;

    public void play(Board _board)
    {
        board = _board;

        evaluate_best_play(board.rows,board.columns);
        move();
    }
    public void move() //Realiza su movimiento
    {
        board.boardElements[scoringmove.move.row, scoringmove.move.column].GetComponent<Line>().pressed = true;
    }
    public void evaluate_best_play(int rows, int columns) //criterio: coge el primero que encuentra sin presionar
    {
        Scoringmove best_play = new Scoringmove();

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
    }

    /*Scoringmove Minimax(Board board, int depth)
    {
        int bestMove = 0;
        int bestScore = 0;
        Scoringmove scoringMove;
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión, por 2 posibles motivos:
        // 1. hemos llegado a una jugada terminal.
        // 2. hemos alcanzado la máxima profundidad que nos permite nuestra inteligencia.
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            scoringMove = new Scoringmove(board.Evaluate(activePlayer), 0);
        }
        else
        {
            if (board.activePlayer == activePlayer) bestScore = MINUS_INFINITY;
            else bestScore = INFINITY;
            int[] possibleMoves;
            possibleMoves = board.PossibleMoves();
            foreach (int move in possibleMoves)
            {
                newBoard = board.GenerateNewBoardFromMove(move);
            }
            // Recursividad
            scoringMove = Minimax(newBoard, depth + 1);
            // Actualizar mejor score si obtenemos una jugada mejor.
            // Hay que comprobar si es un movimiento del contrario o nuestro, para interpretar qué es "mejor".
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
        scoringMove = new ScoringMove(bestScore, bestMove);
    }*/
 //return scoringMove;
}