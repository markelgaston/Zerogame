using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line
{
    /// <summary>
    /// Componente gráfico de la línea
    /// </summary>
    LineGraphic lineGraphic;

    public LineGraphic LineGraphic
    {
        get { return lineGraphic; }
    }

    /// <summary>
    /// Cuadrados que contienen a la línea
    /// </summary>
    List<Square> parentSquares = new List<Square>();

    /// <summary>
    /// Índices de la línea dentro de los cuadrados que la contienen
    /// </summary>
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
    
    /// <summary>
    /// Estado de la línea
    /// </summary>
    bool pressed;
    
    public bool IsPressed
    {
        get { return pressed; }
        set { pressed = value; }
    }
    
    /// <summary>
    /// Se inicializa la parte gráfica de la línea
    /// </summary>
    /// <param name="_graphic"></param>
    public void InitGraphic(LineGraphic _graphic)
    {
        lineGraphic = _graphic;
        lineGraphic.SetLine(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="square"></param>
    /// <param name="index"></param>
    public void AddSquare(Square square, int index)
    {
        parentSquares.Add(square);
        indicesInParent.Add(index);
    }    
    
    /// <summary>
    /// Controla el input de la IA y cambia el estado de la línea
    /// </summary>
    public void On_Pressed()
    {
        if (!pressed)
        {
            pressed = true;
            GameController.Instance.End_Turn(this);
        }
    }

}
