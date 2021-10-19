using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float[] cntitem;

    [SerializeField]
    public GameObject[] itemObj;
    public Transform[] itemPos;

    [SerializeField]
    private string attr;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;

    [SerializeField] // 달리기 속도 조절
    private float runSpeed;
    private float applySpeed;

    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce;

    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    private CapsuleCollider capsuleCollider;

    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    public GameObject MenuCam;
    //  public GameObject FireCam;
    public PlayerController WaterPlayer;
    public PlayerController FirePlayer;
    public GameObject Panel;
    public GameObject UIPanel;
    public GameObject Door;
    public GameObject OpenDoor;

    [SerializeField]
    private Transform cameraArm;


    bool ischeck;
    bool isBorder;
    bool eDown;
    bool rDown;
    bool save;
    bool OneDown;
    bool TwoDown;
    private int locitem;
    GameObject nearObject;
    Rigidbody rigid;
    Material mat;

    NetworkManager _network;

    public MyPlayer myPlayer;
    public int PlayerId { get; set; }
    //public GameObject cubeItem { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        mat = GetComponent<MeshRenderer>().material;
        applySpeed = walkSpeed;

        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;

        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        startGame();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        IsGround();
        TryRun();
        TryJump();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
        //LookAround();
        Interation();
        ItemSelect();
        Drop();
       // Debug.Log(nearObject + "  :  " + nearObject.tag + "   : update");
    }
    private void gameOver(string attr)
    {
        switch (attr)
        {
            case "water":
                Invoke("water", 0.2f);
                break;
            case "fire":
                Invoke("fire", 0.2f);
                break;
        }
        
    }


    private void startGame()
    {
        if(attr == "fire")  
            _network. FirePlayerStart();
        else if(attr == "water")
            _network.WaterPlayerStart();
    }

    void GetInput()
    {   //키입력 이벤트
        eDown = Input.GetButtonDown("Interation");
        rDown = Input.GetButtonDown("Drop");
        OneDown = Input.GetButtonDown("One");
        TwoDown = Input.GetButtonDown("Two");
    }
    private void Move()
    {
        if (!ischeck)
        {
            float _moveDirX = Input.GetAxisRaw("Horizontal");
            float _moveDirZ = Input.GetAxisRaw("Vertical");

            Vector3 _moveHorizontal = transform.right * _moveDirX;
            Vector3 _moveVertical = transform.forward * _moveDirZ;

            Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

            Vector3 v = transform.position + _velocity * Time.deltaTime;

            myRigid.MovePosition(v);
            // 낙사
            if (v.y < -100.0f)
            {
                gameOver(attr);
            }

            // 서버에 움직임 보내줌
            _network.MovePlayer(v.x, v.y, v.z);
        }
    }
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
    private void CharacterRotation()
    {//좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));


        Quaternion rot = myRigid.rotation;
        //Debug.Log(myRigid.rotation);
        //Debug.Log(myRigid.rotation.eulerAngles);


    }
    void FreezeRotation() //회전방지(캐릭터가 물체와 닿았을 때 의도치 않게 회전되는 오류현상 방지)
    {
        myRigid.angularVelocity = Vector3.zero;   //회전속도를 0으로 바꿈 > 스스로 도는현상 X 
    }
    void FixedUpdate()  //함수실행
    {
        FreezeRotation();
        StopToWall();
    }
    private void CameraRotation()
    {//상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);

    }


    IEnumerator CrouchCoroutine()
    {

        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancle();
        }
    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;

    }
    private void RunningCancle()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (isCrouch)
            Crouch();

        myRigid.velocity = transform.up * jumpForce;

    }
    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "FireFloor" && attr == "water")
        //{
        //    Invoke("water", 0.4f);
        //}
        //if (collision.gameObject.tag == "WaterFloor" && attr == "fire")
        //{
        //    Invoke("fire", 0.4f);
        //}
        //if (collision.gameObject.tag == "PoisonFloor")
        //{
        //    if (attr == "water")
        //    {
        //        Invoke("water", 0.4f);
        //    }
        //    else if (attr == "fire")
        //    {
        //        Invoke("fire", 0.4f);
        //    }
        //}
    }
    void water()
    {
        Panel.SetActive(true);
        MenuCam.SetActive(true);
        WaterPlayer.gameObject.SetActive(false);
        UIPanel.SetActive(false);
        gameObject.transform.position = new Vector3(0, 1, 0);
    }
    void fire()
    {
        Panel.SetActive(true);
        MenuCam.SetActive(true);
        UIPanel.SetActive(false);
        FirePlayer.gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(0, 1, 5);
    }

    void StopToWall()
    {
        //Scene 내에서 Ray를 보여주는 함수 / 시작위치 / 쏘는방향 / Ray의 길이 / 색깔
        Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
        // Wall이라는 LayerMask를 가진 물체랑 충돌하면 bool값이 true로 바뀜
        isBorder = Physics.Raycast(transform.position, transform.forward, 2, LayerMask.GetMask("Wall"));
    }
    void Interation()
    {
        if (eDown && nearObject != null)
        {
            
            if (nearObject.tag == "rockitem")
            {               
                cntitem[1] = cntitem[1] + 1;

                //상호작용한 물체를 삭제함
                Destroy(nearObject);
                Debug.Log(nearObject + "  :  " + nearObject.tag);
                
                //_network.DestroyObject("rockitem");
            }
            if (nearObject.tag == "Key")
            {
                cntitem[0] = cntitem[0] + 1;
                Destroy(nearObject);    //상호작용한 물체를 삭제함
            }
            //&& cntitem[0] != 0
            if (nearObject.tag == "Door" && cntitem[0] != 0)
            {
                OpenDoor.gameObject.SetActive(true);
                Destroy(Door);
                //_network.DestroyObject("Door");                
                
            }

            //Debug.Log(nearObject + "  :  " + nearObject.tag + "e");
        }
        

    }
    void ItemSelect()
    {
        if (OneDown)
        {
            locitem = 0;
        }
        if (TwoDown)
        {
            locitem = 1;
        }
    }

    /*  public void DestroyItemEvent(string tag)
      {
          if (tag == "rockitem")
          {  
              Destroy(cubeItem);
          }
          if (tag == "Door")
          {
              OpenDoor.gameObject.SetActive(true);
              Destroy(Door);
          }
      }*/
    void Drop()
    {
        if (rDown && !isBorder && cntitem[locitem] != 0)
        {
            Instantiate(itemObj[locitem], itemPos[0].position, Quaternion.identity);
            cntitem[locitem] -= 1;
           // Debug.Log(nearObject + "  :  " + nearObject.tag + "drop");
        }
    }
    

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "FireFloor" && attr == "water")     //아이템과 닿았을 때
        {
            Invoke("fire", 0.2f);
        }
        if (other.tag == "WaterFloor" && attr == "fire")
        {
            Invoke("water", 0.2f);
        }       

        if (other.tag == "PoisonFloor")
        {
            if (attr == "water")
            {
                Invoke("water", 0.2f);
            }
            else if (attr == "fire")
            {
                Invoke("fire", 0.2f);
            }
        }
        if(other.tag == "WaitFloor")
        {
            ischeck = true;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "rockitem")
            nearObject = other.gameObject;
        else if (other.tag == "Door")
            nearObject = other.gameObject;
        else if (other.tag == "Key")
            nearObject = other.gameObject;
        else if (other.tag == "WaitFloor")
            nearObject = other.gameObject;

    }
    void OnTriggerExit(Collider other)
    {
        nearObject = null;
        ischeck = false;
    }
}

