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
    public Action m_ActionCallBack;
    private Vector2 m_SrartPosition;
    Rigidbody m_CarRigidbody;
    private float _m_CarSpeed;
    private int m_inMaxSpeed = 50;
    private float m_CarSpeed
    {
        get => _m_CarSpeed;
        set
        {
            _m_CarSpeed = value;
            if (_m_CarSpeed > 1) _m_CarSpeed = 1;
            m_EventOnSpeedCh?.Invoke(_m_CarSpeed);
        }
    }
    public delegate void OnFloatChange(float value);
    public OnFloatChange m_EventOnSpeedCh;
    private bool _m_bIfOnDrift;
    public bool m_bIfOnDrift
    {
        get => _m_bIfOnDrift;
        set
        {
            if (_m_bIfOnDrift == value) return;

            _m_bIfOnDrift = value;
            if(_m_bIfOnDrift)
            {
                m_DriftAndTime.m_clToken = new CancellationTokenSource();
                m_DriftAndTime.SetTimer().Forget();
            }
            else
            {
                m_DriftAndTime.m_clToken.Cancel();
            }
        }
    }

    private bool m_IfHeDrifting = false;
    private Vector3 m_vcStart;
    private Quaternion m_qrStart;
    public static Main_Car_Controller m_staticThis;
    private TimeAndDriftCounter m_DriftAndTime;


    private async void SetDrift(bool cur)
    {
        if (!m_IfHeDrifting && !cur) return;

        m_IfHeDrifting = cur;
        if (!cur && m_bIfOnDrift)
        {
            await UniTask.WhenAny(UniTask.WaitUntil(() => m_IfHeDrifting), UniTask.Delay(3000));
            if (m_IfHeDrifting) return;
        }

        m_bIfOnDrift = m_IfHeDrifting;

    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        m_staticThis = this;
        m_DriftAndTime = TimeAndDriftCounter.m_staticThis;
        m_CarRigidbody = GetComponent<Rigidbody>();

        m_CarRigidbody.velocity = Vector3.zero;
        m_vcStart = transform.position;
        m_qrStart = transform.rotation;
    }

    public void Update()
    {
        m_SrartPosition = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
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
        Vector3 velocity = m_CarRigidbody.velocity;
        float forwardDot = Vector3.Dot(transform.forward, velocity.normalized);

        bool isMovingForward = forwardDot > 0;
        float angle = Vector3.Angle(transform.forward, velocity);
        bool isSpeedEnough = velocity.magnitude > 5f;

        if (isMovingForward)
        {
            SetDrift(angle > 15f && isSpeedEnough);
        }
        else
        {
            SetDrift(angle > 25f && isSpeedEnough);
        }
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
        m_CarSpeed =  m_CarRigidbody.velocity.magnitude / m_inMaxSpeed;
    }
    public void WhillesMove()
    {
        m_clfrontLeftWheel.steerAngle = m_flMaxSteerAngle * m_SrartPosition.y;
        m_clfrontRightWheel.steerAngle = m_flMaxSteerAngle * m_SrartPosition.y;
        if (m_SrartPosition.x == 0)
        {
            m_clfrontLeftWheel.brakeTorque = m_inBrakeTorque / 5;
            m_clfrontRightWheel.brakeTorque = m_inBrakeTorque / 5;
        }
        else
        {
            m_clfrontLeftWheel.brakeTorque = 0;
            m_clfrontRightWheel.brakeTorque = 0;
        }

        if (m_CarRigidbody.velocity.magnitude >= 40)
        {
            m_clfrontLeftWheel.motorTorque = 0;
            m_clfrontRightWheel.motorTorque = 0;
            return;
        }

        m_clfrontLeftWheel.motorTorque = m_inMottorTorque * m_SrartPosition.x;
        m_clfrontRightWheel.motorTorque = m_inMottorTorque * m_SrartPosition.x;
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

        if (m_DriftAndTime.m_clStatTimer != null)
            m_DriftAndTime.m_clStatTimer.Cancel();

        m_DriftAndTime.m_inSecond = 1;
        m_DriftAndTime.m_inMinute = 1;

        m_DriftAndTime.m_clStatTimer = new CancellationTokenSource();

        m_DriftAndTime.StartTimer().Forget();
    }
    public void EndGame(bool End = true)
    {
        enabled = false;
        m_bIfOnDrift = false;
        if(m_DriftAndTime.m_clStatTimer != null)
            m_DriftAndTime.m_clStatTimer.Cancel();
        m_clfrontLeftWheel.brakeTorque = 6000;
        m_clfrontRightWheel.brakeTorque = 6000;
        m_clfrontLeftWheel.motorTorque = 0;
        m_clfrontRightWheel.motorTorque = 0;
        m_CarRigidbody.velocity = Vector3.zero;
        Ui_Controller.m_staticThis.m_txTotalScore.gameObject.SetActive(false);
    }
}
