using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai {

    private Board board;
    private Scoringmove scoringmove;

    public void play(Board _board)
    {
        board = _board;

        evaluate_best_play(board.rows,board.columns);
        move();
    }
    public void move() //Realiza su movimiento
    {
        board.tablero[scoringmove.move.row, scoringmove.move.column].GetComponent<Spaces>().pressed = true;
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
                        if (board.tablero[row,column].GetComponent<Spaces>().pressed==false)
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
                        if (board.tablero[row, column].GetComponent<Spaces>().pressed == false)
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
}
