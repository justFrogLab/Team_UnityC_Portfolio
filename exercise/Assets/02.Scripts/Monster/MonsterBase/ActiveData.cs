using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveData : MonoBehaviour
{
    public bool active = false;//풀매니저 반복동작 방지용 bool변수
    public bool Detectactive = true;
    public bool ActiveEndPosition = false;
    public bool Traceactive = false;
    public bool Patrolactive = true;
    public bool Collisionactive = false;
    public bool Attackactive = false;
    public bool is70pat = false;
    public bool is50pat = false;
    public bool is30pat = false;
}
