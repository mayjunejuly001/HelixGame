using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour
{
    private Vector2 lastTapPos;
    private Vector3 startRotation;

    public Transform topTransform;
    public Transform goalTransform;
    public GameObject helixLevelPrefab;
    public GameObject helixRodPrefab;  // The fake rod prefab

    public List<Stage> allStages = new List<Stage>();
    private float helixDistance;
    private List<GameObject> spawnedLevels = new List<GameObject>();

    [SerializeField] private Transform ballTransform;

    public int poolSize = 10; // Number of helix tiles to keep in memory
    private Queue<GameObject> levelPool = new Queue<GameObject>();
    private float currentLowestY;

    public float rodSpacing = 25f;            // Fixed spacing between rods
    private float nextTriggerY = -25f;        // Ball crossing triggers spawn
    private float rodSpawnOffset = -12.5f;    // Offset from trigger to spawn rod at bottom
    private GameObject lastSpawnedRod;



    private void Awake()
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + 0.1f);

        InitializePool();
    }

    private void InitializePool()
    {
        float levelHeight = helixDistance / poolSize;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject level = Instantiate(helixLevelPrefab, transform);
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            levelPool.Enqueue(level);
            spawnedLevels.Add(level);

            // ? Skip death parts for the first level
            ConfigureLevel(level, i != 0);

            spawnPosY -= levelHeight;
        }


        currentLowestY = spawnPosY;
    }

    private void ConfigureLevel(GameObject level, bool addDeathParts = true)
    {
        foreach (Transform t in level.transform)
        {
            t.gameObject.SetActive(true);
            Destroy(t.GetComponent<Death>());
        }

        int partCount = Random.Range(7, 12);
        int partsToDisable = 12 - partCount;
        List<GameObject> disabledParts = new List<GameObject>();

        while (disabledParts.Count < partsToDisable)
        {
            GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
            if (!disabledParts.Contains(randomPart))
            {
                randomPart.SetActive(false);
                disabledParts.Add(randomPart);
            }
        }

        foreach (Transform t in level.transform)
        {
            t.GetComponent<Renderer>().material.color = Color.yellow;
        }

        if (!addDeathParts) return; // ? Skip adding death parts

        List<GameObject> leftParts = new List<GameObject>();
        foreach (Transform t in level.transform)
        {
            if (t.gameObject.activeInHierarchy)
                leftParts.Add(t.gameObject);
        }

        int deathPartCount = Random.Range(1, 4);
        int count = 0;
        while (count < deathPartCount)
        {
            GameObject random = leftParts[Random.Range(0, leftParts.Count)];
            if (!random.GetComponent<Death>())
            {
                random.AddComponent<Death>();
                count++;
            }
        }
    }


    private void Update()
    {
        HandleInput();

        // Infinite helix tile scrolling using pool
        GameObject bottomLevel = levelPool.Peek();
        float distanceToBottom = ballTransform.position.y - bottomLevel.transform.position.y;

        if (distanceToBottom < -5f)
        {
            GameObject reusedLevel = levelPool.Dequeue();
            reusedLevel.transform.localPosition = new Vector3(0, currentLowestY, 0);
            currentLowestY -= (helixDistance / poolSize);
            ConfigureLevel(reusedLevel);
            levelPool.Enqueue(reusedLevel);
        }

        // Spawn fake rod when ball crosses Y milestones
        // Spawn fake rod when reaching end
        // Check if ball passed trigger Y level
        if (ballTransform.position.y < nextTriggerY)
        {
            // Destroy the last clone rod if it exists
            if (lastSpawnedRod != null)
            {
                Destroy(lastSpawnedRod);
            }

            float spawnY = nextTriggerY + rodSpawnOffset;
            Vector3 spawnPos = new Vector3(0, spawnY, 0);

            lastSpawnedRod = Instantiate(helixRodPrefab, spawnPos, Quaternion.identity, transform);
            lastSpawnedRod.transform.localScale = Vector3.one;
            lastSpawnedRod.name = "HelixRodClone";
            lastSpawnedRod.tag = "CloneRod";

            nextTriggerY -= rodSpacing;
        }




    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;

            if (lastTapPos == Vector2.zero)
            {
                lastTapPos = curTapPos;
            }

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }
    }

    //public void LoadStage(int stageNumber)
    //{
    //    Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];

    //    if (stage == null)
    //    {
    //        Debug.LogError("No stage " + stageNumber + " found in allStages list (HelixController). All stages assigned in list?");
    //        return;
    //    }

    //    Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;
    //    FindFirstObjectByType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageBallColor;

    //    transform.localEulerAngles = startRotation;

    //    foreach (GameObject go in spawnedLevels)
    //        Destroy(go);

    //    spawnedLevels.Clear();

    //    float levelDistance = helixDistance / stage.levels.Count;
    //    float spawnPosY = topTransform.localPosition.y;

    //    for (int i = 0; i < stage.levels.Count; i++)
    //    {
    //        spawnPosY -= levelDistance;
    //        GameObject level = Instantiate(helixLevelPrefab, transform);
    //        level.transform.localPosition = new Vector3(0, spawnPosY, 0);
    //        spawnedLevels.Add(level);

    //        int partsToDisable = 12 - stage.levels[i].partCount;
    //        List<GameObject> disabledParts = new List<GameObject>();

    //        while (disabledParts.Count < partsToDisable)
    //        {
    //            GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
    //            if (!disabledParts.Contains(randomPart))
    //            {
    //                randomPart.SetActive(false);
    //                disabledParts.Add(randomPart);
    //            }
    //        }

    //        List<GameObject> leftParts = new List<GameObject>();

    //        foreach (Transform t in level.transform)
    //        {
    //            t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;
    //            if (t.gameObject.activeInHierarchy)
    //                leftParts.Add(t.gameObject);
    //        }

    //        List<GameObject> deathParts = new List<GameObject>();

    //        while (deathParts.Count < stage.levels[i].deathPartCount)
    //        {
    //            GameObject randomPart = leftParts[Random.Range(0, leftParts.Count)];
    //            if (!deathParts.Contains(randomPart))
    //            {
    //                randomPart.AddComponent<Death>();
    //                deathParts.Add(randomPart);
    //            }
    //        }
    //    }
    //}
}
