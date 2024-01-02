using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour {

    public string galaxyName;
    public int starResolution = 300; // max number of stars per galaxy sector
    public float radiusH = 6000f; // max distance from center x and z
    public float radiusV = 300f; // max y cordinate of stars from center
    [HideInInspector] public float diameterH; // galaxyRadiusH * 2
    [HideInInspector] public float diameterV; // galaxyRadiusV * 2
    [HideInInspector] public int gridDiameterH, gridDiameterV; // Horizontal and Vertival number of galaxy grid locs
    public Texture2D starIntensityMap; // black and white image used to define where stars should be placed
    public Texture2D galaxyIntensityMap1, galaxyIntensityMap2, galaxyIntensityMap3;
    
    [Range(0.0f, 1.0f)]
    public float starCutoff = 0.0125f;
    public GameObject spaceDust1Prefab, spaceDust2Prefab, spaceDust3Prefab;
    public Texture2D dust1ColorMap;
    [Range(0.0f, 1.0f)]
    public float dust1Alpha = 0.1f;
    public Color dust2Color;
    public Texture2D dust3ColorMap;
    [Range(0.0f, 1.0f)] public float dust3Alpha = 0.2f;
    [Range(0.025f, 12.0f)] public float dust1Size = 7f, dust2Size = 10f, dust3Size = 5f;
    private int lastDust1GridLocX = 0, lastDust1GridLocY = 0, lastDust2GridLocX = 0, lastDust2GridLocY = 0, lastDust3GridLocX = 0, lastDust3GridLocY = 0; // the last location we generated dust in
    public int dustDinsity = 1; // Number of dust sprites to create per pixel. Setting to 0 result in no dust being created, Setting to 2 will result in double, and so on.
    public long totalStars, dustCount; // How many stars & dust exist?
    private bool dust1Complete = false, dust2Complete = false, dust3Complete = false;
    [HideInInspector] public bool galaxyGenComplete = false; // Has the set number of dust particles been generated?
    // group of arrays that acts as a 3d grid that stores the number of stars of each class(M to A) that belong in each area of the galaxy
    [HideInInspector] public int[,,] starPopGridTotal, starPopGridM, starPopGridK, starPopGridG, starPopGridF, starPopGridA;
    private int[] starHeightPop; // total count of stars in each vertical galaxy grid position
    private Transform galaxyCamera;
    private Vector3 lastGCamPos;

    void Start ()
    {
        diameterH = radiusH * 2;
        diameterV = radiusV * 2;
        gridDiameterH = (int)(diameterH / 20f); // horizontal length of our galaxy grid
        gridDiameterV = (int)(diameterV / 20f); // vertical length of our galaxy grid
        print("Galaxy Grid DiamH: " + gridDiameterH);
        galaxyCamera = GameObject.FindObjectOfType<GalaxyCamera>().transform; // All galaxy particles should LookAt this camera.
        lastGCamPos = new Vector3(0f,0f,0f);

        // stores the apropriate number of stars for every pixel of the galaxy texture using its intensity (white is max stars, black is none)
        starPopGridM = new int[gridDiameterH, gridDiameterH, gridDiameterV];
        starPopGridK = new int[gridDiameterH, gridDiameterH, gridDiameterV];
        starPopGridG = new int[gridDiameterH, gridDiameterH, gridDiameterV];
        starPopGridF = new int[gridDiameterH, gridDiameterH, gridDiameterV];
        starPopGridA = new int[gridDiameterH, gridDiameterH, gridDiameterV];
        starPopGridTotal = new int[gridDiameterH, gridDiameterH, gridDiameterV];
        starHeightPop = new int[gridDiameterV];

        // populate the 3d star pop array with stars
        for (int i = 0; i < gridDiameterH; i++)
        {
            int xPixel = 0;
            if(i != 0) xPixel = (int)(((float)i / gridDiameterH) * starIntensityMap.width);
            for (int j = 0; j < gridDiameterH; j++)
            {
                int yPixel = 0;
                if(j != 0) yPixel = (int)(((float)j / gridDiameterH) * starIntensityMap.height);
                float pixelValue = starIntensityMap.GetPixel(xPixel, yPixel).r;
                if (pixelValue - starCutoff < 0.0f) pixelValue = 0.0f;

                // calculate the total number of stars in each vertical sector
                float vCenterF = (((gridDiameterV - 1) / 2)); // vertical center of galaxy
                for (int k = 0; k < gridDiameterV; k++)
                {
                    if (k != 0 && (vCenterF - Mathf.Abs(vCenterF - k)) > 0)
                        starPopGridTotal[i, j, k] = (int)((((vCenterF - Mathf.Abs(vCenterF - k)) / vCenterF) * pixelValue) * starResolution);
                    else
                        starPopGridTotal[i, j, k] = (int)(((0.5f / vCenterF) * pixelValue) * starResolution);
                    starHeightPop[k] += starPopGridTotal[i, j, k];
                    totalStars += starPopGridTotal[i, j, k];
                }
            }
        }
        print("Total Stars In Galaxy: " + totalStars);
        print("Vertical star population on the galaxy grid:");
        for (int i = 0; i < starHeightPop.GetLength(0); i++)
        {
            print(i - ((starHeightPop.GetLength(0) - 1) / 2) + ": " + starHeightPop[i]);
        }
    }

    private void Update()
    {
        if (Vector3.Distance(galaxyCamera.position, lastGCamPos) > 0.025f)
        {
            for (int i = 0; i < transform.childCount - 1; i++)
                transform.GetChild(i).LookAt(galaxyCamera.position);
            lastGCamPos = galaxyCamera.position;
        }
        if (!galaxyGenComplete) GenerateAllStarDust();
    }

    // Generate the galaxy star dust in the background
    private void GenerateAllStarDust()
    {
        bool firstIteration = true;
        bool limitReached = false;
        int numDustGeneratedThisLoop = 0; //number of dust generated in this loop (add ++ each iteration)
        if (!dust1Complete)
        {
            for (int i = lastDust1GridLocX; i < galaxyIntensityMap1.width; i++)
            {
                int jValue = 0; // set this to lastGridLocY so that we can continue where we left off last update only if this is the first iteration
                if (firstIteration)
                {
                    jValue = lastDust1GridLocY;
                    firstIteration = false;
                }
                int locsSinceLastDust = 0; // how long since we created a dust sprite
                for (int j = jValue; j < galaxyIntensityMap1.height; j++)
                {
                    locsSinceLastDust++;
                    // we only want to generate 1000 objects per frame or less
                    if (numDustGeneratedThisLoop > 1000)
                    {
                        lastDust1GridLocX = i; // store the i and j locations in the grid so we can continue where we left off next update
                        lastDust1GridLocY = j;
                        limitReached = true; // we need this to break from the outer i for loop
                        break;
                    }
                    // calculate the area we want this iteration of galaxy dust to spawn in
                    float radiusOfDust = 10; // galaxy dust renders to its own camera and the area it spawns is very small relative to the star spawn area
                    float dustAreaX = -radiusOfDust + (((radiusOfDust * 2) / galaxyIntensityMap1.width) * i);
                    float dustAreaZ = -radiusOfDust + (((radiusOfDust * 2) / galaxyIntensityMap1.height) * j);
                    // Spawn Galaxy Dust
                    if ((galaxyIntensityMap1.GetPixel(i, j).r > 0.01f || galaxyIntensityMap1.GetPixel(i, j).r > 0.01f || galaxyIntensityMap1.GetPixel(i, j).r > 0.01f) &&  dustDinsity > 0)
                    {
                        float pSize = dust1Size - ((1f - galaxyIntensityMap1.GetPixel(i, j).r) * dust1Size);
                        for (int d = 0; d < dustDinsity; d++)
                        {
                            if (pSize > dust1Size * 0.3f)
                            {
                                GameObject spaceDust = Instantiate(spaceDust1Prefab, new Vector3(Random.Range(dustAreaX, dustAreaX + (((radiusOfDust * 2) / galaxyIntensityMap1.width))), Random.Range(-0.2f, 0.2f), Random.Range(dustAreaZ, dustAreaZ + (((radiusOfDust * 2) / galaxyIntensityMap1.height)))), new Quaternion(0, 0, 0, 0), transform);
                                float alphaValue = galaxyIntensityMap1.GetPixel(i, j).r * dust1Alpha;
                                if (alphaValue < dust1Alpha * 0.5f) alphaValue = (dust1Alpha * 0.5f);
                                spaceDust.GetComponent<SpriteRenderer>().material.color = new Color(dust1ColorMap.GetPixel(i, j).r, dust1ColorMap.GetPixel(i, j).g, dust1ColorMap.GetPixel(i, j).b, alphaValue);
                                spaceDust.transform.localScale = new Vector3(pSize, pSize, pSize);
                                spaceDust.transform.localPosition = spaceDust.transform.position;
                                dustCount++;
                                locsSinceLastDust = 0;
                                numDustGeneratedThisLoop++;
                            }
                        }
                    }
                }
                if (i == galaxyIntensityMap1.width - 1)
                    dust1Complete = true;
                if (limitReached) break;
            }
        }
        if (!dust2Complete)
        {
            for (int i = lastDust2GridLocX; i < galaxyIntensityMap2.width; i++)
            {
                int jValue = 0; // set this to lastGridLocY so that we can continue where we left off last update only if this is the first iteration
                if (firstIteration)
                {
                    jValue = lastDust2GridLocY;
                    firstIteration = false;
                }
                int locsSinceLastDust = 0; // how long since we created a dust sprite
                for (int j = jValue; j < galaxyIntensityMap2.height; j++)
                {
                    locsSinceLastDust++;
                    // we only want to generate 1000 objects per frame or less
                    if (numDustGeneratedThisLoop > 1000)
                    {
                        lastDust2GridLocX = i; // store the i and j locations in the grid so we can continue where we left off next update
                        lastDust2GridLocY = j;
                        limitReached = true; // we need this to break from the outer i for loop
                        break;
                    }
                    // calculate the area we want this iteration of galaxy dust to spawn in
                    float radiusOfDust = 10; // galaxy dust renders to its own camera and the area it spawns is very small relative to the star spawn area
                    float dustAreaX = -radiusOfDust + (((radiusOfDust * 2) / galaxyIntensityMap2.width) * i);
                    float dustAreaZ = -radiusOfDust + (((radiusOfDust * 2) / galaxyIntensityMap2.height) * j);
                    // Spawn Galaxy Dust
                    if ((galaxyIntensityMap2.GetPixel(i, j).r > 0.01f || galaxyIntensityMap2.GetPixel(i, j).r > 0.01f || galaxyIntensityMap2.GetPixel(i, j).r > 0.01f) && dustDinsity > 0)
                    {
                        float pSize = dust2Size - ((1f - galaxyIntensityMap2.GetPixel(i, j).r) * dust2Size);
                        for (int d = 0; d < dustDinsity; d++)
                        {
                            if (pSize > dust2Size * 0.35f)
                            {
                                GameObject spaceDust = Instantiate(spaceDust2Prefab, new Vector3(Random.Range(dustAreaX, dustAreaX + (((radiusOfDust * 2) / galaxyIntensityMap2.width))), Random.Range(-0.2f, 0.2f), Random.Range(dustAreaZ, dustAreaZ + (((radiusOfDust * 2) / galaxyIntensityMap2.height)))), new Quaternion(0, 0, 0, 0), transform);
                                float alphaValue = galaxyIntensityMap2.GetPixel(i, j).r * dust2Color.a;
                                if (alphaValue < dust2Color.a * 0.3f) alphaValue = (dust2Color.a * 0.3f);
                                spaceDust.GetComponent<SpriteRenderer>().material.color = new Color(dust2Color.r, dust2Color.g, dust2Color.b, alphaValue);
                                spaceDust.transform.localScale = new Vector3(pSize, pSize, pSize);
                                spaceDust.transform.localPosition = spaceDust.transform.position;
                                dustCount++;
                                locsSinceLastDust = 0;
                                numDustGeneratedThisLoop++;
                            }
                        }
                    }
                }
                if (i == galaxyIntensityMap2.width - 1)
                    dust2Complete = true;
                if (limitReached) break;
            }
        }
        if (!dust3Complete)
        {
            for (int i = lastDust3GridLocX; i < galaxyIntensityMap3.width; i++)
            {
                int jValue = 0; // set this to lastGridLocY so that we can continue where we left off last update only if this is the first iteration
                if (firstIteration)
                {
                    jValue = lastDust3GridLocY;
                    firstIteration = false;
                }
                int locsSinceLastDust = 0; // how long since we created a dust sprite
                for (int j = jValue; j < galaxyIntensityMap3.height; j++)
                {
                    locsSinceLastDust++;
                    // we only want to generate 1000 objects per frame or less
                    if (numDustGeneratedThisLoop > 1000)
                    {
                        lastDust3GridLocX = i; // store the i and j locations in the grid so we can continue where we left off next update
                        lastDust3GridLocY = j;
                        limitReached = true; // we need this to break from the outer i for loop
                        break;
                    }
                    // calculate the area we want this iteration of galaxy dust to spawn in
                    float radiusOfDust = 10; // galaxy dust renders to its own camera and the area it spawns is very small relative to the star spawn area
                    float dustAreaX = -radiusOfDust + (((radiusOfDust * 2) / galaxyIntensityMap3.width) * i);
                    float dustAreaZ = -radiusOfDust + (((radiusOfDust * 2) / galaxyIntensityMap3.height) * j);
                    // Spawn Galaxy Dust
                    if ((galaxyIntensityMap3.GetPixel(i, j).r > 0.01f || galaxyIntensityMap3.GetPixel(i, j).r > 0.01f || galaxyIntensityMap3.GetPixel(i, j).r > 0.01f) && dustDinsity > 0)
                    {
                        float pSize = dust3Size - ((1f - galaxyIntensityMap3.GetPixel(i, j).r) * dust3Size);
                        int dust3Density = 1;
                        if (dustDinsity > 1) dust3Density = 2;
                        for (int d = 0; d < dust3Density; d++) {
                            if (pSize > dust3Size * 0.3f)
                            {
                                GameObject spaceDust = Instantiate(spaceDust3Prefab, new Vector3(Random.Range(dustAreaX, dustAreaX + (((radiusOfDust * 2) / galaxyIntensityMap3.width))), Random.Range(-0.1f, 0.1f), Random.Range(dustAreaZ, dustAreaZ + (((radiusOfDust * 2) / galaxyIntensityMap3.height)))), new Quaternion(0, 0, 0, 0), transform);
                                float dustA = (galaxyIntensityMap3.GetPixel(i, j).r * dust3Alpha);
                                if (dustA < dust3Alpha * 0.3f) dustA = dust3Alpha * 0.3f;
                                spaceDust.GetComponent<SpriteRenderer>().material.color = new Color(dust3ColorMap.GetPixel(i, j).r, dust3ColorMap.GetPixel(i, j).g, dust3ColorMap.GetPixel(i, j).b, dustA);
                                spaceDust.transform.localScale = new Vector3(pSize, pSize, pSize);
                                spaceDust.transform.localPosition = spaceDust.transform.position;
                                dustCount++;
                                numDustGeneratedThisLoop++;
                                locsSinceLastDust = 0;
                            }
                        }
                    }
                }
                if (i == galaxyIntensityMap3.width - 1)
                    dust3Complete = true;
                if (limitReached) break;
            }
        }
        if (dust1Complete && dust2Complete && dust3Complete)
        {
            galaxyGenComplete = true;
            for (int i = 0; i < transform.childCount - 1; i++)
                transform.GetChild(i).LookAt(galaxyCamera.position);
            print("Dust Gen complete! Count: " + dustCount);
        }
    }
}