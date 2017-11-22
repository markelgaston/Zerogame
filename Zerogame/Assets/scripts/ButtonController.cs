using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour {

    public float spd = 0.5f;
    public int control = 0;

    public string type = null;

	void Update()
    {

        //raya_vertical
        if (type== "v_line") {
            if (control == 1)
            {
                this.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.GetComponent<RectTransform>().sizeDelta, new Vector2(125, 325), spd * Time.deltaTime);

                if (this.GetComponent<RectTransform>().sizeDelta.x == 125)
                {
                    control = 0;
                }
            }
            if (control == 2)
            {
                this.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.GetComponent<RectTransform>().sizeDelta, new Vector2(100, 300), spd * Time.deltaTime);

                if (this.GetComponent<RectTransform>().sizeDelta.x == 100)
                {
                    control = 0;
                }
            }
        }
        //raya_horizontal
        if (type == "h_line")
        {
            if (control == 1)
            {
                this.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.GetComponent<RectTransform>().sizeDelta, new Vector2(325, 125), spd * Time.deltaTime);

                if (this.GetComponent<RectTransform>().sizeDelta.x == 125)
                {
                    control = 0;
                }
            }
            if (control == 2)
            {
                this.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.GetComponent<RectTransform>().sizeDelta, new Vector2(300, 100), spd * Time.deltaTime);

                if (this.GetComponent<RectTransform>().sizeDelta.x == 100)
                {
                    control = 0;
                }
            }
        }
        //circulo
        if (type == "circle")
        {
            if (control == 1)
            {
                this.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.GetComponent<RectTransform>().sizeDelta, new Vector2(125, 125), spd * Time.deltaTime);

                if (this.GetComponent<RectTransform>().sizeDelta.x == 125)
                {
                    control = 0;
                }
            }
            if (control == 2)
            {
                this.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.GetComponent<RectTransform>().sizeDelta, new Vector2(100, 100), spd * Time.deltaTime);

                if (this.GetComponent<RectTransform>().sizeDelta.x == 100)
                {
                    control = 0;
                }
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
