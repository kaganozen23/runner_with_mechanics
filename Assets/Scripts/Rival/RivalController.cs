using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RivalController : MonoBehaviour //PlayerMove (will inherit PlayerMove later)
{

    public GameObject goal;
    public static GameObject RivalSelfObject;
    public Animator MyAnimator;
    
       
    void Start () 
    {
        MyAnimator = GetComponentInChildren<Animator>();
        RivalSelfObject = this.gameObject;
        transform.position = new Vector3(Random.Range(-4.0f,4.0f),0.5f,-8.0f);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.transform.position;
        MyAnimator.Play("Base Layer.Run Forward");
    }
}
