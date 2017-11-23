using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;

public class GameController : MonoBehaviour
{

    public GameObject circle;
    public GameObject line;

    public GameObject circles_container;
    public GameObject lines_container;
    public GameObject texts_container;

    public GameObject text;

    public int rows, columns;

    public float distance; //distance between nodes

    public RectTransform panel;

    //board
    public Board board;

    //AI
    public Ai ai;

    //ActivePlayer
    public string active_player;

    private void Awake()
    {
        int realRows = rows * 2 + 1;
        int realColumns = columns * 2 + 1;

        //columns and rows for LINE
        int s_rows = 0;
        int s_columns = 0;

        active_player = "Player";

        board = new Board(rows, columns);
        board.lines = new Line[rows * 2 + 1, columns + 1];
        board.boardElements = new GameObject[realRows, realColumns];

        ai = new Ai();

        Vector3 pos = this.transform.position;

       
        //Instantiate square/circle
        for (int row = 0; row < realRows; row++)
        {
            for (int column = 0; column < realColumns; column++)
            {
                if ((row + 1) % 2 != 0)
                {
                    if ((column + 1) % 2 != 0)
                    {
                        GameObject obj = Instantiate(circle, pos, this.transform.rotation, this.transform);
                        obj.GetComponent<Line>().set_row_column(row, column);
                        obj.GetComponent<ButtonController>().type = "circle";
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(circles_container.transform, true);
                    }
                    if ((column + 1) % 2 == 0)
                    {
                        GameObject obj = Instantiate(line, pos, this.transform.rotation, this.transform);
                        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 100);
                        obj.GetComponent<Line>().set_row_column(row, column);
                        obj.GetComponent<ButtonController>().type = "h_line";                       
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(lines_container.transform, true);

                        board.lines[s_rows, s_columns] = obj.GetComponent<Line>();
                        s_columns++;
                    }
                }
                if ((row + 1) % 2 == 0)
                {
                    if ((column + 1) % 2 != 0)
                    {
                        GameObject obj = Instantiate(line, pos, this.transform.rotation, this.transform);
                        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 300);
                        obj.GetComponent<Line>().set_row_column(row, column);
                        obj.GetComponent<ButtonController>().type = "v_line";
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(lines_container.transform, true);

                        board.lines[s_rows, s_columns] = obj.GetComponent<Line>();
                        s_columns++;
                    }
                    if ((column + 1) % 2 == 0)
                    {
                        GameObject obj = Instantiate(text, pos, this.transform.rotation, this.transform);
                        obj.GetComponent<Line>().set_row_column(row, column);
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(texts_container.transform, true);
                    }
                }
                pos = new Vector3(pos.x + distance, pos.y, pos.z);
            }
            s_columns = 0;
            s_rows++;
            pos = new Vector3(this.transform.position.x, pos.y - distance, pos.z);
        }


        RectTransform boardRT = GetComponent<RectTransform>();
        panel.position = new Vector2(boardRT.position.x - 50, boardRT.position.y + 50);

        /*panel.sizeDelta = new Vector2( (tablero[rows - 1, 0].GetComponent<RectTransform>().position.x - tablero[0, 0].GetComponent<RectTransform>().position.x) + 100,
                                      -(tablero[0, columns - 1].GetComponent<RectTransform>().position.y - tablero[0, 0].GetComponent<RectTransform>().position.y) + 100);*/
        panel.sizeDelta = new Vector2((board.boardElements[realRows - 1, realColumns - 1].GetComponent<RectTransform>().position.x - board.boardElements[0, 0].GetComponent<RectTransform>().position.x) + 100,
                                      -(board.boardElements[realRows - 1, realColumns - 1].GetComponent<RectTransform>().position.y - board.boardElements[0, 0].GetComponent<RectTransform>().position.y) + 100);

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        //print(screenWidth);
        float scaleX = screenWidth / panel.sizeDelta.x;
        float scaleY = screenHeight / panel.sizeDelta.y;
        float square_scale;

        if (scaleX >= scaleY)
            square_scale = scaleY;
        else square_scale = scaleX;

        transform.SetParent(panel.transform, true);
        panel.localScale = new Vector3(square_scale, square_scale, 0);
        panel.anchorMin = new Vector2(.5f, .5f);
        panel.anchorMax = new Vector2(.5f, .5f);
        panel.sizeDelta = Vector2.zero;
        panel.anchoredPosition = Vector2.zero;

    }

    private void Update()
    {

        if (active_player == "Ai")
        {
            //ai.play(board);
            end_turn();
        }

    }
    public void end_turn()
    {
        board.updateColours(active_player);
        if (active_player == "Ai")
        {
            active_player = "Player";
        }
        else
        {
            active_player = "Ai";
        }
    }

}
