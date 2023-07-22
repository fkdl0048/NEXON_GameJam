using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position, Rotation, Scale 코드 간소화,
/// 접근 및 호출을 편하게 하기 위해 만든 스크립트.
/// </summary>
[System.Serializable]
public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class Util
{
    // 회전량이 0인 상태 (0,0,0,1)
    public static Quaternion QI => Quaternion.identity;

    public static Vector3 MousePos
    {
        get
        {
            Vector3 result = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, - Camera.main.transform.position.z));
            //result.z = 0f;
            return result;
        }
    }
}

