using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    public int row, column;

    public bool pressed = false;

    //public State space = 0; //debug

    public enum State{
        idle,
        pressed,
        square
    };
    public State state = State.idle;

    //public Spaces()
    //{

    //}

    public void set_row_column(int row_, int column_)
    {
        row = row_;
        column = column_;
    }
	
}
