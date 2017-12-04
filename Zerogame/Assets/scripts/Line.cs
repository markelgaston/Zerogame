using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

<<<<<<< HEAD
public class Line
{
    LinePainter linePainter;

    public LinePainter LinePainter
    {
        get { return linePainter; }
    }

    List<Square> parentSquares;
=======
public class Line : MonoBehaviour
{
    List<Square> parentSquares = new List<Square>();

    Image image;

    Vector3 initScale;
>>>>>>> master

    public List<Square> ParentSquares
    {
        get { return parentSquares; }
    }

<<<<<<< HEAD
=======
    int score;

    public int Score {
        get { return score; }
        set { score = value; }
    }

>>>>>>> master
    bool pressed;
    
    public bool IsPressed
    {
<<<<<<< HEAD
        get { return pressed; }
        set { pressed = value; }
    }

    public Line(LinePainter _linePainter)
    {
        linePainter = _linePainter;
        parentSquares = new List<Square>();
=======
        //get { return pressed; }
        set { pressed = value; }
    }


    private void Start()
    {
        image = GetComponent<Image>();
        initScale = transform.localScale;
>>>>>>> master
    }

    public void AddSquare(Square square)
    {
        parentSquares.Add(square);
    }
    
<<<<<<< HEAD
=======
    public void SetColor(Color color)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Normal");
        transform.localScale = initScale;
        animator.enabled = false;
        image.color = color;
    }

>>>>>>> master
    public void On_Pressed()
    {
        if (!pressed)
        {
            pressed = true;
            GameController.Instance.End_Turn(this); // Ver si ha cerrado cuadrado
        }
    }

}
