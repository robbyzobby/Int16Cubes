using UnityEngine;
using System.Collections;

public class CupPoint : BaseCap {
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
    CapSpawner spawn;

    public float rotatespeed = 0.5f;
    public float rotateborderAngle = 360f;
    bool TurnAround = false;

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
        Num_X = GameObject.FindGameObjectWithTag("Spawn").GetComponent<CapSpawner>().Added_X;
        Num_Y = GameObject.FindGameObjectWithTag("Spawn").GetComponent<CapSpawner>().Added_Y;
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
        rotateborderAngle = rotateborderAngle / 90;
        print(rotateborderAngle);
    }

    // Update is called once per frame
    void Update()
    {
        Fall();
        Check_growing();


        if (CenterChoosen && isGrowingRand)
        {
            isGrowingRand = false;
            transform.FindChild("Cola Text").gameObject.SetActive( false);
        }
        if (!CenterChoosen && Time.time > (AllPauseStart + DelayTime))
        {
            if (!isGrowingRand)
            {
                isGrowingRand = true;
                transform.FindChild("Cola Text").gameObject.SetActive(true);
            }
            if (isGrowingRand)
                RotateBehavior();

        }

    }

    void RotateBehavior()
    {
       if (isGrowingRand)
        {
            if (Mathf.Abs(transform.localRotation.y) < rotateborderAngle && !TurnAround)
            {
                transform.Rotate(Vector3.up, rotatespeed);
            }
           
            else if ( Mathf.Abs( transform.localRotation.y )< rotateborderAngle && TurnAround)
            {
              transform.Rotate(Vector3.up, -rotatespeed);
            }
            else if (Mathf.Abs(transform.localRotation.y) >= rotateborderAngle)
            {
                    
                TurnAround = !TurnAround;
                if (TurnAround)
                    transform.Rotate(Vector3.up, -rotatespeed);
                else if (!TurnAround)
                {
                    transform.Rotate(Vector3.up, rotatespeed);
                }
            }    
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
     void OnMouseEnter()
    {
        transform.FindChild("Cola Text").gameObject.SetActive(false);
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
            if (cube != null && cube.GetComponent<CupPoint>().isGrowing)
            {
                if (!isGrowingSide && cube.transform.FindChild("Child").localScale.y > this.transform.FindChild("Child").localScale.y * 2)
                {
                    isGrowingSide = true;
                }
                Grow(SquareXY(CubeLimit));
            }
            else if (cube != null && !this.isGrowing && cube.GetComponent<CupPoint>().isGrowingSide && cube.transform.FindChild("Child").localScale.y > this.transform.FindChild("Child").localScale.y * 1.2)
            {
                if (!isGrowingSide)
                    isGrowingSide = true;
                Grow(SquareXY(CubeLimit));
            }
        }
    }

    int SinKoef()
    {
        int CenX;
        int CenY;
        if (WaveCenterX % 1 != 0)
            CenX = Mathf.Abs(Num_X - (int)(WaveCenterX + 0.5));
        else
            CenX = Mathf.Abs(Num_X - (int)(WaveCenterX));

        if (WaveCenterY % 1 != 0)
            CenY = Mathf.Abs(Num_Y - (int)(WaveCenterY + 0.5));
        else
            CenY = Mathf.Abs(Num_Y - (int)(WaveCenterY));

        if (CenX >= CenY)
            return -(CenX % angle_divider) * 4;
        else
            return -(CenY % angle_divider) * 4;
    }

    float SquareXY(float initial)
    {
        return (initial / (Mathf.Sqrt(Mathf.Pow((Mathf.Abs(CenterY - Num_Y)), 2) + Mathf.Pow((Mathf.Abs(CenterX - Num_X)), 2))));
    }

    public void NotGrowSide()
    {
        foreach (var cube in neighbors)
        {
            if (cube != null)
            {
                if (cube.GetComponent<CupPoint>().isGrowingSide)
                {
                    cube.GetComponent<CupPoint>().isGrowingSide = false;
                    cube.GetComponent<CupPoint>().NotGrowSide();
                }
            }
        }
    }

    
    void RandomBehavior()
    {
        if (Timer() <= Randtime)
        {
            Grow(Randhigh);
        }
        else if ((int)Timer() == (int)Randtime)
        {
            PauseTime = Random.Range(1, 3);
        }
        else if ((int)Timer() == (int)(PauseTime + Randtime))
        {
           startPauseTime = Time.time;
            Randtime = Random.Range(1, 3);
        }
    }

    void Find_Neighborns()
    {
        GameObject[] All = GameObject.FindGameObjectsWithTag("Player");
        int neighcount = 0;
        foreach (var cube in All)
        {
            if ((cube.GetComponent<CupPoint>().Num_X == this.Num_X - 1 && cube.GetComponent<CupPoint>().Num_Y == this.Num_Y) ||
                (cube.GetComponent<CupPoint>().Num_X == this.Num_X + 1 && cube.GetComponent<CupPoint>().Num_Y == this.Num_Y) ||
                (cube.GetComponent<CupPoint>().Num_X == this.Num_X && cube.GetComponent<CupPoint>().Num_Y == this.Num_Y - 1) ||
                (cube.GetComponent<CupPoint>().Num_X == this.Num_X && cube.GetComponent<CupPoint>().Num_Y == this.Num_Y + 1))
            {
                neighbors[neighcount] = cube;
                neighcount++;
            }
        }
    }
}
