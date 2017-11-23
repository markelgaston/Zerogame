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
        if (lines[_row, _column].state == Line.State.idle ) 
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
                if (lines[row, column] != null) {
                    if (lines[row, column].pressed == false)
                    {
                        lines[row, column].state = Line.State.idle;
                    }
                    else if (lines[row, column].pressed == true && lines[row, column].state == Line.State.idle)
                    {
                        lines[row, column].state = Line.State.pressed;
                    }
                }
            }

        }
        
    }
    public void findSquares(string _active_player)
    {

        for (int row = 1; row < lines.GetLength(0); row+=2)
        {
            for (int column = 0; column < lines.GetLength(1)-1; column++)
            {
                if (lines[row, column] != null)
                {
                    if (lines[row, column].pressed == true &&
                            lines[row-1,column].pressed == true &&
                            lines[row+1,column].pressed == true &&
                            lines[row,column+1].pressed == true)
                    {
                        lines[row, column].state = Line.State.square;
                        lines[row - 1, column].state = Line.State.square;
                        lines[row + 1, column].state = Line.State.square;
                        lines[row, column + 1].state = Line.State.square;
                        
                        if (boardElements[row, column*2 + 1].GetComponent<Text>().text == "")
                        {
                            boardElements[row, column*2 +1].GetComponent<Text>().text = _active_player;
                        }
                    }
                }
            }

        }
    }
}
