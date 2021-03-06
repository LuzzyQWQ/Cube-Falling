﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CubeType
{
    Air, Stone, Sand, Ice,
    //length of cube type
    Length
}

public class CubeManager
{
    private World world;
    private int worldWidth;
    private int worldHeight;
    private Cube[,,] cubes;
    private CubeData[,,] cubeDatas;
    private int cubeCount;

    public CubeManager(World world)
    {
        this.world = world;
        worldWidth = world.worldWidth;
        worldHeight = world.worldHeight;
        InitCubeManager();
    }

    public void InitCubeManager()
    {
        cubes = new Cube[worldWidth, worldHeight, worldWidth];
        cubeCount = 0;
    }

    public void LoadCubeDatas(CubeData[,,] datas)
    {
        cubeDatas = datas;
    }

    public void SetCube(Vector3 pos, CubeType type)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        if (OutOfBound(x, y, z))
        {
            return;
        }

        if (type == CubeType.Stone)
            cubes[x, y, z] = new Stone(pos, world);
        else if (type == CubeType.Sand)
            cubes[x, y, z] = new Sand(pos, world);
        else if (type == CubeType.Ice)
            cubes[x, y, z] = new Ice(pos, world);
    }

    public Cube[,,] GetCubes()
    {
        return cubes;
    }

    public void GenerateCubes()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldHeight; y++)
            {
                for (int z = 0; z < worldWidth; z++)
                {
                    if (cubeDatas[x, y, z] != null)
                    {
                        if (cubeDatas[x, y, z].cubeType == CubeType.Stone)
                            cubes[x, y, z] = new Stone(new Vector3(x, y, z), world);
                        else if (cubeDatas[x, y, z].cubeType == CubeType.Sand)
                            cubes[x, y, z] = new Sand(new Vector3(x, y, z), world);
                        else if (cubeDatas[x, y, z].cubeType == CubeType.Ice)
                            cubes[x, y, z] = new Ice(new Vector3(x, y, z), world);

                        cubeCount++;
                    }
                }
            }
        }
    }

    public void BreakBlock(Vector3 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        if (OutOfBound(x, y, z))
            return;
        if (cubes[x, y, z] != null)
        {
            cubes[x, y, z].Disappear();
            cubes[x, y, z] = null;
        }
    }

    /*
     * Finish
     * 使世界中某个位置产生一个坠落影响，作用于上下左右的方块
     * isDie = true 是怪物死亡产生的坠落效果
     * isDie = false 是方块连锁产生的效果
     * time 坠落的延迟，如果是怪物死亡，默认为0
     */
    public void FallAround(Vector3 diePos, bool isDie, float time = 0)
    {
        Vector3Int[] offset =  { new Vector3Int( 0, 1, 0 ), new Vector3Int( 0, -1, 0 ),
                                 new Vector3Int( -1, 0, 0 ), new Vector3Int( 1, 0, 0 ),
                                 new Vector3Int( 0, 0, 1 ), new Vector3Int( 0, 0, -1 )};

        int x = Mathf.FloorToInt(diePos.x);
        int y = Mathf.FloorToInt(diePos.y);
        int z = Mathf.FloorToInt(diePos.z);

        for (int i = 0; i < offset.Length; i++)
        {
            Vector3Int pos = new Vector3Int(x, y, z);
            pos += offset[i];
            if (!OutOfBound(pos.x, pos.y, pos.z))
            {
                Cube temp = cubes[pos.x, pos.y, pos.z];
                if (temp != null)
                {
                    if (isDie)
                    {
                        temp.FallDown();
                    }
                    else
                    {
                        if (temp.GetChainable())
                        {
                            if (time != 0)
                                temp.DelayToFall();
                        }
                    }
                }

            }
        }
    }

    private bool OutOfBound(int x, int y, int z)
    {
        return x < 0 || x >= world.worldWidth || y < 0 || y >= world.worldHeight || z < 0 || z >= world.worldWidth;
    }

    private void CreateCubeByType(CubeType type)
    {

    }

    private void DestroyCubes()
    {
        if (cubes == null)
            return;
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldHeight; y++)
            {
                for (int z = 0; z < worldWidth; z++)
                {
                    if (cubes[x, y, z] != null)
                        cubes[x, y, z].Disappear();
                }
            }
        }
    }

    public void DecreaseCubeCount()
    {
        cubeCount--;
    }

    public void CheckWin()
    {
        Debug.Log(cubeCount);
        if (cubeCount == 0)
            GameManager.GetInstance().Win();
    }

    public void ResetCubes()
    {
        cubeCount = 0;
        DestroyCubes();
    }
}
