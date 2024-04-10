using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class selerScript : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;

    public int Viewtype = 0;
    private string ScreenText;
    private int bb00total;

    private void Start()
    {
        InvokeRepeating("View",0,2f);
    }

    private void View()
    {
        switch (Viewtype)
        {
            case 0: ScreenText = "Hello :)"; break;

            case 1: ScreenText = "searching.."; break;

            case 2: ScreenText = "Total: " + bb00total.ToString(); break;

            default: ScreenText = "Error.."; break;
        }

        
        m_Text.text = ScreenText;
    }

    public void changeSeller(int xxTotal = -1)
    {
        if (xxTotal < 0)
        {
            Viewtype = 404;
        }
        else if (xxTotal > 0)
        {
            Viewtype = 1;
            bb00total = xxTotal;
            Invoke("ViewTotal", 1);
        }
    }

    private void ViewTotal()
    {
        Viewtype = 2;
    }


}
