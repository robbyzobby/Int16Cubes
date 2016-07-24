using UnityEngine;
using System.Collections;

public class BaseTube : MonoBehaviour {

    public static Vector3 Center = new Vector3(0, 0, 0);
    public static int CenterX = 0;
    public static int CenterY = 0;
    public static double WaveCenterX = 0;
    public static double WaveCenterY = 0;
    public int CountX = 0;
    public int CountY = 0;
    public static bool CenterChoosen = false;

    public static double AllPauseStart;
    void Awake()
    {
        CountX = GameObject.FindGameObjectWithTag("Spawn").GetComponent<TubeSpawner>().CountX;
        CountY = GameObject.FindGameObjectWithTag("Spawn").GetComponent<TubeSpawner>().CountY;
        WaveCenterX = CountX / 2;
        WaveCenterY = CountY / 2;
        AllPauseStart = 0;
    }

    void Update()
    {
    }

}
