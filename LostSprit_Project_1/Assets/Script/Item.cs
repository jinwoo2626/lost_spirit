using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { rockitem };
    public Type type;

    Rigidbody rigid;
    //SphereCollider sphereCollider;

    void Awake()    //�ʱ�ȭ
    {
        rigid = GetComponent<Rigidbody>();
        //������Ʈ�� �ݶ��̴��� ù��°�͸� �������Ƿ� is Trigger�� ���Ե��� ���� �ݶ��̴��� ���� �ö󰡾���
        //sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "rockItem")
        {
            rigid.isKinematic = true;   //���̻� �ܺ� ����ȿ���� ���ؼ� �������� ����
                                        // sphereCollider.enabled = false;
        }
    }
}
