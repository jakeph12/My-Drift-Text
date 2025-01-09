using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin_Loader : MonoBehaviour
{
    [SerializeField] private GameObject m_gmSpoiler,m_gmBody;
    private bool _m_bSpoiler;
    private float m_flR, m_flG, m_flB;

    public bool m_bSpoiler
    {
        get => _m_bSpoiler;
        set
        {
            _m_bSpoiler = value;
            m_gmSpoiler.SetActive(_m_bSpoiler);
            PlayerPrefs.SetInt($"{gameObject.name} Spoiler", _m_bSpoiler ? 1 : 0);
        }
    }
    private void Awake()
    {
        //Debug.Log(gameObject.name);
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        //Debug.Log(gameObject.name);

        _m_bSpoiler = PlayerPrefs.GetInt($"{gameObject.name} Spoiler",1) == 1;
        m_gmSpoiler.SetActive(_m_bSpoiler);
        var R = PlayerPrefs.GetFloat($"{gameObject.name} R", 1f);
        var G = PlayerPrefs.GetFloat($"{gameObject.name} G", 1f);
        var B = PlayerPrefs.GetFloat($"{gameObject.name} B", 1f);
        SetColor(R, G, B);

        //Debug.Log($"{R}, {G}, {B}");

    }

    public void SetColor(float R,float G,float B)
    {
        m_flR = R;
        m_flG = G;
        m_flB = B;

        m_gmBody.GetComponent<Renderer>().material.color = new Color(m_flR, m_flG, m_flB);
    }
    public void Save(float R, float G, float B)
    {
        PlayerPrefs.SetFloat($"{gameObject.name} R", R);
        PlayerPrefs.SetFloat($"{gameObject.name} G", G);
        PlayerPrefs.SetFloat($"{gameObject.name} B", B);
    }
    public Color GetColor()
    {
        return new Color(m_flR, m_flG, m_flB);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
