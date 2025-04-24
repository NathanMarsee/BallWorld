using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRunnerManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] chunkList;
    public GameObject[] pillarList;
    public int spawnDistrance = 8;
    private int chunksCompleted = 0;
    private int nextChunk = 1;
    private int offsetX = 0;
    private int lastInstantiated = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z > chunksCompleted * 50)
        {
            chunksCompleted++;
        }
        if (nextChunk < chunksCompleted + spawnDistrance)
        {
            spawnChunk();
            nextChunk++;
        }
    }

    private void spawnChunk()
    {
        int nextInstantiated = UnityEngine.Random.Range(0, chunkList.Length);
        if (nextInstantiated == lastInstantiated)
            nextInstantiated++;
        if(nextInstantiated >= chunkList.Length)
            nextInstantiated = 0;
        Instantiate(chunkList[nextInstantiated], new Vector3(offsetX, nextChunk * -2, nextChunk * 50 + 25), new Quaternion(0, 0, 0, 0));
        Instantiate(pillarList[UnityEngine.Random.Range(0, pillarList.Length)], new Vector3(offsetX, nextChunk * -2, nextChunk * 50 + 25), new Quaternion(0, 0, 0, 0));
        offsetX += UnityEngine.Random.Range(-2, 3);
        lastInstantiated = nextInstantiated;
    }
}
