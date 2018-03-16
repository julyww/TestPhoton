using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
public class PhotonChildTransformView : MonoBehaviour, IPunObservable
{

    public Transform targetTransform;
    private PhotonView m_PhotonView;


    [SerializeField]
    PhotonTransformViewPositionModel m_PositionModel = new PhotonTransformViewPositionModel();
    [SerializeField]
    PhotonTransformViewRotationModel m_RotationModel = new PhotonTransformViewRotationModel();


    private PhotonTransformViewPositionControl m_PositionControl;

    public PhotonTransformViewRotationControl[] m_RotationControlArray;
    private Transform[] m_ChildTransformArray;
    void Awake() {
        this.m_PhotonView = GetComponent<PhotonView>();       
        this.m_PositionControl = new PhotonTransformViewPositionControl(this.m_PositionModel);

        //处理多个子物体的旋转
        m_ChildTransformArray = targetTransform.GetComponentsInChildren<Transform>();
        Debug.Log(m_ChildTransformArray.Length);
        m_RotationControlArray = new PhotonTransformViewRotationControl[m_ChildTransformArray.Length];
        for (int i = 0; i < m_RotationControlArray.Length; i++)
        {
            m_RotationControlArray[i] = new PhotonTransformViewRotationControl(this.m_RotationModel);
        }


    }
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (this.m_PhotonView == null || this.m_PhotonView.isMine == true || PhotonNetwork.connected == false)
        {
            return;
        }
        this.UpdatePosition();
        this.UpdateRotation();
    }

    void UpdatePosition()
    {
        if (this.m_PositionModel.SynchronizeEnabled == false)
        {
            return;
        }

        targetTransform.localPosition = this.m_PositionControl.UpdatePosition(targetTransform.localPosition);
    }
    void UpdateRotation()
    {
        if (this.m_RotationModel.SynchronizeEnabled == false )
        {
            return;
        }
        
        for (int i = 0; i < m_ChildTransformArray.Length; i++)
        {
            m_ChildTransformArray[i].localRotation = m_RotationControlArray[i].GetRotation(m_ChildTransformArray[i].localRotation);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        this.m_PositionControl.OnPhotonSerializeView(targetTransform.localPosition, stream, info);

        for (int i = 0; i < m_ChildTransformArray.Length; i++)
        {
            m_RotationControlArray[i].OnPhotonSerializeView(m_ChildTransformArray[i].localRotation, stream, info);
        }

        if (stream.isReading == true)
        {
            if (this.m_PositionModel.SynchronizeEnabled)
            {
                this.targetTransform.localPosition = this.m_PositionControl.GetNetworkPosition();
            }
            for (int i = 0; i < m_ChildTransformArray.Length; i++)
            {
                m_ChildTransformArray[i].localRotation = m_RotationControlArray[i].GetNetworkRotation();
            }
        }
    }

}
