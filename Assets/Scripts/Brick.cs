using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    
    public int PointValue;

    private int hp = 1;

    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new();
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", Color.green);
                break;
            case 2:
                block.SetColor("_BaseColor", Color.yellow);
                break;
            case 5:
                block.SetColor("_BaseColor", Color.blue);
                break;
            default:
                block.SetColor("_BaseColor", Color.red);
                break;
        }
        renderer.SetPropertyBlock(block);
        if (Scene_Flow.Instance.currentDifficulty != Difficulty.Easy) hp *= PointValue;

       // else if (Scene_Flow.Instance.difficulty == 1) hp *= PointValue;
    }

    private void OnCollisionEnter(Collision other)
    {
        hp--;
        if (hp <= 0)
        {
            onDestroyed.Invoke(PointValue);
            //slight delay to be sure the ball have time to bounce
            Destroy(gameObject, 0.2f);
        }
    }
}
