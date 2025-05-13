using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    public bool isCanMove = true;
    private int mouseY; 

    public static PlayerMove I;

    private void Awake()
    {
        I = this;
    }

    void Start()
    {
        transform.position = SaveSystem.saveFile.playerPosition;
        transform.rotation = SaveSystem.saveFile.playerRotation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    { 
        if (isCanMove && !PouseManuManager.isPoused)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("right", false);
            animator.SetBool("left", false);
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                animator.SetBool("isWalking", true);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position -= transform.forward * moveSpeed * Time.deltaTime;
                animator.SetBool("isWalking", true);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= transform.right * moveSpeed * Time.deltaTime;
                animator.SetBool("left", true);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * moveSpeed * Time.deltaTime;
                animator.SetBool("right", true);
            }

            transform.Rotate(new Vector3(0, Input.mousePositionDelta.x, 0) * rotationSpeed);
            mouseY = Mathf.Clamp(mouseY + (int)Input.mousePositionDelta.y*(int)rotationSpeed*6, 0,  Camera.main.pixelHeight);
            cameraTransform.localRotation = Quaternion.Lerp(Quaternion.Euler(60, 0, 0), Quaternion.Euler(-60, 0, 0), (float)mouseY / Camera.main.pixelHeight);
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero; 
        }
        else
        {
            SaveSystem.saveFile.playerPosition = transform.position;
            SaveSystem.saveFile.playerRotation = transform.rotation;
        }
    }
    

    public IEnumerator LerpPos(Vector3 target, float lerpTime)
    {
        float timer = 0;
        Vector3 start = transform.position;
        while (timer < lerpTime)
        {
            transform.position = Vector3.Lerp(start, target, timer/lerpTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator LerpRot(Quaternion target, float lerpTime)
    {
        float timer = 0;
        Quaternion start = Quaternion.Euler(0,transform.localRotation.eulerAngles.y,0);
        Quaternion cameraStart = Quaternion.Euler(cameraTransform.localRotation.eulerAngles.x,0,0);
        while (timer < lerpTime)
        {
            transform.localRotation = Quaternion.Lerp(start, Quaternion.Euler(0,target.eulerAngles.y,0), timer/lerpTime);
            cameraTransform.localRotation = Quaternion.Lerp(cameraStart, Quaternion.Euler(target.eulerAngles.x,0,0), timer/lerpTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    
    public IEnumerator IsCanMove(bool isCanMove)
    {
        yield return null;
        if(isCanMove) GetComponent<Rigidbody>().isKinematic = false;
        else GetComponent<Rigidbody>().isKinematic = true;
        this.isCanMove = isCanMove;
    }
    public IEnumerator IsCanMove(bool isCanMove, float delay)
    {
        float time = 0;
        while (time <= delay)
        {
            yield return null;
            time += Time.deltaTime;
        }
        if(isCanMove) GetComponent<Rigidbody>().isKinematic = false;
        else GetComponent<Rigidbody>().isKinematic = true;
        this.isCanMove = isCanMove;
    }
}