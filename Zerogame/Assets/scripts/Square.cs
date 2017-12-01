using UnityEngine;

public class Square
{
    int squareIndex;

    Line[] lines;

    bool closedSquare;


    public Square()
    {
        lines = new Line[4];
    }

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

    void SetLine(int index, Line newLine, Square parentSquare)
    {
        lines[index] = newLine;
        lines[index].AddSquare(parentSquare);
    }

    public Line GetLine(string direction)
    {
        if (direction.Equals("N"))
        {
            return lines[0];
        }
        else if (direction.Equals("S"))
        {
            return lines[1];
        }
        else if (direction.Equals("E"))
        {
            return lines[2];
        }
        else if (direction.Equals("W"))
        {
            return lines[3];
        }

        return null;
    }

    public Line GetLine(int index)
    {
        return lines[index];
    }

    public void SetIndex(int index)
    {
        squareIndex = index;
    }

    public bool IsClosedSquare()
    {
        Debug.Log("A");
        if (closedSquare)
            return true;

        else
        {
            int pressedCount = 0;
            for (int i = 0; i < 4; ++i)
            {
                Debug.Log(lines[i].name);
                if (lines[i].IsPressed)
                    ++pressedCount;
            }

            if (pressedCount == 4)
                return true;
        }

        return false;
    }

    public void SetClosedColor(Color color)
    {
        foreach(Line l in lines)
        {
            l.SetColor(color);
        }
    }
}
