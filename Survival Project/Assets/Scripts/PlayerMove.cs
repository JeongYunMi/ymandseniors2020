using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //스피드 조정 변수
    public float walkSpeed;
    public float jumpForce;

    private bool isGround = true;

    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    //민감도
    public float lookSensitivity;

    //카메라 한계
    public float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    public Camera theCamera;

    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        Move();
        CameraRotation();
        ChararcterRotation();
        UnityEngine.Debug.DrawRay(transform.position, Vector3.forward, Color.blue, 0.3f);
    }

    //지면 체크
    private void IsGround()
    { 
        isGround = Physics.Raycast((myRigid.position + new Vector3(0, 0.1f, 0)), Vector3.down,0.2f);
        //         Physics.Raycast(Ray 원점, Ray 방향, 충돌 감지할 RaycastHit, Ray 거리(길이))
    }
    //점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    //점프
    private void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
    }
    //움직임 실행
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime * 5);

    }
    //좌우 캐릭터 회전
    private void ChararcterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    private void CameraRotation() // 상하 카메라 회전
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

}