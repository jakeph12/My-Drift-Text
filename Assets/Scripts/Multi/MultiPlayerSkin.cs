using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerSkin : MonoBehaviour
{
    [SerializeField] private GameObject m_gmSpoiler, m_gmBody;
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
