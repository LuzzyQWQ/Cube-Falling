﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f;
    public float minOffset = 3f;
    public float maxOffset = 10f;
    private float _rotY;
    private float _rotX;
    private Vector3 _offset;
    private float scrool = 0;
    void Start()
    {
        _rotY = transform.eulerAngles.y;//camera's absolute rotation y
        _rotX = transform.eulerAngles.x;
        _offset = target.position - transform.position;// position offset from camera point to target
        
    }

    void Update()
    {
        GetInput();
        ScrollDistance();
        //float horInput = Input.GetAxis("Horizontal");
        float horInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");
        _rotY += horInput * rotSpeed;

        _rotX -= verticalInput;

        Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);// rotate _rotX, _rotY degree relatively
        transform.position =
            target.position - (rotation * _offset);// rotation is a matrix and it apply to vector _offset 
        transform.LookAt(target);//auto focus on target
        
    }

    private void GetInput()
    {
        scrool = Input.GetAxis("Mouse ScrollWheel");
    }

    private void ScrollDistance()
    {
        _offset += _offset * scrool * rotSpeed;
        if (Vector3.Magnitude(_offset) < minOffset)
            _offset = Vector3.Normalize(_offset) * minOffset;
        else if (Vector3.Magnitude(_offset) > maxOffset)
            _offset = Vector3.Normalize(_offset) * maxOffset;

    }
}
