using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSquare
{
    /// <summary>
    /// Índice del cuadrado en el tablero
    /// </summary>
    int squareIndex;

    /// <summary>
    /// Puntuación del movimiento
    /// </summary>
    int score;

    public int SquareIndex {
        get { return squareIndex; }
    }

    public int Score {
        get { return score; }
    }

    /// <summary>
    /// Constructor del movimiento
    /// </summary>
    /// <param name="_score"></param>
    /// <param name="_squareIndex"></param>
    public ScoringSquare(int _score, int _squareIndex)
    {        
        squareIndex = _squareIndex;
        score = _score;
    }
}
