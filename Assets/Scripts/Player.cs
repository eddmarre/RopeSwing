using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float resetPlayerAfterSwingTimer = 1.5f;
    [SerializeField] private float playerUpwardsFoceAfterSwing = 100f;
    [SerializeField] private float playerForceAfterSwing = 200f;
    private PlayerInputAction _playerInput;
    private bool isTouched;
    private bool canPerform = true;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private Transform originalParent;
    private WaitForSeconds resetTimer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        resetTimer = new WaitForSeconds(resetPlayerAfterSwingTimer);

        _playerInput = new PlayerInputAction();
        _playerInput.Player.Jump.performed += context => isTouched = true;
        _playerInput.Player.Jump.canceled += context => isTouched = false;
    }

    private void Update()
    {
        Debug.Log(_rigidbody.GetRelativePointVelocity(transform.localPosition));
        if (isTouched && canPerform)
        {
            StartCoroutine(ResetDelayCollision());
        }
    }


    IEnumerator ResetDelayCollision()
    {
        canPerform = false;
        transform.SetParent(null);
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce((Vector3.forward * playerForceAfterSwing) +
                            (Vector3.up * playerUpwardsFoceAfterSwing));
        yield return resetTimer;
        _capsuleCollider.enabled = true;
        canPerform = true;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDestroy()
    {
        _playerInput.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            var ropePlayerAttachLocation =
                collision.gameObject.GetComponentInParent<Rope>().AttachPlayerToRope();
            var myTransform = transform;
            _rigidbody.isKinematic = true;
            _capsuleCollider.enabled = false;
            myTransform.position = ropePlayerAttachLocation.position;
            myTransform.rotation = ropePlayerAttachLocation.rotation;
            myTransform.SetParent(ropePlayerAttachLocation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeadZone"))
        {
            transform.position = new Vector3(0, 1, 0);
        }
    }
}