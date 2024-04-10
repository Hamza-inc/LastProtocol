using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambGlitch : MonoBehaviour
{
    [SerializeField] private Light isik;
    [SerializeField] private Renderer rrLight;
    void FixedUpdate()
    {
        if (Random.Range(0, 15) < 2)
        {
            if (Random.Range(0, 10) < 1)
            {
                isik.GetComponent<Light>().intensity = 0;
                rrLight.materials[1].SetColor("_EmissionColor", new Vector4(0, 0, 0));
            }
            else
            {
                int veri = Random.Range(30, 100);
                isik.GetComponent<Light>().intensity = veri;
                rrLight.materials[1].SetColor("_EmissionColor", new Vector4(5, 5, 5 - ((3 * veri) / 100)) * ((4 * (float)veri) / 100));
            }
            
        }
    }
}
