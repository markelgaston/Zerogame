using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Board
{
    public int rows, columns;

    public Line[,] lines;
    public GameObject[,] boardElements;

    public string activePlayer;

    public Board(int _rows, int _columns)
    {
        rows = _rows;
        columns = _columns;
    }

    public void updateColours(string _active_player)
    {
        //lineEvaluation();
        findSquares(_active_player);

        for (int row = 0; row < lines.GetLength(0); row++)
        {
            for (int column = 0; column < lines.GetLength(1); column++)
            {
                if (lines[row, column] != null)
                {
                    changeColour(row, column);
                }
            }

        }

    }
    public void changeColour(int _row, int _column) //aztualiza los colores
    {
        if (lines[_row, _column].state == Line.State.idle)
        {
            lines[_row, _column].gameObject.GetComponent<Image>().color = Color.green;
        }
        if (lines[_row, _column].state == Line.State.pressed)
        {
            lines[_row, _column].gameObject.GetComponent<Image>().color = Color.red;
        }
        if (lines[_row, _column].state == Line.State.square)
        {
            lines[_row, _column].gameObject.GetComponent<Image>().color = Color.magenta;
        }
    }
    public void lineEvaluation() //SIN TERMINAR
    {
        for (int row = 0; row < lines.GetLength(0); row++)
        {
            for (int column = 0; column < lines.GetLength(1); column++)
            {
                if (lines[row, column] != null)
                {
                    if (lines[row, column].state == Line.State.idle)
                    {
                        lines[row, column].state = Line.State.idle;
                    }
                    else if (lines[row, column].state == Line.State.pressed || lines[row, column].state == Line.State.square)
                    {
                        lines[row, column].state = Line.State.pressed;
                    }
                }
            }

        }

    }
    public void findSquares(string _active_player)
    {

        for (int row = 1; row < lines.GetLength(0); row += 2)
        {
            for (int column = 0; column < lines.GetLength(1) - 1; column++)
            {
                Debug.Log(lines[row, column].name);
                if (lines[row, column] != null)
                {
                    if ((lines[row, column].state == Line.State.pressed || lines[row, column].state == Line.State.square) &&
                            (lines[row - 1, column].state == Line.State.pressed || lines[row - 1, column].state == Line.State.square) &&
                            (lines[row + 1, column].state == Line.State.pressed || lines[row + 1, column].state == Line.State.square) &&
                            (lines[row, column + 1].state == Line.State.pressed || lines[row, column + 1].state == Line.State.square))
                    {
                        lines[row, column].state = Line.State.square;
                        lines[row - 1, column].state = Line.State.square;
                        lines[row + 1, column].state = Line.State.square;
                        lines[row, column + 1].state = Line.State.square;

                        if (boardElements[row, column * 2 + 1].GetComponent<Text>().text == "")
                        {
                            boardElements[row, column * 2 + 1].GetComponent<Text>().text = _active_player;
                        }
                    }
                }
            }

        }
    }

    /// <summary>
    /// Comprueba si están todas las casillas marcadas
    /// </summary>
    /// <returns></returns>
    string Opponent(string player)
    {
        if (player == "Ai")
        {
            return "Player";
        }
        else
        {
            return "Ai";
        }
    }

    /// <summary>
    /// Comprueba si están todas las casillas marcadas
    /// </summary>
    /// <returns></returns>
    public bool IsEndOfGame()
    {
        for (int row = 0; row < lines.GetLength(0); row++)
        {
            for (int column = 0; column < lines.GetLength(1); column++)
            {
                if (lines[row, column] != null)
                    if (lines[row, column].state == Line.State.idle)
                        return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Devuelve un valor del tablero
    /// </summary>
    /// <returns></returns>
    public int Evaluate(string activePlayer)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Todas los posibles movimientos de un estado del tablero
    /// </summary>
    /// <returns></returns>
    public Scoringmove.Move[] PossibleMoves()
    {
        Scoringmove.Move[] moves;
        int count = 0;

        for (int row = 0; row < lines.GetLength(0); row++)
        {
            for (int column = 0; column < lines.GetLength(1); column++)
            {
                if (lines[row, column] != null)
                    if (lines[row, column].state == Line.State.idle)
                        count++;
            }
        }
        moves = new Scoringmove.Move[count];
        count = 0;
        for (int row = 0; row < lines.GetLength(0); row++)
        {
            for (int column = 0; column < lines.GetLength(1); column++)
            {
                if (lines[row, column] != null)
                {
                    if (lines[row, column].state == Line.State.idle)
                    {
                        moves[count] = new Scoringmove.Move() { row = row, column = column };
                    }
                }
            }
        }
        return moves;
    }

    /// <summary>
    /// Crea un nuevo tablero a partir de un movimiento
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public Board GenerateNewBoardFromMove(Scoringmove.Move move)
    {
        Board newBoard = DuplicateBoard();
        newBoard.lines[move.row, move.column].state = Line.State.pressed;
        newBoard.activePlayer = Opponent(newBoard.activePlayer);
        return newBoard;
    }

    private Board DuplicateBoard()
    {
        Board newBoard = new Board(rows, columns);
        for (byte row = 0; row < rows; row++)
        {
            for (byte column = 0; column < columns; column++)
            {
                newBoard.lines[row, column] = this.lines[row, column];
            }
        }
        newBoard.activePlayer = this.activePlayer;
        return newBoard;
    }
}
