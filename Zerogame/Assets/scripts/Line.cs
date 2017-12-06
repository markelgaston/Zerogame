using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    Image image;

    Vector3 initScale;

    List<Square> parentSquares = new List<Square>();
    List<int> indicesInParent = new List<int>();

    public List<Square> ParentSquares
    {
        get { return parentSquares; }
    }

    public List<int> IndicesInParent
    {
        get { return indicesInParent; }
    }
    
    bool pressed;
    
    public bool IsPressed
    {
        get { return pressed; }
    }
    
    private void Start()
    {
        image = GetComponent<Image>();
        initScale = transform.localScale;
    }

    public void AddSquare(Square square, int index)
    {
        parentSquares.Add(square);
        indicesInParent.Add(index);
    }
    
    public void SetColor(Color color)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Normal");
        transform.localScale = initScale;
        animator.enabled = false;
        image.color = color;
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
