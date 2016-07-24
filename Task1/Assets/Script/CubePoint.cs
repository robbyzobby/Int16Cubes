using UnityEngine;
using System.Collections;
public class CubePoint : Base
{
   
    public bool isGrowing = false;
    public bool isGrowingSide = false;
    public bool isGrowingRand = true;
    public float Randhigh;
    public float CubeLimit = 15F;
    public float CubeLow = 1F;
    public float growspeed ;
    public bool isOn = false;
    public int Num_X = 0;
    public int Num_Y = 0;
    GameObject[] neighbors;
    Spawner spawn;

    double startGameTime;
    
    double RandpauseTime;
    public double Randtime;
	// Use this for initialization
    void Awake()
    {
        Num_X = GameObject.FindGameObjectWithTag("Spawn").GetComponent<Spawner>().Added_X;
        Num_Y = GameObject.FindGameObjectWithTag("Spawn").GetComponent<Spawner>().Added_Y;
        startGameTime = Time.frameCount;
	}
	void Start () 
    {
        neighbors = new GameObject[4];
        Find_Neighborns();
        Randhigh = Random.Range(100, 500);
        Randtime = Random.Range(20, 300);
        RandpauseTime = Random.Range(20, 200);
        Randhigh = Randhigh / 100;
    }
	
	// Update is called once per frame
	void Update () 
    {
       Fall();
       Check_growing();
      if (!CenterChoosen )
       {
           RandomBehavior();
       }
       else if (CenterChoosen)
       {
           StopTime();
       }
	}
    double Timer()
    {
        return Time.frameCount - startGameTime;
    }
    void StopTime()
    {
        startGameTime = Time.frameCount;
    }
    

    void OnMouseOver()
    {
           Grow(CubeLimit *2);
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
}

    void Grow(float limit)
    {
        limit += 1; 
        if (isGrowingRand && limit -1 <= (int)transform.FindChild("Child").localScale.z )
        {
            isGrowingRand = false;          
        }
        if (transform.FindChild("Child").localScale.z <= limit)
        {
            growspeed = (limit - (transform.FindChild("Child").localScale.z)) / limit;
            transform.FindChild("Child").localScale += new Vector3(0, 0, growspeed);
        }
        
    }

    void Fall()
    {
        if (!isGrowing && !isGrowingSide)
        {
            if (transform.FindChild("Child").localScale.z > CubeLow)
            {
                growspeed = (transform.FindChild("Child").localScale.z - CubeLow) / CubeLimit;

                transform.FindChild("Child").localScale -= new Vector3(0, 0, growspeed);
            }
        }
    }

    //CHECK GROWING 
    void Check_growing()
    {
        foreach (var cube in neighbors)
        {
           if (cube != null && cube.GetComponent<CubePoint>().isGrowing )
            {
                if (!isGrowingSide && cube.transform.FindChild("Child").localScale.z > this.transform.FindChild("Child").localScale.z * 2)
                {
                    isGrowingSide = true;
                }
               Grow(SquareXY(CubeLimit));
            }
           else if (cube != null && !this.isGrowing && cube.GetComponent<CubePoint>().isGrowingSide && cube.transform.FindChild("Child").localScale.z >= this.transform.FindChild("Child").localScale.z )
             {
                 if (!isGrowingSide)
                 {
                     isGrowingSide = true;
                  }
                Grow(SquareXY(CubeLimit));
               }
        }
    }

    float SquareXY(float initial)
    {
       return (( initial / (Mathf.Sqrt( Mathf.Pow((Mathf.Abs(CenterY - Num_Y)) , 2) + Mathf.Pow((Mathf.Abs(CenterX - Num_X)) , 2) ))) );
    }
    public void NotGrowSide()
    {
        foreach (var cube in neighbors)
        {
            if (cube != null)
            {
                if (cube.GetComponent<CubePoint>().isGrowingSide)
                {
                    cube.GetComponent<CubePoint>().isGrowingSide = false;
                    cube.GetComponent<CubePoint>().NotGrowSide();
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
        else if (Timer() == Randtime)
        {
            RandpauseTime = Random.Range(20, 200);
         }
        else if (Timer() == (RandpauseTime + Randtime))
        {
             startGameTime = Time.frameCount;
            Randtime = Random.Range(20, 300);
          }
    }

    void Find_Neighborns()
    {
        GameObject [] All = GameObject.FindGameObjectsWithTag("Player");
        int neighcount = 0;
        foreach (var cube in All)
        {
            if ((cube.GetComponent<CubePoint>().Num_X == this.Num_X - 1 && cube.GetComponent<CubePoint>().Num_Y == this.Num_Y) ||
                (cube.GetComponent<CubePoint>().Num_X == this.Num_X + 1 && cube.GetComponent<CubePoint>().Num_Y == this.Num_Y) ||
                (cube.GetComponent<CubePoint>().Num_X == this.Num_X && cube.GetComponent<CubePoint>().Num_Y == this.Num_Y - 1) ||
                (cube.GetComponent<CubePoint>().Num_X == this.Num_X && cube.GetComponent<CubePoint>().Num_Y == this.Num_Y + 1))
            {
                neighbors[neighcount] = cube;
                neighcount++;
            }
        }
     }
}
