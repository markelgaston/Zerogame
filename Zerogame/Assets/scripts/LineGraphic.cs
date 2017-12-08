using UnityEngine;
using UnityEngine.UI;

public class LineGraphic : MonoBehaviour
{
    /// <summary>
    /// Render de la línea
    /// </summary>
    Image image;

    /// <summary>
    /// Escala inicial de la línea
    /// </summary>
    Vector3 initScale;

    /// <summary>
    /// Línea que contiene la lógica
    /// </summary>
    Line line;

    private void Start()
    {
        image = GetComponent<Image>();
        initScale = transform.localScale;
    }

    /// <summary>
    /// Se actualiza la línea de la clase Line
    /// </summary>
    /// <param name="_line"></param>
    public void SetLine(Line _line)
    {
        line = _line;
    }

    /// <summary>
    /// Se cambia el color de una línea pulsada y se desactiva su animator
    /// </summary>
    /// <param name="_color"></param>
    public void SetColor(Color _color)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Normal");
        transform.localScale = initScale;
        animator.enabled = false;
        image.color = _color;
    }

    /// <summary>
    /// Controla el input del jugador
    /// </summary>
    public void GraphicPressed()
    {
        line.On_Pressed();
    }
}
