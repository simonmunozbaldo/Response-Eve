using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorUtils
{
    public static float GetProjectionModule(Vector2 mainVector,Vector2 projectionVector)
    {
        return Vector2.Dot(mainVector, projectionVector) / mainVector.magnitude;
    }

    public static float ZAngleOfVectorInWorldSpace(Vector3 v)
    {
        float angle = v.y == 0 ? 90 : Mathf.Atan2(Mathf.Abs(v.x), Mathf.Abs(v.y)) * Mathf.Rad2Deg;
        
       
        //Primer cuadrante
        if(v.x >= 0 && v.y >= 0)
        {  
            return 360 - angle;
        }

        if (v.x >= 0 && v.y < 0)
        {
            return 180 + angle;
        }

        if (v.x < 0 && v.y < 0)
        {
            return 180 - angle;
        }

        return angle;
    }
}
