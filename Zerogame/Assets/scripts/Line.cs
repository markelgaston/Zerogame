using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{


    public int row, column;

    public enum State
    {
        idle,
        pressed,
        square
    }
    public State state = State.idle;

    public void set_row_column(int row_, int column_)
    {
        row = row_;
        column = column_;
    }

    public void On_Pressed()
    {
        if (state == State.idle)
        {
            state = State.pressed;
            GameController.Instance.end_turn();
        }
    }

}
