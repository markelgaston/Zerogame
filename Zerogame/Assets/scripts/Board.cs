using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Board
{
    public int rows, columns;

    public Square[] squares;

    public List<Text> texts = new List<Text>();

    public GameObject[,] boardElements;

    public Line[,] lines;
    
    public string[] players = { "Player", "Ai"};

    public int activePlayer = 0;

    public Board(int _rows, int _columns)
    {
        rows = _rows;
        columns = _columns;

        squares = new Square[rows * columns];
    }

    public void InitializePlayerText(Text _text)
    {
        GameController.Instance.activePlayerText = _text;
        GameController.Instance.activePlayerText.text = players[0];
    }
    
    public void UpdateColours(Line move)
    {
        bool anyClosedSquare = false;
        List<Square> squaresClosed = new List<Square>();

        foreach (Square square in move.ParentSquares)
        {
            if (square.IsClosedSquare())
            {
                anyClosedSquare = true;
                squaresClosed.Add(square);
                texts[square.Index].text = players[activePlayer];
            }
        }

        if (anyClosedSquare)
            foreach (Square square in squaresClosed)
            {
                square.SetClosedColor(Color.gray);
            }
        else
            move.SetColor(Color.red);

    }

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
    /// Comprueba si están todas las casillas marcadas
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
    /// Devuelve un valor del tablero
    /// </summary>
    /// <returns></returns>
    public int Evaluate(int activePlayer)
    {
        int[] evaluationMatrix = new int [squares.Length];
        int bestScore = 0;
        List<int> bestScores = new List<int>();

        if (IsEndOfGame())
        {
            int bestIndex;
            FinalScore(out bestIndex);

            if (bestIndex == activePlayer)
                return 200;
            else
                return -200;
        }

        for(int i = 0; i < squares.Length; ++i)
        {
            int pressedLines = squares[i].GetPressedLines();
            if(pressedLines != 4) {

                if(pressedLines == 0) {

                    bestScore += 7;

                } else if(pressedLines == 1) {

                    bestScore += 5;

                } else if(pressedLines == 3) {

                    bestScore += 20;

                } else {

                    bestScore += -20;

                }
            }
        }

        return bestScore;
    }

    /// <summary>
    /// Todas los posibles movimientos de un estado del tablero
    /// </summary>
    /// <returns></returns>
    public List<Square> PossibleMoves()
    {
        List<Square> availableLines = new List<Square>();

        for (int i = 0; i < squares.Length; i++)
        {
            if (squares[i].GetPressedLines() < 4)
                availableLines.Add(squares[i]);                
        }

        return availableLines;
    }

    /// <summary>
    /// Crea un nuevo tablero a partir de un movimiento
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public Board GenerateNewBoardFromMove(Square move)
    {
        Board newBoard = DuplicateBoard();
        // newBoard.lines[move.row, move.column].state = Line.State.pressed;

        if(!move.IsClosedSquare())
            newBoard.activePlayer = NextPlayer();

        return newBoard;
    }

    private Board DuplicateBoard()
    {
        Board newBoard = new Board(rows, columns);
        for (int i = 0; i < squares.Length; i++)
        {
            newBoard.squares[i] = this.squares[i];

            /*for(int j = 0; j < 4; j++) {
                newBoard.squares[i].SetLine(j, this.squares[i].GetLine(j), this.squares[i]);
            }*/
        }
        newBoard.activePlayer = this.activePlayer;

        return newBoard;
    }

    public bool IsSquare(Line line) {
        foreach(Square square in line.ParentSquares) {
            if(square.IsClosedSquare())
                return true;
        }

        return false;
    }

    public void FinishGame()
    {
        GameController.Instance.turnTexts.SetActive(false);
        
        RectTransform endTextRectTr = GameController.Instance.endTexts.GetComponent<RectTransform>();

        Text scoreText = endTextRectTr.GetChild(0).GetComponent<Text>();
        Text winnerText = endTextRectTr.GetChild(1).GetComponent<Text>();

        int bestIndex = 0;
        int[] count = FinalScore(out bestIndex);
        FinalText(scoreText, winnerText, count, bestIndex);

        GameController.Instance.endTexts.SetActive(true);
    }

    int[] FinalScore(out int bestIndex)
    {
        int[] count = new int[players.Length]; // Número de jugadores
        int bestScore = -1;
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

        return count;
    }

    void FinalText(Text score, Text winner, int[] count, int bestIndex)
    {
        string separator = "   -   ";
        winner.text = players[bestIndex] + " wins!";

        score.text = "";
        for (int i = 0; i < count.Length; i++)
        {
            score.text += players[i] + ": " + count[i];
            if (i != count.Length - 1)
                score.text += separator;
        }
    }
}
