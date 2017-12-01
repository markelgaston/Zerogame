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
    public GameObject text;

    public GameObject circles_container;
    public GameObject lines_container;
    public GameObject texts_container;


    public int rows, columns;

    public float distance; //distance between nodes

    public RectTransform panel;

    public Board board;

    List<GameObject> boardElements = new List<GameObject>();

    Ai ai;
    


    public static GameController Instance;

    private void Awake()
    {
        if (Instance != null || Instance == this)
            Destroy(this);
        else
            Instance = this;

        ai = new Ai();

        int realRows = rows * 2 + 1;
        int realColumns = columns * 2 + 1;

        int count = 0;

        Vector3 pos = this.transform.position;

        int s_rows = 0,
            s_columns = 0;

        board = new Board(rows, columns);

        board.boardElements = new GameObject[realRows, realColumns];

        CreateBoard();

        board.lines = new Line[rows * 2 + 1, columns + 1];

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
                        //obj.GetComponent<Line>().set_row_column(row, column);
                         //obj.GetComponent<ButtonController>().type = "circle";
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(circles_container.transform, true);
                    }
                    if ((column + 1) % 2 == 0)
                    {
                        GameObject obj = Instantiate(line, pos, this.transform.rotation, this.transform);
                        obj.name = "Line " + ++count;
                        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 100);
                        //obj.GetComponent<Line>().set_row_column(row, column);
                         //obj.GetComponent<ButtonController>().type = "h_line";                       
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
                        obj.name = "Line " + ++count;
                        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 300);
                        //obj.GetComponent<Line>().set_row_column(row, column);
                         //obj.GetComponent<ButtonController>().type = "v_line";
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(lines_container.transform, true);

                        board.lines[s_rows, s_columns] = obj.GetComponent<Line>();
                        s_columns++;
                    }
                    if ((column + 1) % 2 == 0)
                    {
                        GameObject obj = Instantiate(text, pos, this.transform.rotation, this.transform);
                        //obj.GetComponent<Line>().set_row_column(row, column);
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


        board.FindSquares();
    }

    private void CreateBoard()
    {
        board.squares = new Square[rows * columns];

        for (int i = 0; i < board.squares.Length; ++i)
        {
            board.squares[i] = new Square();
            board.squares[i].SetIndex(i);

            // Primera fila de rows
            if (i < rows)
            {
                // Primer cuadrado
                if (i == 0)
                {

                }

                else
                {

                }
            }

            // Primera columna de columns
            else if (i % rows == 0)
            {

            }

            // Resto de casillas (ya sin primera fila y primera columna)
            else
            {

            }
        }
    }

    /*private void Update()
    {

        if (board.activePlayer == "Ai")
        {
            ai.play(board);
            End_Turn();
        }

    }*/

    public void End_Turn(Line line)
    {
        board.UpdateColours(board.activePlayer, line);

        if (!IsSquare(line))
            board.Opponent();
    }

    public bool IsSquare(Line line)
    {
        foreach(Square square in line.ParentSquares)
        {
            if (square.IsClosedSquare())
                return true;
        }

        return false;
    }
}
