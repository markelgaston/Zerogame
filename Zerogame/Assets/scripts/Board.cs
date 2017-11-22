using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Board {
    public int rows, columns;

    public Line[,] lines;
    public GameObject[,] boardElements;

    public Board(int _rows, int _columns)
    {
        rows = _rows;
        columns = _columns;
    }
    public void updateColours(string _active_player)
    {
        lineEvaluation();
        findSquares(_active_player);

        for (int row = 1; row <= rows; row++)
        {
            for (int column = 1; column <= columns; column++)
            {
                if (row % 2 != 0)
                {
                    if (column % 2 != 0)    //cricle
                    {        
                    }
                    if (column % 2 == 0)    //horizontal
                    {
                        changeColour(Convert.ToByte(row - 1), Convert.ToByte(column - 1));
                    }
                }
                if (row % 2 == 0)
                {
                    if (column % 2 != 0)    //vertical
                    {
                        changeColour(Convert.ToByte(row - 1), Convert.ToByte(column - 1));
                    }
                    if (column % 2 == 0)
                    {
                        //nothing
                    }
                }

            }

        }
        

    }
    public void changeColour(int _row, int _column) //aztualiza los colores
    {
        if (lines[_row, _column].state == Line.State.idle ) 
        {
            boardElements[_row, _column].GetComponent<Image>().color = Color.green;
        }
        if (lines[_row, _column].state == Line.State.pressed)
        {
            boardElements[_row, _column].GetComponent<Image>().color = Color.red;
        }
        if (lines[_row, _column].state == Line.State.square)
        {
            boardElements[_row, _column].GetComponent<Image>().color = Color.magenta;
        }
    }
    public void lineEvaluation() //
    {
        for (int row = 1; row <= rows; row++)
        {
            for (int column = 1; column <= columns; column++)
            {
                if (row % 2 != 0)//uneven
                {
                    if (column % 2 != 0)//circle
                    {
                    }
                    if (column % 2 == 0)//horizontal
                    {
                        if (lines[row - 1, column - 1].pressed == false)
                        {
                            lines[row - 1, column - 1].state = Line.State.idle;
                        }
                        else if(lines[row - 1, column - 1].pressed == true && lines[row-1, column-1].state == Line.State.idle)
                        {
                            lines[row - 1, column - 1].state = Line.State.pressed;
                        }
                    }
                }
                if (row % 2 == 0)//even
                {
                    if (column % 2 != 0)//vertical
                    {
                        if (boardElements[row - 1, column - 1].GetComponent<Line>().pressed == false)
                        {
                            lines[row - 1, column - 1].state = Line.State.idle;
                        }
                        else if (boardElements[row - 1, column - 1].GetComponent<Line>().pressed == true && lines[row - 1, column - 1].state == Line.State.idle)
                        {
                            lines[row - 1, column - 1].state = Line.State.pressed;
                        }
                    }
                    if (column % 2 == 0)//even
                    {
                        //nothing
                    }
                }

            }

        }
    }
    public void findSquares(string _active_player)
    {
        for (int row = 0; row < rows-2; row++)
        {

            for (int column = 0; column < columns; column++)
            {
                if ((row+1) % 2 != 0 )//uneven
                {
                    if ((column+1) % 2 == 0 )//horizontal
                    {
                        //eveluation from square center
                        if (lines[row, column].pressed == true &&
                            lines[row + 2, column].pressed == true &&
                            lines[row + 1, column + 1].pressed == true &&
                            lines[row + 1, column - 1].pressed == true)
                        {
                            lines[row, column].state = Line.State.square;
                            lines[row + 2, column].state = Line.State.square;
                            lines[row + 1, column + 1].state = Line.State.square;
                            lines[row + 1, column - 1].state = Line.State.square;

                            if (boardElements[row + 1, column].GetComponent<Text>().text=="") {
                                boardElements[row + 1, column].GetComponent<Text>().text = _active_player;
                            }
                        }
                    }
                }

            }

        }
    }
    public void Debug_spaces()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {

            }

        }
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if ((row + 1) % 2 != 0)//si la fila es impar
                {
                    if ((column + 1) % 2 == 0)//si la columna es par(horizontal)
                    {
                        boardElements[row, column].GetComponent<Line>().state = lines[row, column].state;


                    }
                }
                if ((row + 1) % 2 == 0)//si la fila es par
                {
                    if ((column + 1) % 2 != 0)//si la columna es impar (vertical)
                    {
                        boardElements[row, column].GetComponent<Line>().state = lines[row, column].state;
                    }

                }

            }
        }
    }
}
