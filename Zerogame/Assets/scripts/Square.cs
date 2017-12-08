using UnityEngine;

public class Square
{
    /// <summary>
    /// Líneas que contiene un cuadrado
    /// </summary>
    Line[] lines;

    public Line[] Lines
    {
        get { return lines; }
        set { lines = value; }
    }

    /// <summary>
    /// Índice del cuadrado en el tablero
    /// </summary>
    int index;

    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    
    /// <summary>
    /// Nombre del jugador que cierra el cuadrado actual
    /// </summary>
    string player;

    public string Player
    {
        get { return player; }
        set { player = value; }
    }

    /// <summary>
    /// Constructor de cuadrado
    /// </summary>
    public Square()
    {
        lines = new Line[4];
        player = "";
    }
    
    /// <summary>
    /// Devuelve si una línea está o no pulsada
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetIsPressed(int index)
    {
        return lines[index].IsPressed;
    }

    /// <summary>
    /// Actualiza una línea a estado pulsada
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public void SetPressed(int index, bool value)
    {
        lines[index].IsPressed = value;
    }

    /// <summary>
    /// Establece una línea mediante coordenadas y añade un cuadrado padre
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="newLine"></param>
    /// <param name="parentSquare"></param>
    public void SetLine(string direction, Line newLine, Square parentSquare)
    {
        if (direction.Equals("N"))
        {
            SetLine(0, newLine, parentSquare);
        }
        else if (direction.Equals("S"))
        {
            SetLine(1, newLine, parentSquare);
        }
        else if (direction.Equals("E"))
        {
            SetLine(2, newLine, parentSquare);
        }
        else if (direction.Equals("W"))
        {
            SetLine(3, newLine, parentSquare);
        }
    }

    /// <summary>
    /// Establece una línea mediante índice y añade un cuadrado padre
    /// </summary>
    /// <param name="index"></param>
    /// <param name="newLine"></param>
    /// <param name="parentSquare"></param>
    public void SetLine(int index, Line newLine, Square parentSquare)
    {
        SetLine(index, newLine);
        lines[index].AddSquare(parentSquare, index);
    }

    /// <summary>
    /// Establece una línea mediante índice
    /// </summary>
    /// <param name="index"></param>
    /// <param name="newLine"></param>
    public void SetLine(int index, Line newLine)
    {
        lines[index] = newLine;
    }

    /// <summary>
    /// Se obtiene una línea del cuadrado mediante índice
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Line GetLine(int index)
    {
        return lines[index];
    }

    /// <summary>
    /// Devuelve si el cuadrado tiene todas sus líneas pulsadas o no
    /// </summary>
    /// <returns></returns>
    public bool IsClosedSquare()
    {
        int pressedCount = 0;

        // Se recorren las 4 líneas del cuadrado
        for (int i = 0; i < 4; ++i)
        {
            if (GetIsPressed(i))
                ++pressedCount;
        }

        if (pressedCount == 4)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Actualiza el color de todas las líneas de un cuadrado una vez ha sido cerrado
    /// </summary>
    /// <param name="color"></param>
    public void SetClosedColor(Color color)
    {
        foreach(Line l in lines)
        {
            l.LineGraphic.SetColor(color);
        }
    }

    /// <summary>
    /// Devuelve el número de líneas pulsadas en un cuadrado
    /// </summary>
    /// <returns></returns>
    public int GetPressedLines()
    {
        int count = 0;
        foreach(Line l in lines) 
        {
            if(l.IsPressed)
                ++count;
        }

        return count;
    }

}
