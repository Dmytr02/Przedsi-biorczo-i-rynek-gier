using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public UnityEvent OnInteract;
    [SerializeField] private float InteractDistance = 3;
    public GameObject Text;

    void Update()
    {
        if (PlayerMove.I.isCanMove && !PouseManuManager.isPoused)
        {
            if (Vector3.Distance(PlayerMove.I.transform.position, transform.position) < InteractDistance)
            {
                Text.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OnInteract.Invoke();
                }
            }
            else{
                Text.SetActive(false);
            }
        }
        else
        {
            Text.SetActive(false);
        }
    }
}
