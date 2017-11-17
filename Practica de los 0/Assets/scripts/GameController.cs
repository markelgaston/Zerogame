using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;

public class GameController : MonoBehaviour {

    public GameObject circulo;

    public GameObject raya;

    public GameObject text;

    public int rows, columns;

    public float distancia; //distancia entre nodos
    
    public RectTransform panel;

    //board
    public Board board;

    //AI
    public Ai ai;

    //ActivePlayer
    public string active_player;

    private void Awake()
    {
        Spaces[,] spaces = new Spaces[rows, columns];
        active_player = "Jugador";
        board = new Board(rows, columns);
        ai = new Ai();
        GameObject[,] tablero;
        int realRows = rows * 2 + 1;
        int realColumns = columns * 2 + 1;
        tablero = new GameObject[realRows, realColumns];
        Vector3 pos = this.transform.position;
        
        //Instantiate cuadrado/circulo
        for (int row = 0; row < rows; row++) 
        {
            for (int column = 0; column < columns; column++) 
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

        RectTransform boardRT = GetComponent<RectTransform>();
        panel.position = new Vector2(boardRT.position.x - 50, boardRT.position.y + 50);

        /*panel.sizeDelta = new Vector2( (tablero[rows - 1, 0].GetComponent<RectTransform>().position.x - tablero[0, 0].GetComponent<RectTransform>().position.x) + 100,
                                      -(tablero[0, columns - 1].GetComponent<RectTransform>().position.y - tablero[0, 0].GetComponent<RectTransform>().position.y) + 100);*/
        panel.sizeDelta = new Vector2((tablero[rows - 1, columns - 1].GetComponent<RectTransform>().position.x - tablero[0, 0].GetComponent<RectTransform>().position.x) + 100,
                                      -(tablero[rows - 1, columns - 1].GetComponent<RectTransform>().position.y - tablero[0, 0].GetComponent<RectTransform>().position.y) + 100);
        
        int screenWidth  = Screen.width;
        int screenHeight = Screen.height;

        print(screenWidth);
        float scaleX = screenWidth / panel.sizeDelta.x;
        float scaleY = screenHeight / panel.sizeDelta.y;
        float escalaCuadrada;

        if (scaleX >= scaleY)
            escalaCuadrada = scaleY;
        else escalaCuadrada = scaleX;

        transform.parent = panel.transform;
        panel.localScale = new Vector3(escalaCuadrada, escalaCuadrada, 0);
        panel.anchorMin = new Vector2(.5f, .5f);
        panel.anchorMax = new Vector2(.5f, .5f);
        panel.sizeDelta = Vector2.zero;
        panel.anchoredPosition = Vector2.zero;
        
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
