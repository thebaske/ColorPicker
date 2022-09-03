using Sirenix.OdinInspector;

using UnityEngine;

[System.Serializable]
public class ValuesCurve
{
    [SerializeField][ShowIf("showVisualization")] private AnimationCurve curve;

    [ShowIf("showVisualization")]public float[] numberOfPoints;
    [SerializeField] private float startValue;
    public float base_Value;
    public float exponential;
    public float exponentIncrease;
    
    [SerializeField][ShowIf("showVisualization")] XPData[] XPDatas;

    [SerializeField] private bool showVisualization;
    
    
    [Button("ExecuteFunction")]
    void ExecuteCalculation()
    {
        if (numberOfPoints.Length <= 0)
        {
            numberOfPoints = new float[10];
        }
        XPDatas = new XPData[numberOfPoints.Length];
        curve.keys = new Keyframe [numberOfPoints.Length];
        
        for (int i = 0; i < numberOfPoints.Length; i++)
        {
            float exponentAdder = i * exponentIncrease;
            numberOfPoints[i] = startValue + (i * Mathf.Pow(base_Value + exponentAdder, exponential));
            if (i > 0)
            {
                XPDatas[i] = new XPData(numberOfPoints[i], numberOfPoints[i] - numberOfPoints[i - 1]);
            }
            else
            {
                XPDatas[i] = new XPData(numberOfPoints[i], numberOfPoints[i]);
            }
        }
        float highest = numberOfPoints[numberOfPoints.Length - 1];
        for (int i = 0; i < numberOfPoints.Length; i++)
        {
            curve.AddKey(Utility.RemapValues(0, numberOfPoints.Length, 0f, 1f, i),
                Utility.RemapValues(0f, highest, 0f, 1f, numberOfPoints[i]));
        }
    }
    [Button("Check level")]
    public float GetValueForLevel(int level)
    {
        float exponentAdder = level * exponentIncrease;
        float result = startValue +( level * Mathf.Pow(base_Value + exponentAdder, exponential));
        Utility.DebugText($"XP for Level{level} = {result}");
        return result;
    }

    public int GetValueForLevle(int level)
    {
        int exponentAdder = (int)(level * exponentIncrease);
        int result = (int)(startValue + (level * Mathf.Pow(base_Value + exponentAdder, exponential)));
        Utility.DebugText($"XP for Level{level} = {result}");
        return result;
    }
    [System.Serializable]
    private class XPData
    {
        public float xpLevel;
        public float xpDifference;

        public XPData(float level, float difference)
        {
            xpLevel = level;
            xpDifference = difference;
        }
    }
}
