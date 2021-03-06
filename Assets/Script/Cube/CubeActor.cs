﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 执行方块对象的行为
 * 1. 检查删除
 * 2. 播放动画
 */
public class CubeActor : MonoBehaviour
{
    private bool isDestorying = false;
    

    public void DelayToFall(float time, Cube cube)
    {
        StartCoroutine(FallTimer(time, cube));
    }

    //重构
    private IEnumerator FallTimer(float time, Cube temp)
    {
        yield return new WaitForSeconds(time);
        temp.FallDown();
    }

    private IEnumerator DestoryFallingCube()
    {
        while (transform.position.y > CubeConstant.vanishingYPlane)
        {
            yield return new WaitForSeconds(CubeConstant.checkInterval);
        }
        Destroy(gameObject);
    }

    public void CheckToDestory()
    {
        if(!isDestorying)
            StartCoroutine(DestoryFallingCube());
    }
    public void DestoryCube()
    {
        Destroy(gameObject);
    }
}
