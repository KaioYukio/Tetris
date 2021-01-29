using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : MonoBehaviour
{
    public bool isMobile;

    public float swipeDirection;
    public float beginTouchPosX;
    public float endTouchPosX;
    public float timePressing;
    public float timeToBoostSpeed;
    public float timeToRotate;
    public bool isPressing;
    private string tetroName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPressing)
        {
            timePressing += Time.deltaTime;

            if (timePressing >= timeToBoostSpeed)
            {
                GameManager.instance.BoostSpeed();
            }
        }

    }

    public void BeginTouch()
    {
        isPressing = true;
        tetroName = GameManager.instance.activeTetro.name;
        beginTouchPosX = 0;
        endTouchPosX = 0;
        if (isMobile)
        {
            beginTouchPosX = Input.GetTouch(0).position.x;
        }
        else
        {
            beginTouchPosX = Input.mousePosition.x;
        }
    }

    public void EndTouch()
    {

        isPressing = false;
        timePressing = 0;

        if (isMobile)
        {
            endTouchPosX = Input.GetTouch(0).position.x;
        }
        else
        {
            endTouchPosX = Input.mousePosition.x;
        }


        if ((timePressing < timeToBoostSpeed && !GameManager.instance.isBoosting) && (SwipeDirection() < 15 && SwipeDirection() > -15) && tetroName == GameManager.instance.activeTetro.name)
        {
            GameManager.instance.RotateEvent();
        }
        GameManager.instance.StopBoostSpeed();

        if (SwipeDirection() > 15 || SwipeDirection() < -15)
        {
            GameManager.instance.MoveEvent(SwipeDirection());
        }

    }

    public float SwipeDirection()
    {
        return endTouchPosX - beginTouchPosX;
    }
}
