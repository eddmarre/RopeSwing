using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private float thrustForce = 1000f;

    private HingeJoint[] _ropeJoints;
    private GameObject _bottomRope;
    private Rigidbody _bottomRopeRigidBody;
    private Transform _playerTransformLocation;

    private void Awake()
    {
        _ropeJoints = GetComponentsInChildren<HingeJoint>();
        _bottomRope = _ropeJoints[_ropeJoints.Length - 1].gameObject;
        _bottomRopeRigidBody = _bottomRope.GetComponent<Rigidbody>();
        _playerTransformLocation = _bottomRope.transform.Find("PlayerAttachTransform");
    }

    private void Start()
    {
        _bottomRopeRigidBody.AddForce(Vector3.forward * thrustForce);
    }

    public void PushRope()
    {
        _bottomRopeRigidBody.AddForce(Vector3.forward * thrustForce);
    }
    
    public Transform AttachPlayerToRope()
    {
        return _playerTransformLocation;
    }
}