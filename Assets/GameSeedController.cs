using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSeedController : MonoBehaviour
{
    public int seed = 0;

    private void Awake()
    {
        if (seed == 0)
        {
            seed = Random.Range(0, 1000);
        }
        UnityEngine.Random.InitState(seed);
    }

    public int GetGameSeed()
    {
        return seed;
    }
}
