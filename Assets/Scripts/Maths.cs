using UnityEngine;

public static class Maths
{
    public static float Map (float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        float fromRange = fromMax - fromMin;
        float toRange = toMax - toMin;
        float rate = toRange / fromRange;
        return from * rate;
    }

    public static Vector3 Map(Vector3 from, float fromMin, float fromMax, float toMin, float toMax)
    {
        float x = Map(from.x, fromMin, fromMax, toMin, toMax);
        float y = Map(from.y, fromMin, fromMax, toMin, toMax);
        float z = Map(from.z, fromMin, fromMax, toMin, toMax);

        return new Vector3(x, y, z);
    }
}
