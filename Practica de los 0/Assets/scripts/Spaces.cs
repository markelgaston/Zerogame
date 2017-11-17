using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaces : MonoBehaviour {

    public byte row, column;

    public bool pressed = false;

    public State space = 0; //debug

    public enum State{
        normal,
        pulsado,
        cuadrado
    };
    public State state = State.normal;

    //public Spaces()
    //{

    //}

    public void set_row_column(byte row_, byte column_)
    {
        row = row_;
        column = column_;
    }
	
}
