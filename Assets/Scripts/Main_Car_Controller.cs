using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Main_Car_Controller : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider m_clfrontLeftWheel;
    [SerializeField] private WheelCollider m_clfrontRightWheel;
    [SerializeField] private WheelCollider m_clrearLeftWheel;
    [SerializeField] private WheelCollider m_clrearRightWheel;

    [Header("Wheel Transform")]
    [SerializeField] private Transform m_trfrontLeftWheel;
    [SerializeField] private Transform m_trfrontRightWheel;
    [SerializeField] private Transform m_trrearLeftWheel;
    [SerializeField] private Transform m_trrearRightWheel;


    [Space(10f)]
    [SerializeField] private int m_inMottorTorque = 1000;
    [SerializeField] private int m_inBrakeTorque = 3000;
    [SerializeField] private float m_flMaxSteerAngle = 30f;
    public Action m_acCallBack;
    private Vector2 m_vcPos;
    Rigidbody m_rbMain;
    private float _m_flSpeed;
    private int m_inMaxSpeed = 50;
    private float m_flSpeed
    {
        get => _m_flSpeed;
        set
        {
            _m_flSpeed = value;
            if (_m_flSpeed > 1) _m_flSpeed = 1;
            Ui_Controller.m_sinThis.m_gmSpeed.transform.rotation = Quaternion.Euler(new Vector3(0, 0,-276 * _m_flSpeed));
            //m_txMainSpeed.text = $"{(int)(130 * _m_flSpeed)}";
        }
    }

    private Text m_txMainSpeed;
    private bool _m_bDrift;
    [HideInInspector]public float m_flPrivS;
    public bool m_bDrift
    {
        get => _m_bDrift;
        set
        {
            if (_m_bDrift == value) return;

            _m_bDrift = value;
            if(_m_bDrift)
            {
                m_flPrivS = Time.time;
                Ui_Controller.m_sinThis.m_clToken = new CancellationTokenSource();
                Ui_Controller.m_sinThis.SetTimer().Forget();
            }
            else
            {
                Ui_Controller.m_sinThis.m_clToken.Cancel();
            }
        }
    }

    private bool m_bPrivD = false;
    private Vector3 m_vcStart;
    private Quaternion m_qrStart;
    public static Main_Car_Controller m_sinThis;
    [SerializeField] private WindowUi m_wiGetReward;


    private async void SetDrift(bool cur)
    {
        if (!m_bPrivD && !cur) return;

        m_bPrivD = cur;
        if (!cur && m_bDrift)
        {
            await UniTask.WhenAny(UniTask.WaitUntil(() => m_bPrivD), UniTask.Delay(3000));
            if (m_bPrivD) return;
        }

        m_bDrift = m_bPrivD;

    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        m_sinThis = this;
        m_rbMain = GetComponent<Rigidbody>();

        m_rbMain.velocity = Vector3.zero;
        m_vcStart = transform.position;
        m_qrStart = transform.rotation;
    }

    public void Update()
    {
        m_vcPos = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        if (Input.GetKey(KeyCode.Space))
        {
            m_clrearLeftWheel.brakeTorque = m_inBrakeTorque;
            m_clrearRightWheel.brakeTorque = m_inBrakeTorque;
        }
        else
        {
            m_clrearLeftWheel.brakeTorque = 0;
            m_clrearRightWheel.brakeTorque = 0;
        }
        SyncWheel();
        Drift();
    }

    private void Drift()
    {
        float angle = Vector3.Angle(transform.forward, GetComponent<Rigidbody>().velocity);
        SetDrift((angle > 15f && GetComponent<Rigidbody>().velocity.magnitude > 5f));
    }

    private void SyncWheel()
    {

        UpdateWheelTransform(m_clfrontLeftWheel, m_trfrontLeftWheel);
        UpdateWheelTransform(m_clfrontRightWheel, m_trfrontRightWheel);
        UpdateWheelTransform(m_clrearLeftWheel, m_trrearLeftWheel);
        UpdateWheelTransform(m_clrearRightWheel, m_trrearRightWheel);
    }
    public void FixedUpdate()
    {
        WhillesMove();
        m_flSpeed =  m_rbMain.velocity.magnitude / m_inMaxSpeed;
    }
    public void WhillesMove()
    {
        m_clfrontLeftWheel.steerAngle = m_flMaxSteerAngle * m_vcPos.y;
        m_clfrontRightWheel.steerAngle = m_flMaxSteerAngle * m_vcPos.y;
        if (m_vcPos.x == 0)
        {
            m_clfrontLeftWheel.brakeTorque = m_inBrakeTorque / 5;
            m_clfrontRightWheel.brakeTorque = m_inBrakeTorque / 5;
        }
        else
        {
            m_clfrontLeftWheel.brakeTorque = 0;
            m_clfrontRightWheel.brakeTorque = 0;
        }

        if (m_rbMain.velocity.magnitude >= 40)
        {
            m_clfrontLeftWheel.motorTorque = 0;
            m_clfrontRightWheel.motorTorque = 0;
            return;
        }

        m_clfrontLeftWheel.motorTorque = m_inMottorTorque * m_vcPos.x;
        m_clfrontRightWheel.motorTorque = m_inMottorTorque * m_vcPos.x;
    }
    private void UpdateWheelTransform(WheelCollider collider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }





    public void StartGame()
    {
        transform.rotation = m_qrStart;
        transform.position = m_vcStart;
        enabled = true;

        if (Ui_Controller.m_sinThis.m_clStatTimer != null)
            Ui_Controller.m_sinThis.m_clStatTimer.Cancel();

        Ui_Controller.m_sinThis.m_inSecond = 1;
        Ui_Controller.m_sinThis.m_inMinute = 1;

        Ui_Controller.m_sinThis.m_clStatTimer = new CancellationTokenSource();

        Ui_Controller.m_sinThis.StartTimer().Forget();
    }
    public void EndGame(bool End = true)
    {
        enabled = false;
        m_bDrift = false;
        if(Ui_Controller.m_sinThis.m_clStatTimer != null)
            Ui_Controller.m_sinThis.m_clStatTimer.Cancel();
        m_clfrontLeftWheel.brakeTorque = 6000;
        m_clfrontRightWheel.brakeTorque = 6000;
        m_clfrontLeftWheel.motorTorque = 0;
        m_clfrontRightWheel.motorTorque = 0;
        m_rbMain.velocity = Vector3.zero;
        if (m_wiGetReward && End)
        {
            var Ne = Main_Menu_Controller.m_sinThis.OpenWindow(m_wiGetReward, TypeWindow.Main).GetComponent<You_Get_Ui>();
            Debug.Log(Ui_Controller.m_sinThis.m_inCoin);
            Ne.Init(Ui_Controller.m_sinThis.m_inCoin,m_acCallBack);
        }
        Ui_Controller.m_sinThis.m_txTotalScore.gameObject.SetActive(false);
    }
}
