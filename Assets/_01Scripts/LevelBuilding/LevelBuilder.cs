using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataSystem;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class LevelBuilder : MonoBehaviour
{
    [SerializeField] IntValue playerLevel;
    [Header("Minimum number of unlocked Chunks")]
    [SerializeField] int minChunkUnlockIndex;
    [Header("Min number of chunks")]
    [SerializeField] int minNumberOfChunks;
    [Header("MaximumNumberOfChunks")]
    [SerializeField] int maxNumberOfChunks;
    public GameObject StartGO;
    public GameObject End;
    public List<GameObject> levelChunks;
    [Header("Spiral, Levak, Holes")]
    public List<GameObject> specials;
    int numberOfChunks;
    int numberOfSpecials;
    public IntValue levelSeed;
    int[] chunkIndexes;
    List<int> specialsSpawned;
    public List<OnboardingLevels> onboardingLevels;
    public List<GameObject> testChunks;
    public List<GameObject> Bonuschunks;
    [SerializeField] IntValue lastBonusLoaded;
    [SerializeField]bool TEST;
    [SerializeField]BoolValue bonusLoaded;
    private void Start()
    {
        bonusLoaded.MyValue = false;
#if UNITY_EDITOR
        if (TEST)
        {
            Build_TEST();
            GlobalEventHolder.geh.OnLevelSpawned.RaiseEmpty();
            return;
        }
#endif
        if (playerLevel.MyValue % 5 == 0 &&  lastBonusLoaded.MyValue != playerLevel.MyValue && playerLevel.MyValue > 2)
        {
            BonusLevel();
            bonusLoaded.MyValue = true;
            GlobalEventHolder.geh.OnGameSuccess.OnRaiseEmpty += BonusSuccessValueIncrease;
            GlobalEventHolder.geh.OnLevelSpawned.RaiseEmpty();
        }
        else
        {
            specialsSpawned = new List<int>();
            if (playerLevel.MyValue > onboardingLevels.Count - 1)
            {
                BuildLevel();
            }
            else
            {
                BuildLevelOnboarding();
            }
            GlobalEventHolder.geh.OnLevelSpawned.RaiseEmpty();
        }
    }
#if UNITY_EDITOR
    [Button("Play Test")]
    void TestBuild()
    {
        TEST = true;
        EditorApplication.EnterPlaymode();
    }
#endif
    private void OnDisable()
    {
        TEST = false;
        if (bonusLoaded)
        {
            GlobalEventHolder.geh.OnGameSuccess.OnRaiseEmpty -= BonusSuccessValueIncrease;
        }
        
    }
    void Build_TEST()
    {
        Vector3 connection = transform.position;
       
        
        GameObject start = Instantiate(StartGO, connection, Quaternion.identity);
        connection = start.GetComponent<ChunkConnection>().Connection.position;
        for (int i = 0; i < testChunks.Count; i++)
        {   
            GameObject chunk = Instantiate(testChunks[i], connection, Quaternion.identity);
            connection = chunk.GetComponent<ChunkConnection>().Connection.position;
        }
        GameObject end_tmp = Instantiate(End, connection, Quaternion.identity);
    }
    void BonusLevel()
    {
        Vector3 connection = transform.position;
      GameObject[] chunksRandomized = Utility.ShuffleArray<GameObject>(Bonuschunks.ToArray(), levelSeed.MyValue);
        GameObject start = Instantiate(StartGO, connection, Quaternion.identity);

        connection = start.GetComponent<ChunkConnection>().Connection.position;
        for (int i = 0; i < chunksRandomized.Length; i++)
        {
            GameObject chunk = Instantiate(chunksRandomized[i], connection, Quaternion.identity);
            connection = chunk.GetComponent<ChunkConnection>().Connection.position;
        }
        GameObject end_tmp = Instantiate(End, connection, Quaternion.identity);
    }
    void BonusSuccessValueIncrease()
    {
        lastBonusLoaded.MyValue = playerLevel.MyValue;
        
    }
    void BuildLevelOnboarding()
    {
        //Utility.DebugText("building onboarding level");
        Vector3 connection = transform.position;
                GameObject start = Instantiate(StartGO, connection, Quaternion.identity);
                connection = start.GetComponent<ChunkConnection>().Connection.position;
        for (int i = 0; i < onboardingLevels[playerLevel.MyValue].onboardingChunks.Length; i++)
        {
           
                GameObject chunk = Instantiate(onboardingLevels[playerLevel.MyValue].onboardingChunks[i], connection, Quaternion.identity);
                connection = chunk.GetComponent<ChunkConnection>().Connection.position;
        }
        GameObject end_tmp = Instantiate(End, connection, Quaternion.identity);
    }
    void BuildLevel()
    {
        Vector3 connection = transform.position;
        int maxUnlockedChunk = onboardingLevels.Count - 1;
        maxNumberOfChunks = (int)Utility.RemapValues(0f, 30f, minNumberOfChunks, maxNumberOfChunks, Mathf.Clamp(playerLevel.MyValue, minNumberOfChunks, maxNumberOfChunks));
        if (playerLevel.MyValue + minChunkUnlockIndex > levelChunks.Count)
        {
            maxUnlockedChunk = levelChunks.Count;
        }
        else
        {
            maxUnlockedChunk = playerLevel.MyValue + minChunkUnlockIndex;
        }
        //Utility.DebugText($"Unlocked chunk {maxUnlockedChunk}");
        RandomizeIndexes(maxUnlockedChunk, GetNumberOfChunksForSeed());
        GameObject start = Instantiate(StartGO, connection, Quaternion.identity);
        connection = start.GetComponent<ChunkConnection>().Connection.position;
        for (int i = 0; i < chunkIndexes.Length; i++)
        {
            //if (i == 0)
            //{
            //}
            //else
            //{
               
            //}
                    GameObject chunk = Instantiate(levelChunks[chunkIndexes[i]], connection, Quaternion.identity);
                    connection = chunk.GetComponent<ChunkConnection>().Connection.position; 
        }
        GameObject end_tmp = Instantiate(End, connection, Quaternion.identity);
    }
   
    private int GetNumberOfChunksForSeed()
    {
        System.Random nmbrOfChunks = new System.Random(levelSeed.MyValue);
        numberOfChunks = nmbrOfChunks.Next(minNumberOfChunks, maxNumberOfChunks);
        return numberOfChunks;
    }
    private void RandomizeIndexes(int maxChunkIndex, int numberOfChunks)
    {
        System.Random prng = new System.Random(levelSeed.MyValue);
        int lastIndex = 1110;
        chunkIndexes = new int[numberOfChunks];
        if (maxChunkIndex <= levelChunks.Count - 1)
        {

            chunkIndexes[0] = maxChunkIndex;
            //Utility.DebugText($"Unlocked chunk {maxChunkIndex} first chunk to instantiate {chunkIndexes[0]}");
            for (int i = 1; i < numberOfChunks; i++)
            {
                for (int z = 0; z < 1; z++)
                {
                    int index = prng.Next(0, maxChunkIndex);
                    if (index == lastIndex)
                    {
                        z = -1;
                    }
                    else
                    {
                        chunkIndexes[i] = index;
                        lastIndex = index;
                        z = 2;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < numberOfChunks; i++)
            {
                for (int z = 0; z < 1; z++)
                {
                    int index = prng.Next(0, maxChunkIndex);
                    if (index == lastIndex)
                    {
                        z = -1;
                    }
                    else
                    {
                        chunkIndexes[i] = index;
                        lastIndex = index;
                        z = 2;
                    }
                }
            }
        }
    }
}
[System.Serializable]
public class OnboardingLevels 
{   
    public GameObject[] onboardingChunks;
}
