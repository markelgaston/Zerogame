using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoringmove
{

    public int score;

    public struct Move
    {
        public int row;
        public int column;
    }

    public Move move;

    public Scoringmove()
    {

    }
    public Scoringmove(int _score, Move _move)
    {
        score = _score;
        move = _move;
    }
}