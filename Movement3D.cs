
using System.Threading;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    // 변수 선언
    // SerializeField : 인스펙터에서는 접근 가능하지만 외부 스크립트에서 접근이 불가능하게 막을 때 사용
    [SerializeField] 
    private float moveSpeed = 5.0f; //이동 속도
    [SerializeField]
    private float jumpForce = 3.0f; //뛰는 힘
    private float gravity = -9.81f; //중력 계수
    private Vector3 moveDirection;  //이동 방향

    [SerializeField]
   // private Transform cameraTransform; //카메라 Transform 컴포넌트
    private CharacterController characterController;

    private void Awake()
    {
        /*                                          내가 소속된 게임오브젝트의 정보    내가 아닌 다른게임오브젝트의 컴포넌트 정보
          게임오브젝트의 컴포넌트에 접근하는 방법 : GetComponent<컴포넌트 이름>();  ,  게임오브젝트.GetComponent<컴포넌트 이름>();
          권장하는 사용 방법
          1. 컴포넌트와 동일한 타입의 변수 생성   (Line 18)
          2. 컴포넌트 정보를 얻어와서 변수에 저장 (Line 31)
          3. 컴포넌트 정보가 저장된 변수를 사용   (Line 48)
          + 이렇게 클래스 변수 생성, 컴포넌트 정보 한번만 저장하면 현재 클래스 내부 어디에서든 
            컴포넌트 정보를 바꾸거나 얻어올 수 있다.
         */
        characterController = GetComponent<CharacterController>();
    }

    /* 
       characterController.Move(Vector3 motion);
       -매개변수로 이동방향, 속도, Time.deltaTime 등의 세부적인 이동방법을 설정하면 이동을 수행한다.
       characterController.SimpleMove(Vector3 Speed);
       -매개변수로 3 방향의 이동속도(=속력)를 넣어 호출하면 이동을 수행한다.
       characterController.isGrounded
       -발 위치의 충돌을 체크해 충돌이 되면 true, 충돌이 되지 않으면 false 값을 나타내는 변수
    */
    private void Update()
    {
        if(characterController.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime; //gravity 변수가 음수여서 더하면 y는 감소
        }
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);  //characterController.Move 함수 호출
    }

    public void MoveTo(Vector3 direction)
    {
        //↓중력의 값 적용 전
        //moveDirection = direction
        //↓중력의 값 적용 후, 중력값이 direction에 의해 바뀌지 않도록 / 3인칭 시점 코드
        moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
        //↓1인칭 시점으로 바꾼 코드
        //Vector3 movedis = cameraTransform.rotation * direction;
        //moveDirection = new Vector3(movedis.x, moveDirection.y, movedis.z);
    }

    public void JumpTo()
    {
        if(characterController.isGrounded == true) // 캐릭터가 바닥에 있으면 실행
        {
            moveDirection.y = jumpForce;
        }
    }
}
