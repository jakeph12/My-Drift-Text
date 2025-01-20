using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class WindowUi : MonoBehaviour
{
    public Action m_acOnInit;
    [HideInInspector]
    public Vector2 m_vcStartPos;
    private CanvasGroup m_cvGroop;
    private TweenerCore<float, float, FloatOptions> m_anMain;

    protected virtual void Awake()
    {
        m_cvGroop = GetComponent<CanvasGroup>();
    }

    public virtual void DellObj(float time = 1)
    {
        if (m_anMain != null)
        {
           
            m_anMain.Kill();
        }
        m_cvGroop.DOFade(0, time).OnComplete(() =>
        {
            gameObject.SetActive(false);

        });

    }
    public virtual void Open(float time = 1)
    {
        gameObject.SetActive(true);
        m_acOnInit?.Invoke();
        if (!m_cvGroop) m_cvGroop = GetComponent<CanvasGroup>();
        m_cvGroop.alpha = 0;
        m_anMain = m_cvGroop.DOFade(1, time).OnComplete(() =>
        {
            m_anMain = null;
        });

    }
    public virtual void Init()
    {
        
    }
    public virtual void OpenOther(WindowUi wi)
    {
        DellObj();
        wi.Open();
    }
    public virtual void OpenOtherN(WindowUi wi)
    {
        wi.Open();
    }
    public virtual void OnDestroy()
    {
    }

}
