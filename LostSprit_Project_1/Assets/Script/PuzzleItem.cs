using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItem : MonoBehaviour
{
    public enum Type { rockitem };
    public Type type;

    Rigidbody rigid;
    //SphereCollider sphereCollider;
    Material mat;

    void Awake()    //�ʱ�ȭ
    {
        rigid = GetComponent<Rigidbody>();
        mat = GetComponent<MeshRenderer>().material;
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
        if (collision.gameObject.tag == "fireplayer")
        {
            mat.color = Color.red;
        }
        if (collision.gameObject.tag == "waterplayer")
        {
            mat.color = Color.blue;
        }
    }
}
