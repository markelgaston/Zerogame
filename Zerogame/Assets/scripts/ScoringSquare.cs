using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSquare {
    
    int squareIndex;
    int score;

    public int SquareIndex {
        get { return squareIndex; }
    }

    public int Score {
        get { return score; }
    }

    public ScoringSquare(int _score, int _squareIndex) {
        
        squareIndex = _squareIndex;
        score = _score;
    }
}
