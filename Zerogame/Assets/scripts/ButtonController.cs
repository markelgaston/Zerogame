using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour {

    public float spd = 0.5f;
    public int control = 0;

    public string type = null;

    private RectTransform rectTransform;

    private Vector2 enterSize = new Vector2();
    private Vector2 exitSize = new Vector2();

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {

        //raya_vertical
        if (type == "v_line")
        {
            if (control == 1)
                enterSize = new Vector2(125, 325);

            if (control == 2)
                exitSize = new Vector2(100, 300);
        }

        //raya_horizontal
        else if (type == "h_line")
        {
            if (control == 1)
                enterSize = new Vector2(325, 125);

            if (control == 2)
                exitSize = new Vector2(300, 100);
        }

        //circulo
        else if (type == "circle")
        {
            if (control == 1)
                enterSize = new Vector2(125, 125);

            if (control == 2)
                exitSize = new Vector2(100, 100);
        }

        if (control == 1)
        {
            rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, enterSize, spd * Time.deltaTime);

            if (rectTransform.sizeDelta.x == 125)
            {
                control = 0;
            }
        }

        else if (control == 2)
        {
            rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, exitSize, spd * Time.deltaTime);

            if (rectTransform.sizeDelta.x == 100)
            {
                control = 0;
            }
        }


    }
    public void on_hover_enter()
    {
        control = 1;
    }

    public void on_hover_exit()
    {
        control = 2;
    }
    public void on_press()
    {
        this.gameObject.GetComponent<Line>().pressed = true;
    }

}
