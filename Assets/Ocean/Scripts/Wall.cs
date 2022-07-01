using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wall : MonoBehaviour
{
    public GameObject Bomb;
    public List<Rigidbody> Bricks;
    public float Coefficient;
    private bool m_ChangeMatColor;

    [SerializeField] Material LightMat;
    [SerializeField] Material DarkMat;

    [SerializeField] MeshRenderer[] m_RenderersLight;
    [SerializeField] MeshRenderer[] m_RenderersDark;

    [SerializeField] Color defaultDarkColor;
    [SerializeField] Color defaultLightColor;

    [SerializeField] Color TargetColor;

    private void Start()
    {
        defaultDarkColor = m_RenderersDark[0].material.color;
        defaultLightColor = m_RenderersLight[0].material.color;
        TargetColor = Color.Lerp(defaultLightColor, Color.white, 0.5f);
    }

    bool changeFirstLight;
    bool changeFirstDark;

    bool changeSecondLight;
    bool changeSecondDark;

    private void ColorChange(MeshRenderer[] array, Color color, ref bool cont)
    {
        float speed = Time.fixedDeltaTime * 25;
        foreach (var item in array)
        {
            Color matColor = Color.Lerp(item.material.color, color, speed);
            if (matColor != TargetColor)
            {
                item.material.color = matColor;
                cont = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (m_ChangeMatColor)
        {
            if (!changeFirstLight && !changeFirstDark)
            {
                changeFirstLight = true;
                changeFirstDark = true;
                ColorChange(m_RenderersLight, TargetColor, ref changeFirstLight);
                ColorChange(m_RenderersDark, TargetColor, ref changeFirstDark);

            }
            else if (!changeSecondLight && !changeSecondDark)
            {
                changeSecondLight = true;
                changeSecondDark = true;
                ColorChange(m_RenderersLight, defaultLightColor, ref changeSecondLight);
                ColorChange(m_RenderersDark, defaultDarkColor, ref changeSecondDark);
            }
            else
            {
                m_ChangeMatColor = false;
            }
        }

        if (Bomb != null)
        {
            if (Vector3.Distance(Bomb.transform.position, transform.position) <= 2.5f)
            {
                m_ChangeMatColor = true;
                foreach (var item in Bricks)
                {
                    item.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
    }
}
