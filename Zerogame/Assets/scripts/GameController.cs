using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject circle;
    public GameObject line;
    public GameObject text;

    public GameObject circles_container;
    public GameObject lines_container;
    public GameObject texts_container;

    /// <summary>
    /// Texto del jugador activo en un turno
    /// </summary>
    public Text activePlayerText;

    [Range(3, 7)]
    public int rows, columns;

    /// <summary>
    /// Establecen las medidas del tablero según el input del jugador
    /// </summary>
    int realRows, realColumns;

    /// <summary>
    /// Panel en el que se sitúan los objetos del tablero
    /// </summary>
    public RectTransform panel;

    public Board board;

    /// <summary>
    /// Distancia entre objetos del tablero
    /// </summary>
    float distance = 72;

    public Ai ai;

    public GameObject turnTexts;
    public GameObject endTexts;

    /// <summary>
    /// Círculos separadores de líneas
    /// </summary>
    public Sprite[] circleSprites;

    /// <summary>
    /// Panel de información
    /// </summary>
    public GameObject controlPanel;

    public static GameController Instance;

    private void Awake()
    {
        if (Instance != null || Instance == this)
            Destroy(this);
        else
            Instance = this;
        
        endTexts.SetActive(false);
        
        realRows = rows * 2 + 1;
        realColumns = columns * 2 + 1;

        int count = 0;

        Vector3 pos = this.transform.position;

        int s_rows = 0,
            s_columns = 0;

        // Se inicializa el tablero
        board = new Board(rows, columns);
        board.InitializePlayerText(this.activePlayerText);

        board.boardElements = new GameObject[realRows, realColumns];

        CreateBoard();
        board.lines = new Line[rows * 2 + 1, columns + 1];

        // Instancias de círculos y líneas
        for (int row = 0; row < realRows; row++)
        {
            for (int column = 0; column < realColumns; column++)
            {
                if ((row + 1) % 2 != 0)
                {
                    if ((column + 1) % 2 != 0)
                    {
                        GameObject obj = Instantiate(circle, pos, this.transform.rotation, this.transform);
                        obj.GetComponent<Image>().sprite = circleSprites[UnityEngine.Random.Range(0, 5)];
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

                        board.lines[s_rows, s_columns] = new Line();
                        LineGraphic lineGraphic = obj.GetComponent<LineGraphic>();
                        board.lines[s_rows, s_columns].InitGraphic(lineGraphic);

                        s_columns++;
                    }
                }

                if ((row + 1) % 2 == 0)
                {
                    if ((column + 1) % 2 != 0)
                    {
                        GameObject obj = Instantiate(line, pos, this.transform.rotation, this.transform);
                        obj.name = "Line " + ++count;
                        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 100);
                        obj.GetComponent<RectTransform>().localRotation = new Quaternion(0, 0, 90, 90);
                        board.boardElements[row, column] = obj;
                        obj.transform.SetParent(lines_container.transform, true);

                        board.lines[s_rows, s_columns] = new Line();
                        LineGraphic lineGraphic = obj.GetComponent<LineGraphic>();
                        board.lines[s_rows, s_columns].InitGraphic(lineGraphic);

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

    /// <summary>
    /// Crea un tablero mediante cuadrados
    /// </summary>
    private void CreateBoard()
    {
        board.squares = new Square[rows * columns];

        for (int i = 0; i < board.squares.Length; ++i)
        {
            board.squares[i] = new Square();
            board.squares[i].Index = i;
        }
    }

    /// <summary>
    /// Ajusta el panel según las medidas de la pantalla y del tamaño del tablero
    /// </summary>
    void PanelFit()
    {
        RectTransform boardRT = GetComponent<RectTransform>();
        panel.position = new Vector2(boardRT.position.x + 20, boardRT.position.y + 70);
        
        panel.sizeDelta = new Vector2((board.boardElements[realRows - 1, realColumns - 1].GetComponent<RectTransform>().position.x - 150 - board.boardElements[0, 0].GetComponent<RectTransform>().position.x) + 100,
                                      -(board.boardElements[realRows - 1, realColumns - 1].GetComponent<RectTransform>().position.y - 150 - board.boardElements[0, 0].GetComponent<RectTransform>().position.y) + 100);

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
        panel.anchorMin = new Vector2(.5f, .5f);
        panel.anchorMax = new Vector2(.5f, .5f);
        panel.sizeDelta = Vector2.zero;
        panel.anchoredPosition = Vector2.zero;
        
    }
    
    /// <summary>
    /// Controla el movimiento final de la IA
    /// </summary>
    /// <param name="scoringSquare"></param>
    public void AIEnded(ScoringSquare scoringSquare)
    {
        Line line = board.ChooseLine(scoringSquare.SquareIndex);

        line.On_Pressed();
    }

    /// <summary>
    /// Maneja el cambio de turnos y la actualización del tablero
    /// </summary>
    /// <param name="line"></param>
    public void End_Turn(Line line)
    {
        /*for(int i = 0; i < line.ParentSquares.Count; ++i)
        {
            line.ParentSquares[i].SetPressed(line.IndicesInParent[i], true);
        }*/

        board.UpdateColours(line);
        
        if(!board.IsSquare(line))
            board.activePlayer = board.NextPlayer();

        if (board.IsEndOfGame())
            board.FinishGame();

        else if(board.players[board.activePlayer].Contains("Ai"))
            ai.Play(board, board.activePlayer);
    }

    /// <summary>
    /// Control del panel de información
    /// </summary>
    public void ShowControl() {

        if(controlPanel.activeInHierarchy)
            controlPanel.SetActive(false);
        else
            controlPanel.SetActive(true);

    }

    /// <summary>
    /// Comienza una nueva partida
    /// </summary>
    public void RestartGame() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
