using UnityEngine;
using UnityEngine.UI;

public class LinePainter : MonoBehaviour {

    Vector3 initScale;

    Image image;

    Line line;

    public Line Line
    {
        get { return line; }
    }

    void Start ()
    {
        image = GetComponent<Image>();
        initScale = transform.localScale;

        line = new Line(this);
        Debug.Log(line);
    }

    public void SetColor(Color color)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Normal");
        transform.localScale = initScale;
        animator.enabled = false;
        image.color = color;
    }

    public void OnPressed()
    {
        line.On_Pressed();
    }
}
