using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {
    public GameObject player;
    public enum CurrentState { idle, trace, attack, dead };
    public CurrentState curState = CurrentState.idle;

    private Transform transform;
    //private Transform playerTransform;
    private NavMeshAgent nvAgent;

    public float traceDist = 15.0f;
    public float attackDist = 30.0f;
    private bool isDead = false;

    void start()
    {
        transform = this.gameObject.GetComponent<Transform>();
        //playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheackStateForACtion());
    }

    IEnumerator CheckState()
    {
        while(!isDead)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if(dist <= attackDist)
            {
                curState = CurrentState.attack;
            }
            else if(dist <= traceDist)
            {
                curState = CurrentState.trace;
            }
            else
            {
                curState = CurrentState.idle;
            }
        }
    }

    IEnumerator CheackStateForACtion()
    {
        while(!isDead)
        {
            switch(curState)
            {
                case CurrentState.idle:
                    nvAgent.Stop();
                    break;
                case CurrentState.trace:
                    nvAgent.destination = player.transform.position;
                    nvAgent.Resume();
                    break;
                case CurrentState.attack:
                    break;
            }
            yield return null;
        }
    }

}
