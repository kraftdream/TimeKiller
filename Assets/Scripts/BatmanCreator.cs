﻿using System.Collections;
using UnityEngine;

public class BatmanCreator : MonoBehaviour
{

    [SerializeField]
    private GameObject batman;

    public GameObject Batman
    {
        get { return batman; }
        set { batman = value; }
    }

    private Camera cam;
    private float _maxScreenWidth;
    private float _maxScreenHeight;

    private GameObject leftBatman;
    private GameObject rightBatman;

    void Start()
    {
        GetScreenSize();
        CreateBatmans();
        PlaceLeftBatmanToPosition();
    }

    void Update()
    {
        CreateBatmans();
        DestroyBatmans();
    }

    void GetScreenSize()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        Vector3 targetWidth = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        _maxScreenWidth = targetWidth.x;
        _maxScreenHeight = targetWidth.y;
    }

    void CreateBatmans()
    {
        float waitTime = Random.Range(0.5f, 2.0f);

        if (leftBatman == null)
        {
            leftBatman = Instantiate(batman) as GameObject;
            PlaceLeftBatmanToPosition();
        }

        if (rightBatman == null)
        {
            rightBatman = Instantiate(batman) as GameObject;
            rightBatman.GetComponent<MainMenuBatmanScript>().Flip();
            PlaceRightBatmanToPosition();
        }
    }

    void PlaceLeftBatmanToPosition()
    {
        leftBatman.transform.position = new Vector3(-_maxScreenWidth, _maxScreenHeight - 1);
    }

    void PlaceRightBatmanToPosition()
    {
        rightBatman.transform.position = new Vector3(_maxScreenWidth, _maxScreenHeight - 2);
    }

    void DestroyBatmans()
    {
        if (leftBatman.transform.position.x > _maxScreenWidth)
        {
            Destroy(leftBatman);
        }

        if (rightBatman.transform.position.x < -_maxScreenWidth)
        {
            Destroy(rightBatman);
        }
    }
}
