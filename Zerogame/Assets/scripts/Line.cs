using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line
{
    LinePainter linePainter;

    public LinePainter LinePainter
    {
        get { return linePainter; }
    }

    List<Square> parentSquares;

    public List<Square> ParentSquares
    {
        get { return parentSquares; }
    }

    bool pressed;
    
    public bool IsPressed
    {
        get { return pressed; }
        set { pressed = value; }
    }

    public Line(LinePainter _linePainter)
    {
        linePainter = _linePainter;
        parentSquares = new List<Square>();
    }

    public void AddSquare(Square square)
    {
        parentSquares.Add(square);
    }
    
    public void On_Pressed()
    {
        if (!pressed)
        {
            pressed = true;
            GameController.Instance.End_Turn(this); // Ver si ha cerrado cuadrado
        }
    }

}
