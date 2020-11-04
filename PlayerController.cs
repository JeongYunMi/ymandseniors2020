using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //변수 선언
    [SerializeField]
    private KeyCode jumKeyCode = KeyCode.Space;
    //[SerializeField]
    //private CameraController cameraController;
    private Movement3D movement3D;

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();
    }

    void Update()
    {
        // x,z 방향 이동
        float x = Input.GetAxisRaw("Horizontal"); // 방향키 좌,우 움직임
        float z = Input.GetAxisRaw("Vertical"); // 방향키 위,아래 움직임
        movement3D.MoveTo(new Vector3(x, 0, z)); // x,z의 좌표를 MoveTo 함수로 보내주기


        // 점프키를  눌러 y축 방향으로 뛰어 오르기 (Jump)
        if (Input.GetKeyDown(jumKeyCode))
        {
            movement3D.JumpTo();
        }


        //카메라컨트롤러 함수 사용
        //float mouseX = Input.GetAxis("Mouse X"); // 마우스 좌,우 움직임
        //float mouseY = Input.GetAxis("Mouse Y"); // 마우스 위,아래 움직임
        //cameraController.RotateTo(mouseX, mouseY);
    }
}