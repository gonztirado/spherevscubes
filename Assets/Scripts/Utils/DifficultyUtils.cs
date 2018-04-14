using UnityEngine;

public class DifficultyUtils
{
    public static float IncreaseExpontential(float value, float initValue, float difficulty)
    {
        if (value < 1)
            return Mathf.Pow(initValue, 1 / difficulty);
        if (value > 1)
            return Mathf.Pow(initValue, difficulty);
        return value;
    }

    public static int IncreaseExpontential(int value, int initValue, float difficulty)
    {
        return (int) IncreaseExpontential((float) value, (float) initValue, difficulty);
    }
    
    public static float DecreaseExpontential(float value, float initValue, float difficulty)
    {
        if (value > 1)
            return Mathf.Pow(initValue, 1 / difficulty);
        if (value < 1)
            return Mathf.Pow(initValue, difficulty);
        return value;
    }

    public static int DecreaseExpontential(int value, int initValue, float difficulty)
    {
        return (int) DecreaseExpontential((float) value, (float) initValue, difficulty);
    }
}