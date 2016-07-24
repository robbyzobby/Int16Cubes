using UnityEngine;
using System.Collections;

public class TubePoint : BaseTube {

    public bool isGrowing = false;
    public bool isGrowingSide = false;
    public bool isGrowingRand = true;
    public float Randhigh;
    public float CubeLimit = 15F;
    public float CubeLow = 1F;
    public float growspeed;
    public bool isOn = false;
    public int Num_X = 0;
    public int Num_Y = 0;
    int angle_divider = 36;
    GameObject[] neighbors;
    TubeSpawner spawn;



    public double DelayTime;
    public double MaxHigh;
    double InstantScale;
    int InstantSinx;


    double startPauseTime;

    double PauseTime;
    public double Randtime;
    // Use this for initialization
    void Awake()
    {
        Num_X = GameObject.FindGameObjectWithTag("Spawn").GetComponent<TubeSpawner>().Added_X;
        Num_Y = GameObject.FindGameObjectWithTag("Spawn").GetComponent<TubeSpawner>().Added_Y;
      //  startPauseTime = Time.time;
    }
    void Start()
    {
        DelayTime = 2;
        MaxHigh = 0.1;
        neighbors = new GameObject[4];
        Find_Neighborns();
        Randhigh = Random.Range(2, 10);
        Randtime = Random.Range(1, 10);
        PauseTime = Random.Range(1, 10);
        InstantSinx = SinKoef();
    }

    // Update is called once per frame
    void Update()
    {
        Fall();
        Check_growing();


        if (CenterChoosen && isGrowingRand)
        {
            isGrowingRand = false;

        }
        if (!CenterChoosen && Time.time > (AllPauseStart + DelayTime) )
        {
            if (!isGrowingRand)
            {
                isGrowingRand = true;
                InstantSinx = SinKoef();
            }
            if(isGrowingRand)
            WaveBehavior();

        }
     }



    double Timer()
    {
        return Time.time - startPauseTime;
    }
    void StopTime()
    {
        startPauseTime = Time.time;
    }
    void OnMouseOver()
    {
        if (isGrowingRand)
        {
            isGrowingRand = false;
        }
        Grow(CubeLimit * 2);
        if (!isGrowing)
        {
            isGrowing = true;
            Center = this.transform.position;
            CenterX = Num_X;
            CenterY = Num_Y;
            CenterChoosen = true;
        }
       

    }

    void OnMouseExit()
    {
        Fall();
        NotGrowSide();
        CenterChoosen = false;
        isGrowing = false;
        AllPauseStart = Time.time;
    }

    void Grow(float limit)
    {
        if (isGrowingRand && limit - 1 <= (int)transform.FindChild("Child").localScale.y)
        {
            isGrowingRand = false;
        }
        if (transform.FindChild("Child").localScale.y <= limit)
        {
            growspeed = (limit - (transform.FindChild("Child").localScale.y)) / limit;
            transform.FindChild("Child").localScale += new Vector3(0, growspeed, 0);
        }

    }

    void Fall()
    {
        if (!isGrowing && !isGrowingSide && !isGrowingRand)
        {
            if (transform.FindChild("Child").localScale.y > CubeLow)
            {
                growspeed = (transform.FindChild("Child").localScale.y - CubeLow) / CubeLimit;

                transform.FindChild("Child").localScale -= new Vector3(0, growspeed, 0);
            }
        }
    }

    //CHECK GROWING 
    void Check_growing()
    {
        foreach (var cube in neighbors)
        {
            if (cube != null && cube.GetComponent<TubePoint>().isGrowing)
            {
                if (!isGrowingSide && cube.transform.FindChild("Child").localScale.y > this.transform.FindChild("Child").localScale.y * 2)
                {
                    isGrowingSide = true;
                }
                Grow(SquareXY(CubeLimit));
            }
            else if (cube != null && !this.isGrowing && cube.GetComponent<TubePoint>().isGrowingSide && cube.transform.FindChild("Child").localScale.y > this.transform.FindChild("Child").localScale.y )
            {
                if (!isGrowingSide)
                    isGrowingSide = true;
                Grow(SquareXY(CubeLimit));
            }
        }
    }

    int SinKoef()
    {
      //  print("OK");
       int CenX; 
       int CenY; 
        if(WaveCenterX %1 != 0)
            CenX = Mathf.Abs(Num_X - (int)(WaveCenterX + 0.5));
        else
            CenX = Mathf.Abs(Num_X - (int)(WaveCenterX )); 
        
        if(WaveCenterY %1 != 0)
            CenY = Mathf.Abs(Num_Y- (int)(WaveCenterY + 0.5));
        else
            CenY = Mathf.Abs(Num_Y - (int)(WaveCenterY ));

       if (CenX >= CenY)
           return -(CenX % angle_divider)*4;
       else
           return -(CenY % angle_divider)*4; 
    }

    float SquareXY(float initial)
    {
       return (initial / (Mathf.Sqrt(Mathf.Pow((Mathf.Abs(CenterY - Num_Y)), 2) + Mathf.Pow((Mathf.Abs(CenterX - Num_X)), 2))))*0.48f;
    }

    public void NotGrowSide()
    {
        foreach (var cube in neighbors)
        {
            if (cube != null)
            {
                if (cube.GetComponent<TubePoint>().isGrowingSide)
                {
                    cube.GetComponent<TubePoint>().isGrowingSide = false;
                    cube.GetComponent<TubePoint>().NotGrowSide();
                }
            }
        }
    }

    void WaveBehavior()
    {
        if (isGrowingRand)
        {
           // print(InstantScale);
            InstantScale = ((MaxHigh) * Mathf.Sin(InstantSinx * (Mathf.PI / angle_divider))) ;
            if (InstantSinx >= 0) 
            transform.FindChild("Child").localScale += new Vector3(0, (float)InstantScale, 0);
            
            InstantSinx++;
        }
    }

    void Find_Neighborns()
    {
        GameObject[] All = GameObject.FindGameObjectsWithTag("Player");
        int neighcount = 0;
        foreach (var cube in All)
        {
            if ((cube.GetComponent<TubePoint>().Num_X == this.Num_X - 1 && cube.GetComponent<TubePoint>().Num_Y == this.Num_Y) ||
                (cube.GetComponent<TubePoint>().Num_X == this.Num_X + 1 && cube.GetComponent<TubePoint>().Num_Y == this.Num_Y) ||
                (cube.GetComponent<TubePoint>().Num_X == this.Num_X && cube.GetComponent<TubePoint>().Num_Y == this.Num_Y - 1) ||
                (cube.GetComponent<TubePoint>().Num_X == this.Num_X && cube.GetComponent<TubePoint>().Num_Y == this.Num_Y + 1))
            {
                neighbors[neighcount] = cube;
                neighcount++;
            }
        }
    }
}
