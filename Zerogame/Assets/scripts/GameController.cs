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

    public Text activePlayerText;

    [Range(3, 12)]
    public int rows, columns;

    int realRows, realColumns;

    public RectTransform panel;

    public Board board;

    float distance = 72; //distance between nodes

    Ai ai;

    public GameObject turnTexts;
    public GameObject endTexts;

    public static GameController Instance;

    private void Awake()
    {
        if (Instance != null || Instance == this)
            Destroy(this);
        else
            Instance = this;
        
        endTexts.SetActive(false);

        ai = new Ai();

        realRows = rows * 2 + 1;
        realColumns = columns * 2 + 1;

        int count = 0;

        Vector3 pos = this.transform.position;

        int s_rows = 0,
            s_columns = 0;

        board = new Board(rows, columns);
        board.InitializePlayerText(this.activePlayerText);

        board.boardElements = new GameObject[realRows, realColumns];

        CreateBoard();

<<<<<<< HEAD
        board.lines = new LinePainter[rows * 2 + 1, columns + 1];
=======
        board.lines = new Line[rows * 2 + 1, columns + 1];
>>>>>>> master

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
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(circles_container.transform, true);
                    }
                    if ((column + 1) % 2 == 0)
                    {
                        GameObject obj = Instantiate(line, pos, this.transform.rotation, this.transform);
                        obj.name = "Line " + ++count;
                        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 100);                  
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(lines_container.transform, true);

<<<<<<< HEAD
                        board.lines[s_rows, s_columns] = obj.GetComponent<LinePainter>();
=======
                        board.lines[s_rows, s_columns] = obj.GetComponent<Line>();
>>>>>>> master
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
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(lines_container.transform, true);

<<<<<<< HEAD
                        board.lines[s_rows, s_columns] = obj.GetComponent<LinePainter>();
=======
                        board.lines[s_rows, s_columns] = obj.GetComponent<Line>();
>>>>>>> master
                        s_columns++;
                    }
                    if ((column + 1) % 2 == 0)
                    {
                        GameObject obj = Instantiate(text, pos, this.transform.rotation, this.transform);
                        board.boardElements[row, column] = obj;
                        board.texts.Add(obj.GetComponent<Text>());
                        obj.transform.SetParent(texts_container.transform, true);
                    }
                }
                pos = new Vector3(pos.x + distance, pos.y, pos.z);
            }
            s_columns = 0;
            s_rows++;
            pos = new Vector3(this.transform.position.x, pos.y - distance, pos.z);
        }

        PanelFit();

        board.FindSquares();
    }

    private void CreateBoard()
    {
        board.squares = new Square[rows * columns];

        for (int i = 0; i < board.squares.Length; ++i)
        {
            board.squares[i] = new Square();
            board.squares[i].Index = i;
        }
    }

    void PanelFit()
    {
        RectTransform boardRT = GetComponent<RectTransform>();
        panel.position = new Vector2(boardRT.position.x - 50, boardRT.position.y + 50);
        
        panel.sizeDelta = new Vector2((board.boardElements[realRows - 1, realColumns - 1].GetComponent<RectTransform>().position.x - board.boardElements[0, 0].GetComponent<RectTransform>().position.x) + 100,
                                      -(board.boardElements[realRows - 1, realColumns - 1].GetComponent<RectTransform>().position.y - board.boardElements[0, 0].GetComponent<RectTransform>().position.y) + 100);

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        
        float scaleX = screenWidth / panel.sizeDelta.x;
        float scaleY = screenHeight / panel.sizeDelta.y;
        float square_scale;

        if (scaleX >= scaleY)
            square_scale = scaleY;
        else square_scale = scaleX;

        transform.SetParent(panel.transform, true);
        panel.localScale = new Vector3(square_scale * .8f, square_scale * .8f, 0);
        panel.anchorMin = new Vector2(.5f, .42f);
        panel.anchorMax = new Vector2(.5f, .42f);
        panel.sizeDelta = Vector2.zero;
        panel.anchoredPosition = Vector2.zero;
        
    }

<<<<<<< HEAD
    public void AIEnded(ScoringSquare scoringSquare) {

=======
    public void AIEnded(ScoringSquare scoringSquare)
    {
>>>>>>> master
        Line line = board.ChooseLine(scoringSquare.SquareIndex);
        
        //End_Turn(line);
        line.On_Pressed();
    }

    public void End_Turn(Line line)
    {
        board.UpdateColours(line);

        if(!board.IsSquare(line))
            board.activePlayer = board.NextPlayer();

        if (board.IsEndOfGame())
            board.FinishGame();

        else if(board.players[board.activePlayer].Contains("Ai"))
            ai.Play(board, board.activePlayer);
    }

}
