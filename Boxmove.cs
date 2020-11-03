using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxmove : MonoBehaviour
{
    Rigidbody rb;
    Transform tr;
    BoxCollider bc;
    float x, z;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // tr.position = (rb.position + new Vector3(x, 0, z) * Time.deltaTime * 5); 
        //      ※transform.position: 많이 느리다. => 모든 콜라이더들이 리지드 바디의 위치들을 재계산하기 때문에
        // rb.position = (rb.position + new Vector3(x, 0, z) * Time.deltaTime * 5); 
        //      ※rb.position: 많이 빠르다.  => tr.position보다 10배 이상 더 빠르다. // 순간이동
        // rb.MovePosition(rb.position + new Vector3(x,0,z) * Time.deltaTime * 5);
        //      ※추가로 지속적인 움직임은 rb.MovePosition으로 조정하라고 권유 // 그냥 이동, 다른 충돌체 밀침
        // rb.velocity = new Vector3(x * 5f, 0, z * 5f);
        //      ※ rb.velocity : rigidbody의 속도. => velocity를 지정하면 오브젝트의 질량과 상관없이 일정 속도를 지정
        // rigidbody2d 컴포넌트가 있어야만 velocity를 사용할 수 있습니다.아래와 같이 x,y 의 속도를 지정할 수 있습니다.

        if(Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(new Vector3(0, 10f, 0), ForceMode.Impulse);
        //  rb.AddForce(Vector3, N) : rigidbody에 힘을 가해 가속도를 준다. F=ma 적용됨
        //  N의 종류는 아래와 같다.
        //      ForceMode.Force: 역학적인 개념의 힘을 rigidbody에 적용, 주로 바람이나 자기력등 연속적인 힘을 나타냄
        //      ForceMode.Impulse: 충격량을 리지드바디에 주는 모드로 충격량이랑 힘의 크기와 주는 시간을 곱한 수치다. 타격이나 폭발, 순간적인 힘 표현
        //      ForceMode.Acceleration: 리지드바디가 갖는 질량을 무시하고 직접적으로 가속량을 주는 모드다. 시간이 흘러가며 변화줌
        //      ForceMode.VelocityChange: 리지드바디가 가진 질량을 무시하고 직접적으로 속도의 변화를 주는 모드다. 시간흐름없이 팍! 줌

        rb.MovePosition(rb.position + new Vector3(x, 0, z) * Time.deltaTime * 5);

        if (Input.GetKeyDown(KeyCode.Escape))
            Destroy(gameObject);

        
    }
}
