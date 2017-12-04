using UnityEngine;

public class Square
{
    Line[] lines;
    bool[] pressedLines;
    
    bool closedSquare;

    int index;

    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    
    int score;

    public int Score {
        get { return score; }
        set { score = value; }
    }

    string player;

    public string Player
    {
        get { return player; }
        set { player = value; }
    }


    public Square()
    {
        lines = new Line[4];
        pressedLines = new bool[4];
        player = "";
    }

<<<<<<< HEAD
    public void SetLine(string direction, LinePainter newLine, Square parentSquare)
=======
    public bool GetPressed(int index)
    {
        return pressedLines[index];
    }

    public void SetPressed(int index, bool value)
    {
        pressedLines[index] = value;
    }

    public void SetLine(string direction, Line newLine, Square parentSquare)
>>>>>>> master
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

    public void SetLine(int index, LinePainter newLine, Square parentSquare)
    {
        lines[index] = newLine.Line;
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

    public bool IsClosedSquare()
    {
        if (closedSquare)
            return true;

        else
        {
            int pressedCount = 0;
            for (int i = 0; i < 4; ++i)
            {
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
            l.LinePainter.SetColor(color);
        }
    }

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
