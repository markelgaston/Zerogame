using UnityEngine;
using UnityEngine.UI;

public class LineGraphic : MonoBehaviour
{

    Image image;

    Vector3 initScale;

    Line line;

    private void Start()
    {
        image = GetComponent<Image>();
        initScale = transform.localScale;
    }

    public void SetLine(Line _line)
    {
        line = _line;
    }

    public void SetColor(Color _color)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Normal");
        transform.localScale = initScale;
        animator.enabled = false;
        image.color = _color;
    }

    public void GraphicPressed()
    {
        line.On_Pressed();
    }
}
