using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static bool IsWithin(float x, float min, float max)
    {
        return (x - min) * (max - x) >= 0;
    }
}
