using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    /* 변수 선언 */

    // <기본 움직임>
    public float walkSpeed = 30f; //인스펙터 창에서 설정할 수 있도록 public으로 설정
    public float runSpeed = 40f;
    public float jumpForce = 10f;
    public float rotateSpeed = 4f;

    float applySpeed;
    float hAxis;
    float vAxis;

    bool wDown;
    bool jDown;
    bool isJump;
    bool isRun = false;

    Vector3 moveVec;
    Vector3 moveH;
    Vector3 moveV;
    Vector3 velocity;

    Rigidbody rigid;
    Animator anim;

    // <공격>
    public GameObject[] weapons;
    Weapon equipWeapon;
    bool fDown; //키입력
    bool isFireReady; //준비완료
    float fire; //공격
    float fireDelay; //딜레이

    // <체력>
    public Slider Hp;
    float high = 0f; // 높이 가져오는 변수
    float dist = 0f; // 거리 측정 변수


    // <카메라>
    //민감도
    [SerializeField]
    private float lookSensitivity = 2f;

    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit = 25f;
    private float currentCameraRotationX = 10f;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;

    void Awake()
    {
        //마우스 창에 가두는 코드
        Cursor.lockState = CursorLockMode.Confined;

        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        equipWeapon = weapons[0].GetComponent<Weapon>();
        applySpeed = runSpeed;
    }

    void Update()
    {
        GetInput();
        Move();
        Jump();
        Walk();
        Attack();
        CharacterRotation();
        CameraRotation();
        HpDown();
        if (Hp.value == 0)
        {
            SceneManager.LoadScene("gameover");
        }
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
    }

    /*움직임 관련 함수*/

    //기본 움직임 함수
    void Move()
    {
        //움직임 벡터
        //normalized : 방향 값이 1로 보정된 벡터
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        moveH = transform.right * hAxis;
        moveV = transform.forward * vAxis;

        velocity = (moveH + moveV).normalized * Time.deltaTime;

        rigid.MovePosition(transform.position + velocity * applySpeed);

        //애니메이션 효과 주기(Walk setting에서 키 추가해줌)
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    // 점프 함수
    void Jump()
    {          //↓ =>  + && moveVec == Vector3.zero 였었다.
        if (jDown && !isJump)
        {
            rigid.velocity = transform.up * jumpForce;
            anim.SetTrigger("doJump");
            Set_JumpState(true);
        }
    }
    //점프 애니메이션 바닥 체크 함수
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")   //tag Floor 는 Terrain으로 설정함
        {
            Set_JumpState(false);
            if (dist - high > 0)
            {
                Hp.value -= 10f;
                dist = 0f;
            }
        }
    }
    void Set_JumpState(bool TorF)
    {
        isJump = TorF;
        anim.SetBool("isJump", TorF);
    }

    // 걷기 함수
    private void Walk()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
            applySpeed = walkSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            applySpeed = runSpeed;
        }
    }


    /* 공격 관련 함수 */

    // 공격함수
    void Attack()
    {
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isJump)
        {
            equipWeapon.Use();
            anim.SetTrigger("doSwing");
            fireDelay = 0;
        }
    }


    /* 카메라 관련 함수 */

    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // 상하 카메라 회전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }


    /* 체력 관련 함수 */

    //높은 곳에서 떨어지면 체력이 닳는 함수
    void HpDown()
    {
        high = transform.position.y;
        if (high > 38) dist = high;
    }

    //물 닿으면 체력이 닳는 함수
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Water")
        {
            Hp.value -= 0.5f;
        }
    }
}