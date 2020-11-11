using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    public float jumpPower = 30;
    public float rotateSpeed;
    public int itemCount;
    public GameManager manager;
    int isJump = 0;
    Rigidbody rigid;
    AudioSource audio;


    void Awake()
    {
        isJump = 0;
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isJump != 2)
        {
            isJump++;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            isJump = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            audio.Play();
            other.gameObject.SetActive(false);
            manager.GetItem(itemCount);
        }
        else if(other.tag == "Goal")
        {
            if(itemCount == manager.totalItemCount)
            {
                if(manager.stage == 1)
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    SceneManager.LoadScene(manager.stage+1);
                }
                
            }
            else
            {
                SceneManager.LoadScene(manager.stage);
            }
        }
    }
}
