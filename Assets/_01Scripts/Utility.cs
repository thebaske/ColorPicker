using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

/// <summary>
/// Various utility functions. 
/// </summary>
public static class Utility
{
    private static Camera _camera;
    
        public static Camera Camera
        {
            get
            {
                if (_camera == null) _camera = Camera.main;
                return _camera;
            }
        }
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }

        return array;
    }
    public static T[] GetRandomizedArrayUnlimitedSize<T>(T[] array, int collectionSize, int seed)
    {
        System.Random prng = new System.Random(seed);
        int lastIndex = 1110;
        int[] collectionIndexes = new int[collectionSize];
        
        for (int i = 1; i < collectionSize; i++)
        {
            for (int z = 0; z < 1; z++)
            {
                int index = prng.Next(0, array.Length);
                if (index == lastIndex)
                {
                    z = -1;
                }
                else
                {
                    collectionIndexes[i] = index;
                    lastIndex = index;
                    z = 2;
                }
            }
        }

        T[] resultingArray = new T[collectionIndexes.Length];
        for (int i = 0; i < collectionIndexes.Length; i++)
        {
            resultingArray[i] = array[collectionIndexes[i]];
        }

        return resultingArray;
    }
    public static bool GetProbability(float percent)
    {
        int randomRange = Random.Range(0, 101);
        if (randomRange > RemapValues(0f, 1f, 0, 100, percent))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static T[] Randomize<T>(this T[] array, int seed)
    {
        array = ShuffleArray<T>(array, seed);
        return array;
    }
    public static T RandomButNotThis<T>(this T[] array, int ExcludeMe)
    {
        if (array.Length > 1)
        {
            T zeroElement = array[0];
            T excluded = array[ExcludeMe];
            array[0] = excluded;
            array[ExcludeMe] = zeroElement;
            return array[Random.Range(1, array.Length)];
        }
        else
        {
            return array[0];
        }
    }
    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _result;
    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
        _result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _result);
        return _result.Count > 0;
    }

    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    public static Vector3 GetWorldPositionFromScreen(Vector2 screenPosition, float distance)
    {
        Vector3 resultingVector = Camera.ScreenToWorldPoint(screenPosition).TakeZ(new Vector3(0f, 0f, distance));
        return resultingVector;
    }

    public struct HitInfo
    {
        public RaycastHit hit;
    }

    public static void GetWorldHitPointWithFilter(Vector2 screenPosition, out HitInfo hitInfo)
    {
        hitInfo = new HitInfo();
        Ray camRay = Camera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(camRay.origin, camRay.direction, out RaycastHit hit, 100f))
        {
            hitInfo.hit = hit;
        }        
    }
    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }
    public static void DeleteChildrenEditor(this Transform t)
    {
        foreach (Transform child in t) Object.DestroyImmediate(child.gameObject);
    }
    public static Vector3[] BoundsGridGeneratorXZPlane(GameObject bounder, float spacing, float padding)
    {   
        Bounds bnds = Utility.GetMaxBoundsOmniKnowing(bounder);
        Vector3 localXPosLeftFront = bnds.center + new Vector3(-bnds.extents.x, 0f, bnds.extents.z);
        Vector3 localxPosRightFront = bnds.center + new Vector3(bnds.extents.x, 0f, bnds.extents.z);
        Vector3 localXPosLeftBack = bnds.center + new Vector3(-bnds.extents.x, 0f, -bnds.extents.z);
        Vector3 localxPosRightBack = bnds.center + new Vector3(bnds.extents.x, 0f, -bnds.extents.z);
        int extentsx = (int)(bnds.extents.x * 2f);
        int extentsz = (int)(bnds.extents.z * 2f);
        
        int gridSizeX = (int)(extentsx / spacing);
        int gridSizeZ = (int) (extentsz / spacing);
        Vector3[] localGrid = new Vector3[gridSizeX * gridSizeZ];
        int gridCounter = 0;
        
        for (int i = 0; i < gridSizeZ; i++)
        {
            for (int j = 0; j < gridSizeX; j++)
            {
                float x = Vector3.Lerp(localXPosLeftFront, localxPosRightFront, Utility.RemapValues(0, gridSizeX - 1, 0f + padding, 1 - padding, j)).x;
                float y = bounder.transform.localPosition.y;
                float z = Vector3.Lerp(localXPosLeftFront, localXPosLeftBack, Utility.RemapValues(0, gridSizeZ - 1, 0f + padding, 1 - padding, i)).z;
                localGrid[gridCounter] = new Vector3(x, y, z);
                gridCounter++;
                
            }
        }
        return localGrid;
    }
    
    //ExampleResult of using this method. Input 123456789123 (123 Billion) - Result: 123.45B

    //We make an enum that contains our suffixes.
    //These can be changed or more suffixes can be added.
    public enum suffixes
    {
        p, // p is a placeholder if the value is under 1 thousand
        K, // Thousand
        M, // Million
        B, // Billion
        T, // Trillion
        Q, //Quadrillion
    }

    //Formats numbers in Millions, Billions, etc.
    public static string NumberFormat(long money)
    {
        int decimals = 2; //How many decimals to round to
        string r = money.ToString(); //Get a default return value

        foreach (suffixes suffix in Enum.GetValues(typeof(suffixes))) //For each value in the suffixes enum
        {
            var currentVal = 1 * Math.Pow(10, (int)suffix * 3); //Assign the amount of digits to the base 10
            var suff = Enum.GetName(typeof(suffixes), (int)suffix); //Get the suffix value
            if ((int)suffix == 0) //If the suffix is the p placeholder
                suff = String.Empty; //set it to an empty string

            if (money >= currentVal)
                r = Math.Round((money / currentVal), decimals, MidpointRounding.ToEven).ToString() + suff; //Set the return value to a rounded value with suffix
            else
                return r; //If the value wont go anymore then return
        }
        return r; // Default Return
    }
    public static int GetCurrentLoopIndex<T>(T[] array, int cycles)
        {
            int counter = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (i % cycles ==0)
                {
                    counter++;
                    if (counter > array.Length - 1)
                    {
                        counter = 0;
                    }
                }
            }
            return counter;
        }
    public static int GetRandomIndex<T>(List<T> list)
    {
        int index = UnityEngine.Random.Range(0, list.Count);

        return index;
    }
    public static float LerpCustom(float a, float b, float t)
    {
        return (1.0f - t) * a + b * t;
    }
    public static float InverseLerpCUstom(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }
    /// <summary>
    /// Takes two input parameters(iMin, iMax) and remaps those values to 
    /// oMin and oMax by v.
    /// if v == iMin output = oMin
    /// if v == iMax output = oMax
    /// </summary>    
    public static float RemapValues(float iMin, float iMax, float oMin, float oMax, float v)
    {
        float t = InverseLerpCUstom(iMin, iMax, v);
        return LerpCustom(oMin, oMax, t);
    }
    public static void DebugText(string message, string color)
    {
        if (Application.isEditor)
            Debug.Log("<color=" + color + ">" + " " + Application.productName + "_" + message + "</color>");
    }
    public static Vector3 CalculateElementPosition(int numberOfelements, int currentElement, float elementsDistance, float elementsSize)
    {
        float screenWidth = numberOfelements * (elementsDistance + elementsSize);
        float endXPointLeft = screenWidth /2f;
        Vector3 result = Vector3.zero;
        float x = Utility.RemapValues(0, numberOfelements, -endXPointLeft, endXPointLeft, currentElement);
        result = new Vector3(x, 0, 0);
        return result;
    }
    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
    public static Vector3 QuadraticBezierCurve(float t, Vector3 start, Vector3 height, Vector3 finish)
    {
        // B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2

        float oneMinusT = 1 - t;
        float oneMinusTSqrd = oneMinusT * oneMinusT;
        float tiSqrd = t * t;
        Vector3 result = oneMinusTSqrd * start + 2 * oneMinusT * t * height + tiSqrd * finish;
        return result;
    }
    public static void DebugText(string message)
    {
        if (Application.isEditor)
            Debug.Log($"<color=" + "red" + ">" + "#" + "</color>" + "<color=" + "white" + ">" + " " + Application.productName + "_" + message + "</color>");
    }
    public static void DebugText(string message, object sender = null)
    {
        if (Application.isEditor)
        {
            if (sender != null)
            {
                Debug.Log("<color=" + "red" + ">" + "#" + "</color>" + "<color=" + "white" + ">" + " " + Application.productName + "_" + message + "  sender: " + sender + "</color>");
            }
            else
            {
                Debug.Log("<color=" + "red" + ">" + "#" + "</color>" + "<color=" + "white" + ">" + " " + Application.productName + "_" + message + "</color>");
            } 
        }
            
    }
    public static void DebugText(string message, string color, object sender = null)
    {
        if (Application.isEditor)
        {
            if (sender != null)
            {
                Debug.Log("<color=" + color + ">" + "#" + "</color>" + "<color=" + color + ">" + " " + Application.productName + "_" + message + "  sender: " + sender + "</color>");
            }
            else
            {
                Debug.Log("<color=" + color + ">" + "#" + "</color>" + "<color=" + color + ">" + " " + Application.productName + "_" + message + "</color>");
            }
        }
            
    }
    public static bool RandomPositionOnNavmesh(Vector3 sourcePosition, out Vector3 hitPos)
    {
        Vector2 randomPoint = Random.insideUnitCircle;
        Vector3 samplePoint = sourcePosition + new Vector3(randomPoint.x, 0, randomPoint.y) * Random.Range(0, 120f);
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(samplePoint, out navHit, 50f, NavMesh.AllAreas))
        {
            hitPos = navHit.position;
            return true;
        }
        else
        {
            hitPos = sourcePosition;
            return false;
        }

    }
    #region Rotate towards using torque

    // private void RotateShooter()
    // {
    //     Vector3 directionToPlayer = GameControl.thePlayer.transform.position - roboBodyRigid.transform.position;
    //     //Quaternion newRotation = Quaternion.LookRotation(directionToPlayer * Time.deltaTime);
    //     float angleDifference = Vector3.Angle(roboBodyRigid.transform.forward, directionToPlayer);
    //     angleDifferenceInfo = angleDifference;
    //     Vector3 cross = Vector3.Cross(roboBodyRigid.transform.forward, directionToPlayer);
    //     roboBodyRigid.AddTorque(cross * angleDifference * torqueForce, ForceMode.Force);
    // }
    // private void RotateHips()
    // {
    //     Vector3 directionToPlayer = GameControl.thePlayer.transform.position - roboBodyHips.transform.position;
    //     //Quaternion newRotation = Quaternion.LookRotation(directionToPlayer * Time.deltaTime);
    //     float angleDifference = Vector3.Angle(roboBodyHips.transform.forward, directionToPlayer);
    //     Vector3 cross = Vector3.Cross(roboBodyHips.transform.forward, directionToPlayer);
    //     roboBodyHips.AddTorque(cross * angleDifference * (torqueForce * 2), ForceMode.Force);
    // }

    #endregion
    public static float PerlinNoiseRandomizator(float time)
    {
        return Mathf.PerlinNoise(time + 0.2f, time);
    }
    public static Vector3 GetTerrainPosition(Vector3 pos, LayerMask layer)
    {
        Debug.DrawRay(new Vector3(pos.x, 50f, pos.z), Vector3.down, Color.blue, 20f);
        if (Physics.Raycast(new Vector3(pos.x, 50f, pos.z), Vector3.down, out RaycastHit hitinfo, 2000f, layer))
        {
            return hitinfo.point;
        }
        return pos;
    }
    public static Quaternion RotateMyAxisWithThisDirection(Vector3 directionTOMatch, Vector3 axisToMatchWithDirection, Transform rotatedObject)
    {

        #region OldRotationExamples
        // Vector3 directionToHitpoint = rotateTowardsMe.transform.position - gameObject.transform.position;
        // float angleDifference = Vector3.Angle(gameObject.transform.forward, directionToHitpoint);
        // Vector3 cross = Vector3.Cross(gameObject.transform.forward, directionToHitpoint);
        // Quaternion tmp = Quaternion.FromToRotation(gameObject.transform.forward, cross * angleDifference);
        // Quaternion tmp = Quaternion.FromToRotation(gameObject.transform.forward, rotateTowardsMe.transform.position);

        // gameObject.transform.rotation = Quaternion.Euler(tmp.x, rotateTowardsMe.transform.rotation.y, 0f);
        //Which axis to match the direction towards game object
        //Quaternion fromToROtation = Quaternion.FromToRotation(fromRotationAxis, rotateTowardsMe.transform.position - gameObject.transform.position);  
        #endregion


        //direction from object
        Debug.DrawRay(rotatedObject.position, directionTOMatch * 15f, Color.white);
        Debug.DrawRay(rotatedObject.position, (rotatedObject.position + axisToMatchWithDirection) * 15f, Color.blue);

        Vector3 crossDirection = Vector3.Cross(directionTOMatch, axisToMatchWithDirection);

        Debug.DrawRay(rotatedObject.position, crossDirection * 15f, Color.black);

        Quaternion fromTOCrossRotation = Quaternion.FromToRotation(axisToMatchWithDirection, crossDirection);

        return fromTOCrossRotation;

        // gameObject.transform.rotation = fromToROtation;   



        // Vector3 crossDirection = Vector3.Cross(fromRotationAxis, crossAxisMain);
        // gameObject.transform.rotation = Quaternion.FromToRotation(axisFromTOMatchCross, crossDirection);
    }
    public static void MatchAxisTODirection(Transform rotor, Vector3 myAxis, Vector3 toThisdirection)
    {
        Vector3 toCrosswith = Vector3.zero;
        if (myAxis.x == 1)
        {
            //choose up
            toCrosswith = rotor.up;
        }
        if (myAxis.y == 1)
        {
            toCrosswith = rotor.right;
        }
        if (myAxis.z == 1)
        {
            toCrosswith = rotor.up;
        }
        if (myAxis.x == -1)
        {
            toCrosswith = -rotor.up;
        }
        if (myAxis.y == -1)
        {
            toCrosswith = -rotor.right;
        }
        if (myAxis.z == -1)
        {
            toCrosswith = -rotor.up;
        }
        Vector3 cross1 = Vector3.Cross(toThisdirection, toCrosswith);
        //Vector3 cross2 = Vector3.Cross(cross1, toCrosswith);
        //rotor.up = Vector3.Lerp(rotor.up, -Vector3.Cross(cross1, toCrosswith), Time.deltaTime * 5f);
        rotor.up = -Vector3.Cross(cross1, toCrosswith);
    }

    public static void DirectROtation(Transform rotatedObject, Vector3 rotateTowardsMe, Vector3 crossDirectionWithme, Vector3 axisToMatchTheCross)
    {
        Vector3 rotatedTowardsMeCorrected = new Vector3(rotateTowardsMe.x, rotateTowardsMe.y, rotateTowardsMe.z);
        rotateTowardsMe = rotatedTowardsMeCorrected;
        //Debug.DrawRay(rotatedObject.position, (rotateTowardsMe - rotatedObject.position).FlattenedXY() * 15f, Color.white);
        //Debug.DrawRay(rotatedObject.position, (rotatedObject.position + crossDirectionWithme).FlattenedXY() * 15f, Color.red);


        Vector3 crossDirection = Vector3.Cross((rotateTowardsMe - rotatedObject.position), rotatedObject.forward);

        //Debug.DrawRay(rotatedObject.position, crossDirection * 15f, Color.black);

        Quaternion fromTOCrossRotation = Quaternion.FromToRotation(axisToMatchTheCross, -Vector3.Cross(crossDirection, crossDirectionWithme));

        rotatedObject.rotation = fromTOCrossRotation;
    }
    public static int[] GetRandomChunksFOrLoop(int totalNumber, int chunksNumber)
    {
        int equalRes = totalNumber / chunksNumber;
        int[] resultingArray = new int[chunksNumber];
        int current = 0;
        int next = equalRes;
        int chunkCOunter = 0;
        //Setting equals
        for (int i = 0; i < totalNumber; i++)
        {
            if (i == next)
            {
                resultingArray[chunkCOunter] = i;
                next += equalRes;
                chunkCOunter++;
                if (chunkCOunter > resultingArray.Length - 1)
                {
                    chunkCOunter = resultingArray.Length - 1;
                }
                if (next + equalRes > totalNumber)
                {
                    next = totalNumber;
                }
            }
        }
        int hasJackpot = 0;
        next = resultingArray[1];
        current = 0;
        //Randomizing chunks
        for (int i = 0; i < resultingArray.Length - 1; i++)
        {
            if (Random.Range(0, 100) < 50)
            {
                //Reduce next element down
                if (i + 1 < resultingArray.Length - 1)
                {
                    resultingArray[i + 1] = (resultingArray[i + 1] - Random.Range(1, (resultingArray[i + 1] - resultingArray[i]) - 1));
                }
            }
        }

        return resultingArray;
    }
    public static int[] GetEqualChunks(int totalNumber, int chunksNumber)
    {
        int equalRes = totalNumber / chunksNumber;
        int[] resultingArray = new int[chunksNumber];
        int current = 0;
        int next = equalRes;
        int chunkCOunter = 0;
        //Setting equals
        for (int i = 0; i <= totalNumber; i++)
        {
            if (i == next)
            {
                resultingArray[chunkCOunter] = i;
                next += equalRes;
                chunkCOunter++;
                if (chunkCOunter > resultingArray.Length - 1)
                {
                    chunkCOunter = resultingArray.Length - 1;
                }
                if (next >= totalNumber)
                {
                    next = totalNumber;
                }
            }
        }
        return resultingArray;
    }
    public static Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        var vVector1 = vPoint - vA;
        var vVector2 = (vB - vA).normalized;

        var d = Vector3.Distance(vA, vB);
        var t = Vector3.Dot(vVector2, vVector1);

        if (t <= 0)
            return vA;

        if (t >= d)
            return vB;

        var vVector3 = vVector2 * t;

        var vClosestPoint = vA + vVector3;

        return vClosestPoint;
    }
    public static Bounds GetMaxBoundsOmniKnowing(GameObject g)
    {
        var renderers = g.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(g.transform.position, Vector3.zero);
        var b = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }
public static int GetClosestByHalvingStraightLine(Vector3[] v, Transform position, float distanceCriteria)
    {
        #region Getting closest Point by halving distance

        
        Vector3 closestPoint = position.position;
        
        float closest = 10000f;
        bool closestFound = false;
        int minItterator = 0;
        int maxItterator = v.Length - 1;
        int halfWayThrough = ((int)(maxItterator - (maxItterator - minItterator)/2));
        while (!closestFound)
        {
            Vector3 pointHalf = v[halfWayThrough];
            Vector3 pointMax = v[maxItterator];
            Vector3 pointMin = v[minItterator];
            float distanceHalf = Vector3.Distance(position.position, pointHalf);
            float distanceMax = Vector3.Distance(position.position, pointMax);
            float distanceMin = Vector3.Distance(position.position, pointMin);
            
            if (maxItterator - minItterator < 3)
            {
                closestFound = true;
                return halfWayThrough;
            }
            if (distanceHalf <= distanceMax)
            {
                minItterator = halfWayThrough;
                halfWayThrough = ((int)(maxItterator - (maxItterator - minItterator)/2));
            }
            else
            {
                if (distanceHalf < distanceCriteria)
                {
                    closestFound = true;
                    closestPoint = pointHalf;
                    return halfWayThrough;
                }
                if (distanceHalf < closest)
                {
                    closest = distanceHalf;
                    maxItterator = halfWayThrough;
                    halfWayThrough = ((int)(maxItterator - (maxItterator - minItterator)/2));
                }
                else
                {
                    closestFound = true;
                    closestPoint = pointHalf;
                    return halfWayThrough;
                }
            }
        }
        
        return halfWayThrough;

        #endregion
    }
}

public static class ExtensionMethods
{

    //public static Vector3 NavMeshRandomPosition(this NavMesh mesh)
    //{
    //    Vector2 randomPoint = Random.insideUnitCircle;
    //    Vector3 samplePoint = new Vector3(randomPoint.x, 0, randomPoint.y) * Random.Range(0, 20f);
    //    NavMeshHit navHit;
    //    NavMesh.SamplePosition(samplePoint, out navHit, 20f, -1);
    //    return navHit.position;
    //}
    //public static GameObject OnDisableCallback(this GameObject go, UnityAction callback)
    //{
    //    go.OnDestroy += ;
    //}
    //public static bool CompareMYTaggable(this GameObject go, string comparer)
    //{
    //    Taggable tgb = go.GetComponent<Taggable>();
    //    if (tgb != null && tgb.CompareMyTag(comparer))
    //    {
    //        return true;
    //    }
    //    else { return false; }
    //}
    
    public static bool IsInMask(this int layer, LayerMask mask)
    {
        return ((1 << layer) & mask.value) != 0;
    }
    public static int GetSelectedLayer(this LayerMask layer)
    {
        int val = (int)layer.value;
        int res = 0;
        if (val == 0)
            res = -1;
        for (int i = 0; i < 32; i++)
        {
            if ((val & (1 << i)) != 0)
                res = i;
        }
        return res;
    }

    public static IEnumerator OnComplete(this MonoBehaviour i, UnityAction OnComplete)
    {
        OnComplete?.Invoke();
        return null;
    }
    public static IEnumerator OnCompleteDelay(this MonoBehaviour i, UnityAction OnComplete, float delay)
    {
        yield return new WaitForSeconds(delay);
        OnComplete?.Invoke();

    }
    public static Vector3 RandomDirection(this Vector3 v, Vector3 startingPoint)
    {
        Vector2 random = Random.insideUnitCircle;
        Vector3 result = startingPoint + new Vector3(random.x, startingPoint.y, random.y);
        return result;
    }
    public static Vector3 TakeY(this Vector3 v, Vector3 mergeFrom)
    {
        return new Vector3(v.x, mergeFrom.y, v.z);
    }
    public static Vector3 TakeZ(this Vector3 v, Vector3 mergeFrom)
    {
        return new Vector3(v.x, v.y, mergeFrom.z);
    }
    public static Vector3 TakeX(this Vector3 v, Vector3 mergeFrom)
    {
        return new Vector3(mergeFrom.x, v.y, v.z);
    }
    public static Vector2 xy(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }
    public static Vector3 LerpMe(this Vector3 v, Vector3 final, float time)
    {
        v = Vector3.Lerp(v, final, time);
        return v;
    }

    public static Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector2 WithX(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }

    public static Vector2 WithY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    public static Vector3 WithZ(this Vector2 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }
    public static Vector3 RandomizeX(this Vector3 v, float lateralMovement)
    {
        return new Vector3(Random.Range(-lateralMovement, lateralMovement), v.y, v.z);
    }
    public static Vector3 ONE(this Vector3 v)
    {
        return new Vector3(1, 1, 1);
    }
    // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
    // point - the point to find nearest on line for
    public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
    {
        if (!isNormalized) axisDirection.Normalize();
        var d = Vector3.Dot(point, axisDirection);
        return axisDirection * d;
    }

    // lineDirection - unit vector in direction of line
    // pointOnLine - a point on the line (allowing us to define an actual line in space)
    // point - the point to find nearest on line for
    public static Vector3 NearestPointOnLine(
        this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
    {
        if (!isNormalized) lineDirection.Normalize();
        var d = Vector3.Dot(point - pointOnLine, lineDirection);
        return pointOnLine + (lineDirection * d);
    }
    public static Vector3 WithAddX(this Vector3 v, float x)
    {
        return new Vector3(v.x + x, v.y, v.z);
    }

    public static Vector3 WithAddY(this Vector3 v, float y)
    {
        return new Vector3(v.x, v.y + y, v.z);
    }

    public static Vector3 WithAddZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, v.z + z);
    }
    public static Vector2 WithAddX(this Vector2 v, float x)
    {
        return new Vector2(v.x + x, v.y);
    }
    public static Vector2 WithAddY(this Vector2 v, float y)
    {
        return new Vector2(v.x, v.y + y);
    }
    public static Vector3 Flattened(this Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }
    public static Vector3 FlattenedXZ(this Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }
    public static Vector3 FlattenedXY(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, 0f);
    }
    public static Vector3 FlattenedZY(this Vector3 vector)
    {
        return new Vector3(0f, vector.y, vector.z);
    }

    public static float DistanceFlat(this Vector3 origin, Vector3 destination)
    {
        return Vector3.Distance(origin.Flattened(), destination.Flattened());
    }
}