using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

    public GameObject circulo;

    public GameObject raya;

    public GameObject text;

    public byte rows, columns;

    public float distancia; //distancia entre nodos

    public Spaces[,] spaces;

    //board
    public Board board;

    //AI
    public Ai ai;

    //ActivePlayer
    public string active_player;

    private void Awake()
    {
        spaces = new Spaces[rows,columns];
        active_player = "Jugador";
        board = new Board(rows, columns);
        ai = new Ai();
        GameObject[,] tablero;
        tablero = new GameObject[rows, columns];
        Vector3 pos = this.transform.position;


        //Instantiate cuadrado/circulo
        for (byte row = 0; row < rows; row++) 
        {
            for (byte column = 0; column < columns; column++) 
            {
                if ((row+1)%2!=0)//si la fila es impar
                {
                    if ((column+1) % 2 != 0)//si la columna es impar (circulo)
                    {
                        GameObject obj = Instantiate(circulo, pos, this.transform.rotation, this.transform);
                        obj.GetComponent<Spaces>().set_row_column(row, column);
                        obj.GetComponent<over_button>().tipo = "circulo";
                        tablero[row, column] = obj;
                    }
                    if ((column+1) % 2 == 0)//si la columna es par(horizontal)
                    {
                        GameObject obj = Instantiate(raya, pos, this.transform.rotation, this.transform);
                        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 100);
                        obj.GetComponent<Spaces>().set_row_column(row, column);
                        obj.GetComponent<over_button>().tipo = "raya_horizontal";
                  
                        spaces[row, column] = obj.GetComponent<Spaces>();
                        tablero[row, column] = obj;
                    }
                }
                if ((row+1)%2==0)//si la fila es par
                {
                    if ((column+1) % 2 != 0)//si la columna es impar (vertical)
                    {
                        GameObject obj = Instantiate(raya, pos, this.transform.rotation, this.transform);
                        obj.GetComponent<RectTransform>().sizeDelta =  new Vector2(100, 300);
                        obj.GetComponent<Spaces>().set_row_column(row, column);
                        obj.GetComponent<over_button>().tipo = "raya_vertical";
                        spaces[row, column ] = obj.GetComponent<Spaces>();
                        tablero[row, column] = obj;
                    }
                    if ((column+1) % 2 == 0)//si la columna es par (Textos)
                    {
                        GameObject obj = Instantiate(text, pos, this.transform.rotation, this.transform);
                        obj.GetComponent<Spaces>().set_row_column(row, column);
                        tablero[row, column] = obj;
                    }
                }
                pos = new Vector3(pos.x + distancia, pos.y, pos.z);
            }
            pos = new Vector3(this.transform.position.x, pos.y - distancia, pos.z);
        }
        board.tablero = tablero;
        board.spaces = spaces;
    }

    private void Update()
    {

        if (active_player=="Ai")
        {
            ai.play(board);
            end_turn();
        }

        board.Debug_spaces();
    }
    public void end_turn()
    {
        board.actualizar_colores(active_player);
        if (active_player=="Ai")
        {
            active_player = "Jugador";
        }
        else
        {
            active_player = "Ai";
        }
    }

}
