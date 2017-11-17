using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Board {
    public int rows, columns;

    public Spaces[,] spaces;
    public GameObject[,] tablero;

    public Board(int _rows, int _columns)
    {
        rows = _rows;
        columns = _columns;
    }
    public void actualizar_colores(string _active_player)
    {
        evaluacion_spaces();
        buscar_cuadrados(_active_player);

        for (int row = 1; row <= rows; row++)
        {
            for (int column = 1; column <= columns; column++)
            {
                if (row % 2 != 0)//si la fila es impar
                {
                    if (column % 2 != 0)//si la columna es impar (circulo)
                    {        
                    }
                    if (column % 2 == 0)//si la columna es par(horizontal)
                    {
                        actualizarlo(Convert.ToByte(row - 1), Convert.ToByte(column - 1));
                    }
                }
                if (row % 2 == 0)//si la fila es par
                {
                    if (column % 2 != 0)//si la columna es impar (vertical)
                    {
                        actualizarlo(Convert.ToByte(row - 1), Convert.ToByte(column - 1));
                    }
                    if (column % 2 == 0)//si la columna es par
                    {
                        //en estas posiciones no hay nada
                    }
                }

            }

        }
        

    }
    public void actualizarlo(int _row, int _column) //aztualiza los colores
    {
        if (spaces[_row, _column].state == Spaces.State.normal ) 
        {
            tablero[_row, _column].GetComponent<Image>().color = Color.green;
        }
        if (spaces[_row, _column].state == Spaces.State.pulsado)
        {
            tablero[_row, _column].GetComponent<Image>().color = Color.red;
        }
        if (spaces[_row, _column].state == Spaces.State.cuadrado)
        {
            tablero[_row, _column].GetComponent<Image>().color = Color.magenta;
        }
    }
    public void evaluacion_spaces() //
    {
        for (int row = 1; row <= rows; row++)
        {
            for (int column = 1; column <= columns; column++)
            {
                if (row % 2 != 0)//si la fila es impar
                {
                    if (column % 2 != 0)//si la columna es impar (circulo)
                    {
                    }
                    if (column % 2 == 0)//si la columna es par(horizontal)
                    {
                        if (spaces[row - 1, column - 1].pressed == false)
                        {
                            spaces[row - 1, column - 1].state = Spaces.State.normal;
                        }
                        else if(spaces[row - 1, column - 1].pressed == true && spaces[row-1, column-1].state == Spaces.State.normal)
                        {
                            spaces[row - 1, column - 1].state = Spaces.State.pulsado;
                        }
                    }
                }
                if (row % 2 == 0)//si la fila es par
                {
                    if (column % 2 != 0)//si la columna es impar (vertical)
                    {
                        if (tablero[row - 1, column - 1].GetComponent<Spaces>().pressed == false)
                        {
                            spaces[row - 1, column - 1].state = Spaces.State.normal;
                        }
                        else if (tablero[row - 1, column - 1].GetComponent<Spaces>().pressed == true && spaces[row - 1, column - 1].state == Spaces.State.normal)
                        {
                            spaces[row - 1, column - 1].state = Spaces.State.pulsado;
                        }
                    }
                    if (column % 2 == 0)//si la columna es par
                    {
                        //en estas posiciones no hay nada
                    }
                }

            }

        }
    }
    public void buscar_cuadrados(string _active_player)
    {
        for (int row = 0; row < rows-2; row++)
        {

            for (int column = 0; column < columns; column++)
            {
                if ((row+1) % 2 != 0 )//si la fila es impar
                {
                    if ((column+1) % 2 == 0 )//si la columna es par(horizontal)
                    {
                        //Debug.Log("row: " + row + " column: " + column);
                        //evaluamos desde el centro del cuadrado
                        if (spaces[row, column].pressed == true &&
                            spaces[row + 2, column].pressed == true &&
                            spaces[row + 1, column + 1].pressed == true &&
                            spaces[row + 1, column - 1].pressed == true)
                        {
                            spaces[row, column].state = Spaces.State.cuadrado;
                            spaces[row + 2, column].state = Spaces.State.cuadrado;
                            spaces[row + 1, column + 1].state = Spaces.State.cuadrado;
                            spaces[row + 1, column - 1].state = Spaces.State.cuadrado;

                            if (tablero[row + 1, column].GetComponent<Text>().text=="") {
                                tablero[row + 1, column].GetComponent<Text>().text = _active_player;
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
                        tablero[row, column].GetComponent<Spaces>().state = spaces[row, column].state;


                    }
                }
                if ((row + 1) % 2 == 0)//si la fila es par
                {
                    if ((column + 1) % 2 != 0)//si la columna es impar (vertical)
                    {
                        tablero[row, column].GetComponent<Spaces>().state = spaces[row, column].state;
                    }

                }

            }
        }
    }
}
