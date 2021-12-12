using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 20;

    [SerializeField]
    float _health = 100;
    public float health {get => _health; set => _health = Mathf.Clamp(value,0,100);}

    Rigidbody rigidbody;
    Blackboard blackboard = new Blackboard();
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        BlackboardServiceDesk.AddBlackboard("Player",blackboard);
        blackboard.SetVariable("self",gameObject);
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;

        movement += Input.GetKey(KeyCode.S) ? Vector3.right : Vector3.zero;
        movement += Input.GetKey(KeyCode.W) ? Vector3.left : Vector3.zero;
        movement += Input.GetKey(KeyCode.A) ? Vector3.back : Vector3.zero;
        movement += Input.GetKey(KeyCode.D) ? Vector3.forward : Vector3.zero;

        rigidbody.velocity = movement*Time.deltaTime*speed*10;
    }
}
