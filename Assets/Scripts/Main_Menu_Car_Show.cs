using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Plugins.Options;

public class Main_Menu_Car_Show : MonoBehaviour
{
    public static Main_Menu_Car_Show m_sinThis;
    [SerializeField] private GameObject[] m_gmCameraPos;
    public Skin_Loader[] m_gmCars;

    private void Awake()
    {
        m_sinThis = this;
    }

    void Start()
    {
        Camera.main.transform.position = m_gmCameraPos[PlayerInfo.m_IntCurrentId].transform.position;
    }
    private TweenerCore<Vector3, Vector3, VectorOptions> m_inMain;

    public Skin_Loader OnIdCh(int id)
    {
        if (m_inMain == null) m_inMain.Kill();

        m_inMain = Camera.main.transform.DOMove(m_gmCameraPos[id].transform.position, 1).OnComplete(()=> m_inMain = null);
        return m_gmCars[id];
    }
    public void OnDestroy()
    {
    }
}
