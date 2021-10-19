using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { rockitem };
    public Type type;

    Rigidbody rigid;
    //SphereCollider sphereCollider;

    void Awake()    //초기화
    {
        rigid = GetComponent<Rigidbody>();
        //오브젝트의 콜라이더중 첫번째것만 가져오므로 is Trigger가 포함되지 않은 콜라이더가 위로 올라가야함
        //sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "rockItem")
        {
            rigid.isKinematic = true;   //더이상 외부 물리효과에 의해서 움직이지 못함
                                        // sphereCollider.enabled = false;
        }
    }
}
