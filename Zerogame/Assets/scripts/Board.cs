using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Board
{
    public int rows, columns;

    public Square[] squares;

    /// <summary>
    /// Textos del centro de cada cuadrado
    /// </summary>
    public List<Text> texts = new List<Text>();

    /// <summary>
    /// Elementos gráficos del tablero
    /// </summary>
    public GameObject[,] boardElements;

    /// <summary>
    /// Líneas del tablero
    /// </summary>
    public Line[,] lines;
    
    /// <summary>
    /// Nombre de los jugadores
    /// </summary>
    public string[] players = { "Jugador", "Ai"};

    /// <summary>
    /// Jugador de la ronda actual
    /// </summary>
    public int activePlayer = 0;


    /// <summary>
    /// Constructor de tablero. Inicializa los tamaños
    /// </summary>
    /// <param name="_rows"></param>
    /// <param name="_columns"></param>
    public Board(int _rows, int _columns, string[] _players)
    {
        rows = _rows;
        columns = _columns;

        squares = new Square[rows * columns];

        players = _players;
    }

    /// <summary>
    /// Establece el texto del turno al empezar el juego
    /// </summary>
    /// <param name="_text"></param>
    public void InitializePlayerText(Text _text)
    {
        GameController.Instance.activePlayerText = _text;
        GameController.Instance.activePlayerText.text = players[0];
    }

    /// <summary>
    /// Actualiza los colores de las líneas según la jugada
    /// </summary>
    /// <param name="move"></param>
    public void UpdateColours(Line move)
    {
        bool anyClosedSquare = false;
        List<Square> squaresClosed = new List<Square>();

        // Busca si la línea ha cerrado alguno de los cuadrados a los que pertenece
        foreach (Square square in move.ParentSquares)
        {
            if (square.IsClosedSquare())
            {
                anyClosedSquare = true;
                squaresClosed.Add(square);

                // Actualiza el texto con el jugador activo en caso de cerrar un cuadrado
                texts[square.Index].text = players[activePlayer];
                square.Player = players[activePlayer];
            }
        }

        // En caso de haber encontrado algún cuadrado cerrado...
        if (anyClosedSquare)
            foreach (Square square in squaresClosed)
            {
                square.SetClosedColor(Color.red);
            }
        else
            move.LineGraphic.SetColor(Color.blue);

    }

    /// <summary>
    /// Inicializa los cuadrados con las líneas ya creadas
    /// </summary>
    public void FindSquares()
    {
        int squareIndex = 0;

        for (int row = 1; row < lines.GetLength(0); row += 2)
        {
            for (int column = 0; column < lines.GetLength(1) - 1; column++)
            {
                if (lines[row, column] != null)
                {
                    squares[squareIndex].SetLine("W", lines[row, column], squares[squareIndex]);
                    squares[squareIndex].SetLine("N", lines[row - 1, column], squares[squareIndex]);
                    squares[squareIndex].SetLine("S", lines[row + 1, column], squares[squareIndex]);
                    squares[squareIndex].SetLine("E", lines[row, column + 1], squares[squareIndex]);

                    ++squareIndex;
                }
            }
        }
    }

    /// <summary>
    /// Devuelve el siguiente jugador al que tocará jugar
    /// </summary>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    public int NextPlayer()
    {
        int auxActive = activePlayer;
        if (auxActive >= players.Length - 1)
            auxActive = 0;
        else
            ++auxActive;

        GameController.Instance.activePlayerText.text = players[auxActive];

        return auxActive;
    }

    /// <summary>
    /// Comprueba si están todos los cuadrados cerrados
    /// </summary>
    /// <returns></returns>
    public bool IsEndOfGame()
    {
        for (int i = 0; i < squares.Length; i++)
        {
            if (!squares[i].IsClosedSquare())
                return false;
        }

        return true;
    }

    /// <summary>
    /// Devuelve un valor del tablero según su estado
    /// </summary>
    /// <returns></returns>
    public int Evaluate(int activePlayer)
    {
        int bestScore = 0;

        if (IsEndOfGame())
        {
            int bestIndex;
            FinalScore(out bestIndex);

            if (bestIndex == activePlayer)
                return 500;
            else
                return -500;
        }

        // Por cada cuadrado se analiza su situación y se puntúa
        for(int i = 0; i < squares.Length; ++i)
        {
            int pressedLines = squares[i].GetPressedLines();

            if (pressedLines != 4)
            {
                if (pressedLines == 0)
                {
                    bestScore += 2;

                }
                else if (pressedLines == 1)
                {
                    bestScore += 1;

                }
                else if (pressedLines == 3)
                {
                    bestScore -= 20;

                }
                else
                {
                    bestScore += 20;
                }
            }            
        }

        return bestScore;
    }

    /// <summary>
    /// Todos los posibles movimientos de un estado del tablero
    /// </summary>
    /// <returns></returns>
    public List<Square> PossibleMoves()
    {
        List<Square> availableSquares = new List<Square>();

        for (int i = 0; i < squares.Length; i++)
        {
            if (squares[i].GetPressedLines() < 4)
                availableSquares.Add(squares[i]);                
        }

        return availableSquares;
    }

    /// <summary>
    /// Crea un nuevo tablero a partir de un movimiento
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public Board GenerateNewBoardFromMove(Square move)
    {
        Board newBoard = this.DuplicateBoard();
        
        // Escoje una línea sin pulsar de un cuadrado
        Line line = newBoard.ChooseLine(move.Index);
        line.IsPressed = true;
        
        if (!move.IsClosedSquare())
            newBoard.activePlayer = NextPlayer();

        return newBoard;
    }

    /// <summary>
    /// Duplica el tablero y las variables necesarias dentro del mismo
    /// </summary>
    /// <returns></returns>
    private Board DuplicateBoard()
    {
        Board newBoard = new Board(rows, columns, players);

        for (int i = 0; i < squares.Length; i++)
        {
            // Se crean cuadrados y líneas nuevas
            newBoard.squares[i] = new Square();
            Line[] newLines = new Line[4];

            // Se asignan los estados del tablero actual al nuevo
            for (int j = 0; j < 4; j++)
            {
                newLines[j] = new Line();
                newLines[j].ParentSquares = squares[i].GetLine(j).ParentSquares;
                newLines[j].IndicesInParent = squares[i].GetLine(j).IndicesInParent;
                newBoard.squares[i].SetLine(j, newLines[j]);
                newBoard.squares[i].SetPressed(j, squares[i].GetIsPressed(j));
            }

            newBoard.squares[i].Index = squares[i].Index;
            newBoard.squares[i].Lines = newLines;
            newBoard.squares[i].Player = squares[i].Player;
        }
        newBoard.activePlayer = this.activePlayer;

        return newBoard;
    }

    /// <summary>
    /// Devuelve true si se ha cerrado un cuadrado
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public bool IsSquare(Line line)
    {
        foreach(Square square in line.ParentSquares)
        {
            if(square.IsClosedSquare())
                return true;
        }

        return false;
    }

    /// <summary>
    /// Termina el juego en caso de haberse completado el tablero
    /// </summary>
    public void FinishGame()
    {
        GameController.Instance.turnTexts.SetActive(false);
        

        Text scoreText = GameController.Instance.endScore.GetComponent<Text>();
        Text winnerText = GameController.Instance.winnerText.GetComponent<Text>();

        int bestIndex = 0;
        int[] count = FinalScore(out bestIndex);
        FinalText(scoreText, winnerText, count, bestIndex);

        GameController.Instance.EndTexts(true);
    }

    /// <summary>
    /// Conteo de puntos de cada jugador
    /// </summary>
    /// <param name="bestIndex"></param>
    /// <returns></returns>
    int[] FinalScore(out int bestIndex)
    {
        int[] count = new int[players.Length]; // Número de jugadores
        int bestScore = -1;

        // Índice del jugador ganador
        bestIndex = -1;

        // Conteo
        for (int i = 0; i < texts.Count; i++)
        {
            for (int j = 0; j < players.Length; j++)
            {
                if (texts[i].text.Equals(players[j]))
                    ++count[j];

                if (count[j] > bestScore)
                {
                    bestScore = count[j];
                    bestIndex = j;
                }
            }
        }

        int drawCount = 0;
        foreach(int i in count)
        {
            if (i == bestScore)
            {
                ++drawCount;
            }
        }

        if (drawCount > 1)
            bestIndex = -1;

        return count;
    }

    /// <summary>
    /// Elije una línea de un cuadrado
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Line ChooseLine(int index)
    {
        Line line = null;

        // Recorro las líneas del cuadrado seleccionado
        for (int i = 0; i < 4; i++)
        {
            // Busco líneas sin pulsar
            if (!squares[index].GetIsPressed(i))
            {
                line = squares[index].GetLine(i);
            }            
        }

        return line;
    }

    /// <summary>
    /// Texto al finalizar el juego
    /// </summary>
    /// <param name="score"></param>
    /// <param name="winner"></param>
    /// <param name="count"></param>
    /// <param name="bestIndex"></param>
    void FinalText(Text score, Text winner, int[] count, int bestIndex)
    {
        string separator = "   -   ";

        if (bestIndex == -1)
            winner.text = "It's a draw!";
        else
            winner.text = players[bestIndex] + " wins!";

        score.text = "";

        // Se recorren todos los jugadores y se pintan junto con su puntuación
        for (int i = 0; i < count.Length; i++)
        {
            score.text += players[i] + ": " + count[i];
            if (i != count.Length - 1)
                score.text += separator;
        }
    }
}
