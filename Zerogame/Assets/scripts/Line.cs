using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line
{
    LineGraphic lineGraphic;

    public LineGraphic LineGraphic
    {
        get { return lineGraphic; }
    }

    List<Square> parentSquares = new List<Square>();
    List<int> indicesInParent = new List<int>();

    public List<Square> ParentSquares
    {
        get { return parentSquares; }
        set { parentSquares = value; }
    }

    public List<int> IndicesInParent
    {
        get { return indicesInParent; }
        set { indicesInParent = value; }
    }
    
    bool pressed;
    
    public bool IsPressed
    {
        get { return pressed; }
        set { pressed = value; }
    }
    
    public void InitGraphic(LineGraphic _graphic)
    {
        lineGraphic = _graphic;
        lineGraphic.SetLine(this);
    }

    public void AddSquare(Square square, int index)
    {
        parentSquares.Add(square);
        indicesInParent.Add(index);
    }    
    
    public void On_Pressed()
    {
        if (!pressed)
        {
            pressed = true;
            GameController.Instance.End_Turn(this);
        }
    }

}
