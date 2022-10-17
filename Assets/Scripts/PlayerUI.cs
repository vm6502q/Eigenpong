using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public UIButton x;
    public UIButton z;
    public UIButton h;
    public UIButton s;
    public UIScroll control;
    public UIScroll pauli;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("X"))
        {
            x.Toggle();
        }

        if (Input.GetButton("Z"))
        {
            z.Toggle();
        }

        if (Input.GetButton("H"))
        {
            h.Toggle();
        }

        if (Input.GetButton("S"))
        {
            s.Toggle();
        }

        if (Input.GetAxis("Control") > 0)
        {
            control.Scroll(true);
        }
        else if (Input.GetAxis("Control") < 0)
        {
            control.Scroll(false);
        }

        if (Input.GetAxis("Pauli") > 0)
        {
            pauli.Scroll(true);
        }
        else if (Input.GetAxis("Pauli") < 0)
        {
            pauli.Scroll(false);
        }
    }
}
