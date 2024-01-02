using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGeneration : MonoBehaviour
{

    public float maxStarSize = 1.25f;
    public GameObject starPrefab;
    public int seed = 42; // Random number seed
    private GameObject starContainer; // The parents of the stars and galactic dust particles
    private List<Transform> poolStars; // this is where we will store all star GameObjects until we need more stars
    public Galaxy galaxy; // The galaxy that we are near or inside of
    // a group of arrays of Lists for each class of stars representing each sector
    private List<Transform>[,,] sectorStarsM;
    private List<Transform>[,,] sectorStarsK;
    private List<Transform>[,,] sectorStarsG;
    private List<Transform>[,,] sectorStarsF;
    private List<Transform>[,,] sectorStarsA;
    // the bounding boxes for each star in each sector to detect clicking because giving stars colliders resulted in terrible performance
    private List<Bounds>[,,] sectorStarBoundsM;
    private List<Bounds>[,,] sectorStarBoundsK;
    private List<Bounds>[,,] sectorStarBoundsG;
    private List<Bounds>[,,] sectorStarBoundsF;
    private List<Bounds>[,,] sectorStarBoundsA;
    // Very important! These cubed are the sizes of the star grids of the different classes.
    private const int gridSectorSizeM = 3;
    private const int gridCenterSectorM = (int)((gridSectorSizeM - 1) / 2);
    private const int gridSectorSizeK = 5;
    private const int gridCenterSectorK = (int)((gridSectorSizeK - 1) / 2);
    private const int gridSectorSizeG = 7;
    private const int gridCenterSectorG = (int)((gridSectorSizeG - 1) / 2);
    private const int gridSectorSizeF = 11;
    private const int gridCenterSectorF = (int)((gridSectorSizeF - 1) / 2);
    private const int gridSectorSizeA = 23;
    private const int gridCenterSectorA = (int)((gridSectorSizeA - 1) / 2);
    private const float gSectDiameterH = 20; // horizontal diameter of 1 sector of the grid
    private const float gSectDiameterV = 20; // vertical diameter of 1 sector of the grid
    // Position of each respective sector in the galaxy grid to be generated
    private Vector3Int[,,] starSectorGridPositionsM;
    private Vector3Int[,,] starSectorGridPositionsK;
    private Vector3Int[,,] starSectorGridPositionsG;
    private Vector3Int[,,] starSectorGridPositionsF;
    private Vector3Int[,,] starSectorGridPositionsA;
    private Vector3Int lastCenterSectorGridPos; // This is the last location the camera was in thus the center of our sector grid

    private bool hasGeneratedFirstStars = false;
    //private bool galaxyGenComplete = false; Have all of the galaxies generated yet?

    private float timeSincePrintStarPoolStats = 0f;
    private int mostStarsUsedAtOnce = 0;

    private Transform starCam;
    private int cToScale = 0; // Scale and rotate only one class of stars every frame
    private bool adjustedNearStars = false; // have we scaled and rotated nearby stars to match new camera location yet
    private bool adjustedFarStars = false; // have we scaled and rotated far away stars to match new camera location yet
    private Vector3 lastDistantStarUpdatePos; // last position the starCam was when the far away stars were updated
    private Vector3 lastCenterStarUpdatePos; // last cam pos when the center stars updated

    void Start()
    {
        Random.InitState(seed);

        starContainer = GameObject.Find("StarContainer");
        starCam = GameObject.FindObjectOfType<StarCamera>().transform;
        lastDistantStarUpdatePos = starCam.position;
        lastCenterStarUpdatePos = starCam.position;

        sectorStarsM = new List<Transform>[gridSectorSizeM, gridSectorSizeM, gridSectorSizeM];
        sectorStarsK = new List<Transform>[gridSectorSizeK, gridSectorSizeK, gridSectorSizeK];
        sectorStarsG = new List<Transform>[gridSectorSizeG, gridSectorSizeG, gridSectorSizeG];
        sectorStarsF = new List<Transform>[gridSectorSizeF, gridSectorSizeF, gridSectorSizeF];
        sectorStarsA = new List<Transform>[gridSectorSizeA, gridSectorSizeA, gridSectorSizeA];
        // initialize the arrays of respective positions to that grid
        starSectorGridPositionsM = new Vector3Int[gridSectorSizeM, gridSectorSizeM, gridSectorSizeM];
        starSectorGridPositionsK = new Vector3Int[gridSectorSizeK, gridSectorSizeK, gridSectorSizeK];
        starSectorGridPositionsG = new Vector3Int[gridSectorSizeG, gridSectorSizeG, gridSectorSizeG];
        starSectorGridPositionsF = new Vector3Int[gridSectorSizeF, gridSectorSizeF, gridSectorSizeF];
        starSectorGridPositionsA = new Vector3Int[gridSectorSizeA, gridSectorSizeA, gridSectorSizeA];
        // initialize our array to hold in use star bounds for click detection
        sectorStarBoundsM = new List<Bounds>[gridSectorSizeM, gridSectorSizeM, gridSectorSizeM];
        sectorStarBoundsK = new List<Bounds>[gridSectorSizeK, gridSectorSizeK, gridSectorSizeK];
        sectorStarBoundsG = new List<Bounds>[gridSectorSizeG, gridSectorSizeG, gridSectorSizeG];
        sectorStarBoundsF = new List<Bounds>[gridSectorSizeF, gridSectorSizeF, gridSectorSizeF];
        sectorStarBoundsA = new List<Bounds>[gridSectorSizeA, gridSectorSizeA, gridSectorSizeA];
        for (int i = 0; i < gridSectorSizeM; i++)
        {
            for (int j = 0; j < gridSectorSizeM; j++)
            {
                for (int k = 0; k < gridSectorSizeM; k++)
                {
                    sectorStarsM[i, j, k] = new List<Transform>();
                    starSectorGridPositionsM[i, j, k] = new Vector3Int();
                    sectorStarBoundsM[i, j, k] = new List<Bounds>();
                }
            }
        }
        for (int i = 0; i < gridSectorSizeK; i++)
        {
            for (int j = 0; j < gridSectorSizeK; j++)
            {
                for (int k = 0; k < gridSectorSizeK; k++)
                {
                    sectorStarsK[i, j, k] = new List<Transform>();
                    starSectorGridPositionsK[i, j, k] = new Vector3Int();
                    sectorStarBoundsK[i, j, k] = new List<Bounds>();
                }
            }
        }
        for (int i = 0; i < gridSectorSizeG; i++)
        {
            for (int j = 0; j < gridSectorSizeG; j++)
            {
                for (int k = 0; k < gridSectorSizeG; k++)
                {
                    sectorStarsG[i, j, k] = new List<Transform>();
                    starSectorGridPositionsG[i, j, k] = new Vector3Int();
                    sectorStarBoundsG[i, j, k] = new List<Bounds>();
                }
            }
        }
        for (int i = 0; i < gridSectorSizeF; i++)
        {
            for (int j = 0; j < gridSectorSizeF; j++)
            {
                for (int k = 0; k < gridSectorSizeF; k++)
                {
                    sectorStarsF[i, j, k] = new List<Transform>();
                    starSectorGridPositionsF[i, j, k] = new Vector3Int();
                    sectorStarBoundsF[i, j, k] = new List<Bounds>();
                }
            }
        }
        for (int i = 0; i < gridSectorSizeA; i++)
        {
            for (int j = 0; j < gridSectorSizeA; j++)
            {
                for (int k = 0; k < gridSectorSizeA; k++)
                {
                    sectorStarsA[i, j, k] = new List<Transform>();
                    starSectorGridPositionsA[i, j, k] = new Vector3Int();
                    sectorStarBoundsA[i, j, k] = new List<Bounds>();
                }
            }
        }

        // Here we populate our Star Pool with stars for later use. Note that every pixel in our galaxy image can hold only a maximum of starResolutions
        // value of stars so we only have to populate the pool for the x and z cordinates in our grid
        int numOfStarsInPool = 30000;
        poolStars = new List<Transform>(); // this is where all of the stars not in use should be placed
        for (int s = 0; s < numOfStarsInPool; s++)
        {
            GameObject star = Instantiate(starPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), starContainer.transform);
            star.transform.GetComponent<Star>().mat = star.transform.GetComponent<SpriteRenderer>().material;
            poolStars.Add(star.transform);
        }
    }

    public void Update()
    {
        
    }

    // Move stars along the grid Sectors and call to generate new ones in new grid locations moving forward and so on
    private void ManageStars()
    {
        Vector3 camPos = starCam.position;

        Vector3Int centerGridLoc = new Vector3Int(); // Location of the star grid sector that the camera is in
        centerGridLoc.x = (int)((((camPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
        centerGridLoc.y = (int)(((((camPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
        centerGridLoc.z = (int)((((camPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));

        Vector3Int differenceInLoc = centerGridLoc - lastCenterSectorGridPos;
        if (centerGridLoc != lastCenterSectorGridPos) // if we have moved to a new grid location
        {
            lastCenterSectorGridPos = centerGridLoc;
        }

        // move grid on X
        if ((differenceInLoc.x == 1 || differenceInLoc.x == -1) && hasGeneratedFirstStars)
        {
            int PorN = differenceInLoc.x;
            int xStart = 0, xEnd = gridSectorSizeM - 1;
            if (PorN == -1)
            {
                xStart = gridSectorSizeM - 1;
                xEnd = 0;
            }
            for (int y = 0; y < gridSectorSizeM; y++)
            {
                for (int z = 0; z < gridSectorSizeM; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsM[xStart, y, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsM[xStart, y, z];
                    for (int x = xStart; (x < gridSectorSizeM) && (x >= 0); x += PorN)
                    {
                        if (x == xEnd)
                        {
                            sectorStarsM[x, y, z] = tempStarHolster;
                            sectorStarBoundsM[x, y, z] = tempBoundsHolster;
                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorM)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorM)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorM)) + camPos.z;

                            starSectorGridPositionsM[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsM[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsM[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsM[x, y, z], new Vector3Int(x, y, z), 'M');
                        }
                        else
                        {
                            starSectorGridPositionsM[x, y, z] = starSectorGridPositionsM[x + PorN, y, z];
                            sectorStarsM[x, y, z] = sectorStarsM[x + PorN, y, z];
                            sectorStarBoundsM[x, y, z] = sectorStarBoundsM[x + PorN, y, z];
                        }
                    }
                }
            }
            xStart = 0;
            xEnd = gridSectorSizeK - 1;
            if (PorN == -1)
            {
                xStart = gridSectorSizeK - 1;
                xEnd = 0;
            }
            for (int y = 0; y < gridSectorSizeK; y++)
            {
                for (int z = 0; z < gridSectorSizeK; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsK[xStart, y, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsK[xStart, y, z];
                    for (int x = xStart; (x < gridSectorSizeK) && (x >= 0); x += PorN)
                    {
                        if (x == xEnd)
                        {
                            sectorStarsK[x, y, z] = tempStarHolster;
                            sectorStarBoundsK[x, y, z] = tempBoundsHolster;
                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorK)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorK)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorK)) + camPos.z;

                            starSectorGridPositionsK[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsK[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsK[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsK[x, y, z], new Vector3Int(x, y, z), 'K');
                        }
                        else
                        {
                            starSectorGridPositionsK[x, y, z] = starSectorGridPositionsK[x + PorN, y, z];
                            sectorStarsK[x, y, z] = sectorStarsK[x + PorN, y, z];
                            sectorStarBoundsK[x, y, z] = sectorStarBoundsK[x + PorN, y, z];
                        }
                    }
                }
            }
            xStart = 0;
            xEnd = gridSectorSizeG - 1;
            if (PorN == -1)
            {
                xStart = gridSectorSizeG - 1;
                xEnd = 0;
            }
            for (int y = 0; y < gridSectorSizeG; y++)
            {
                for (int z = 0; z < gridSectorSizeG; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsG[xStart, y, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsG[xStart, y, z];
                    for (int x = xStart; (x < gridSectorSizeG) && (x >= 0); x += PorN)
                    {
                        if (x == xEnd)
                        {
                            sectorStarsG[x, y, z] = tempStarHolster;
                            sectorStarBoundsG[x, y, z] = tempBoundsHolster;
                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorG)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorG)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorG)) + camPos.z;

                            starSectorGridPositionsG[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsG[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsG[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsG[x, y, z], new Vector3Int(x, y, z), 'G');
                        }
                        else
                        {
                            starSectorGridPositionsG[x, y, z] = starSectorGridPositionsG[x + PorN, y, z];
                            sectorStarsG[x, y, z] = sectorStarsG[x + PorN, y, z];
                            sectorStarBoundsG[x, y, z] = sectorStarBoundsG[x + PorN, y, z];
                        }
                    }
                }
            }
            xStart = 0;
            xEnd = gridSectorSizeF - 1;
            if (PorN == -1)
            {
                xStart = gridSectorSizeF - 1;
                xEnd = 0;
            }
            for (int y = 0; y < gridSectorSizeF; y++)
            {
                for (int z = 0; z < gridSectorSizeF; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsF[xStart, y, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsF[xStart, y, z];
                    for (int x = xStart; (x < gridSectorSizeF) && (x >= 0); x += PorN)
                    {
                        if (x == xEnd)
                        {
                            sectorStarsF[x, y, z] = tempStarHolster;
                            sectorStarBoundsF[x, y, z] = tempBoundsHolster;
                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorF)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorF)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorF)) + camPos.z;

                            starSectorGridPositionsF[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsF[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsF[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsF[x, y, z], new Vector3Int(x, y, z), 'F');
                        }
                        else
                        {
                            starSectorGridPositionsF[x, y, z] = starSectorGridPositionsF[x + PorN, y, z];
                            sectorStarsF[x, y, z] = sectorStarsF[x + PorN, y, z];
                            sectorStarBoundsF[x, y, z] = sectorStarBoundsF[x + PorN, y, z];
                        }
                    }
                }
            }
            xStart = 0;
            xEnd = gridSectorSizeA - 1;
            if (PorN == -1)
            {
                xStart = gridSectorSizeA - 1;
                xEnd = 0;
            }
            for (int y = 0; y < gridSectorSizeA; y++)
            {
                for (int z = 0; z < gridSectorSizeA; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsA[xStart, y, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsA[xStart, y, z];
                    for (int x = xStart; (x < gridSectorSizeA) && (x >= 0); x += PorN)
                    {
                        if (x == xEnd)
                        {
                            sectorStarsA[x, y, z] = tempStarHolster;
                            sectorStarBoundsA[x, y, z] = tempBoundsHolster;
                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorA)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorA)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorA)) + camPos.z;

                            starSectorGridPositionsA[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsA[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsA[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsA[x, y, z], new Vector3Int(x, y, z), 'A');
                        }
                        else
                        {
                            starSectorGridPositionsA[x, y, z] = starSectorGridPositionsA[x + PorN, y, z];
                            sectorStarsA[x, y, z] = sectorStarsA[x + PorN, y, z];
                            sectorStarBoundsA[x, y, z] = sectorStarBoundsA[x + PorN, y, z];
                        }
                    }
                }
            }
        }
        // move grid on Z
        if ((differenceInLoc.z == 1 || differenceInLoc.z == -1) && hasGeneratedFirstStars)
        {
            int PorN = differenceInLoc.z;
            int zStart = 0, zEnd = gridSectorSizeM - 1;
            if (PorN == -1)
            {
                zStart = gridSectorSizeM - 1;
                zEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeM; x++)
            {
                for (int y = 0; y < gridSectorSizeM; y++)
                {
                    List<Transform> tempStarHolster = sectorStarsM[x, y, zStart];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsM[x, y, zStart];
                    for (int z = zStart; (z < gridSectorSizeM) && (z >= 0); z += PorN)
                    {
                        if (z == zEnd)
                        {
                            sectorStarsM[x, y, z] = tempStarHolster;
                            sectorStarBoundsM[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorM)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorM)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorM)) + camPos.z;

                            starSectorGridPositionsM[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsM[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsM[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsM[x, y, z], new Vector3Int(x, y, z), 'M');
                        }
                        else
                        {
                            starSectorGridPositionsM[x, y, z] = starSectorGridPositionsM[x, y, z + PorN];
                            sectorStarsM[x, y, z] = sectorStarsM[x, y, z + PorN];
                            sectorStarBoundsM[x, y, z] = sectorStarBoundsM[x, y, z + PorN];
                        }
                    }
                }
            }
            zStart = 0;
            zEnd = gridSectorSizeK - 1;
            if (PorN == -1)
            {
                zStart = gridSectorSizeK - 1;
                zEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeK; x++)
            {
                for (int y = 0; y < gridSectorSizeK; y++)
                {
                    List<Transform> tempStarHolster = sectorStarsK[x, y, zStart];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsK[x, y, zStart];
                    for (int z = zStart; (z < gridSectorSizeK) && (z >= 0); z += PorN)
                    {
                        if (z == zEnd)
                        {
                            sectorStarsK[x, y, z] = tempStarHolster;
                            sectorStarBoundsK[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorK)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorK)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorK)) + camPos.z;

                            starSectorGridPositionsK[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsK[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsK[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsK[x, y, z], new Vector3Int(x, y, z), 'K');
                        }
                        else
                        {
                            starSectorGridPositionsK[x, y, z] = starSectorGridPositionsK[x, y, z + PorN];
                            sectorStarsK[x, y, z] = sectorStarsK[x, y, z + PorN];
                            sectorStarBoundsK[x, y, z] = sectorStarBoundsK[x, y, z + PorN];
                        }
                    }
                }
            }
            zStart = 0;
            zEnd = gridSectorSizeG - 1;
            if (PorN == -1)
            {
                zStart = gridSectorSizeG - 1;
                zEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeG; x++)
            {
                for (int y = 0; y < gridSectorSizeG; y++)
                {
                    List<Transform> tempStarHolster = sectorStarsG[x, y, zStart];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsG[x, y, zStart];
                    for (int z = zStart; (z < gridSectorSizeG) && (z >= 0); z += PorN)
                    {
                        if (z == zEnd)
                        {
                            sectorStarsG[x, y, z] = tempStarHolster;
                            sectorStarBoundsG[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorG)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorG)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorG)) + camPos.z;

                            starSectorGridPositionsG[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsG[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsG[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsG[x, y, z], new Vector3Int(x, y, z), 'G');
                        }
                        else
                        {
                            starSectorGridPositionsG[x, y, z] = starSectorGridPositionsG[x, y, z + PorN];
                            sectorStarsG[x, y, z] = sectorStarsG[x, y, z + PorN];
                            sectorStarBoundsG[x, y, z] = sectorStarBoundsG[x, y, z + PorN];
                        }
                    }
                }
            }
            zStart = 0;
            zEnd = gridSectorSizeF - 1;
            if (PorN == -1)
            {
                zStart = gridSectorSizeF - 1;
                zEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeF; x++)
            {
                for (int y = 0; y < gridSectorSizeF; y++)
                {
                    List<Transform> tempStarHolster = sectorStarsF[x, y, zStart];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsF[x, y, zStart];
                    for (int z = zStart; (z < gridSectorSizeF) && (z >= 0); z += PorN)
                    {
                        if (z == zEnd)
                        {
                            sectorStarsF[x, y, z] = tempStarHolster;
                            sectorStarBoundsF[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorF)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorF)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorF)) + camPos.z;

                            starSectorGridPositionsF[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsF[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsF[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsF[x, y, z], new Vector3Int(x, y, z), 'F');
                        }
                        else
                        {
                            starSectorGridPositionsF[x, y, z] = starSectorGridPositionsF[x, y, z + PorN];
                            sectorStarsF[x, y, z] = sectorStarsF[x, y, z + PorN];
                            sectorStarBoundsF[x, y, z] = sectorStarBoundsF[x, y, z + PorN];
                        }
                    }
                }
            }
            zStart = 0;
            zEnd = gridSectorSizeA - 1;
            if (PorN == -1)
            {
                zStart = gridSectorSizeA - 1;
                zEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeA; x++)
            {
                for (int y = 0; y < gridSectorSizeA; y++)
                {
                    List<Transform> tempStarHolster = sectorStarsA[x, y, zStart];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsA[x, y, zStart];
                    for (int z = zStart; (z < gridSectorSizeA) && (z >= 0); z += PorN)
                    {
                        if (z == zEnd)
                        {
                            sectorStarsA[x, y, z] = tempStarHolster;
                            sectorStarBoundsA[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorA)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorA)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorA)) + camPos.z;

                            starSectorGridPositionsA[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsA[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsA[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsA[x, y, z], new Vector3Int(x, y, z), 'A');
                        }
                        else
                        {
                            starSectorGridPositionsA[x, y, z] = starSectorGridPositionsA[x, y, z + PorN];
                            sectorStarsA[x, y, z] = sectorStarsA[x, y, z + PorN];
                            sectorStarBoundsA[x, y, z] = sectorStarBoundsA[x, y, z + PorN];
                        }
                    }
                }
            }
        }
        // move on y
        if ((differenceInLoc.y == 1 || differenceInLoc.y == -1) && hasGeneratedFirstStars)
        {
            int PorN = differenceInLoc.y * -1;
            int yStart = 0, yEnd = gridSectorSizeM - 1;
            if (PorN == -1)
            {
                yStart = gridSectorSizeM - 1;
                yEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeM; x++)
            {
                for (int z = 0; z < gridSectorSizeM; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsM[x, yStart, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsM[x, yStart, z];
                    for (int y = yStart; (y < gridSectorSizeM) && (y >= 0); y += PorN)
                    {
                        if (y == yEnd)
                        {
                            sectorStarsM[x, y, z] = tempStarHolster;
                            sectorStarBoundsM[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorM)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorM)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorM)) + camPos.z;

                            starSectorGridPositionsM[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsM[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsM[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsM[x, y, z], new Vector3Int(x, y, z), 'M');
                        }
                        else
                        {
                            starSectorGridPositionsM[x, y, z] = starSectorGridPositionsM[x, y + PorN, z];
                            sectorStarsM[x, y, z] = sectorStarsM[x, y + PorN, z];
                            sectorStarBoundsM[x, y, z] = sectorStarBoundsM[x, y + PorN, z];
                        }
                    }
                }
            }
            yStart = 0;
            yEnd = gridSectorSizeK - 1;
            if (PorN == -1)
            {
                yStart = gridSectorSizeK - 1;
                yEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeK; x++)
            {
                for (int z = 0; z < gridSectorSizeK; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsK[x, yStart, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsK[x, yStart, z];
                    for (int y = yStart; (y < gridSectorSizeK) && (y >= 0); y += PorN)
                    {
                        if (y == yEnd)
                        {
                            sectorStarsK[x, y, z] = tempStarHolster;
                            sectorStarBoundsK[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorK)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorK)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorK)) + camPos.z;

                            starSectorGridPositionsK[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsK[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsK[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsK[x, y, z], new Vector3Int(x, y, z), 'K');
                        }
                        else
                        {
                            starSectorGridPositionsK[x, y, z] = starSectorGridPositionsK[x, y + PorN, z];
                            sectorStarsK[x, y, z] = sectorStarsK[x, y + PorN, z];
                            sectorStarBoundsK[x, y, z] = sectorStarBoundsK[x, y + PorN, z];
                        }
                    }
                }
            }
            yStart = 0;
            yEnd = gridSectorSizeG - 1;
            if (PorN == -1)
            {
                yStart = gridSectorSizeG - 1;
                yEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeG; x++)
            {
                for (int z = 0; z < gridSectorSizeG; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsG[x, yStart, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsG[x, yStart, z];
                    for (int y = yStart; (y < gridSectorSizeG) && (y >= 0); y += PorN)
                    {
                        if (y == yEnd)
                        {
                            sectorStarsG[x, y, z] = tempStarHolster;
                            sectorStarBoundsG[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorG)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorG)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorG)) + camPos.z;

                            starSectorGridPositionsG[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsG[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsG[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsG[x, y, z], new Vector3Int(x, y, z), 'G');
                        }
                        else
                        {
                            starSectorGridPositionsG[x, y, z] = starSectorGridPositionsG[x, y + PorN, z];
                            sectorStarsG[x, y, z] = sectorStarsG[x, y + PorN, z];
                            sectorStarBoundsG[x, y, z] = sectorStarBoundsG[x, y + PorN, z];
                        }
                    }
                }
            }
            yStart = 0;
            yEnd = gridSectorSizeF - 1;
            if (PorN == -1)
            {
                yStart = gridSectorSizeF - 1;
                yEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeF; x++)
            {
                for (int z = 0; z < gridSectorSizeF; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsF[x, yStart, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsF[x, yStart, z];
                    for (int y = yStart; (y < gridSectorSizeF) && (y >= 0); y += PorN)
                    {
                        if (y == yEnd)
                        {
                            sectorStarsF[x, y, z] = tempStarHolster;
                            sectorStarBoundsF[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorF)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorF)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorF)) + camPos.z;

                            starSectorGridPositionsF[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsF[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsF[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsF[x, y, z], new Vector3Int(x, y, z), 'F');
                        }
                        else
                        {
                            starSectorGridPositionsF[x, y, z] = starSectorGridPositionsF[x, y + PorN, z];
                            sectorStarsF[x, y, z] = sectorStarsF[x, y + PorN, z];
                            sectorStarBoundsF[x, y, z] = sectorStarBoundsF[x, y + PorN, z];
                        }
                    }
                }
            }
            yStart = 0;
            yEnd = gridSectorSizeA - 1;
            if (PorN == -1)
            {
                yStart = gridSectorSizeA - 1;
                yEnd = 0;
            }
            for (int x = 0; x < gridSectorSizeA; x++)
            {
                for (int z = 0; z < gridSectorSizeA; z++)
                {
                    List<Transform> tempStarHolster = sectorStarsA[x, yStart, z];
                    List<Bounds> tempBoundsHolster = sectorStarBoundsA[x, yStart, z];
                    for (int y = yStart; (y < gridSectorSizeA) && (y >= 0); y += PorN)
                    {
                        if (y == yEnd)
                        {
                            sectorStarsA[x, y, z] = tempStarHolster;
                            sectorStarBoundsA[x, y, z] = tempBoundsHolster;

                            Vector3 sectorPos;
                            sectorPos.x = (gSectDiameterH * (x - gridCenterSectorA)) + camPos.x;
                            sectorPos.y = (gSectDiameterV * (y - gridCenterSectorA)) + camPos.y;
                            sectorPos.z = (gSectDiameterH * (z - gridCenterSectorA)) + camPos.z;

                            starSectorGridPositionsA[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            starSectorGridPositionsA[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                            starSectorGridPositionsA[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                            GenerateStarsInArea(starSectorGridPositionsA[x, y, z], new Vector3Int(x, y, z), 'A');
                        }
                        else
                        {
                            starSectorGridPositionsA[x, y, z] = starSectorGridPositionsA[x, y + PorN, z];
                            sectorStarsA[x, y, z] = sectorStarsA[x, y + PorN, z];
                            sectorStarBoundsA[x, y, z] = sectorStarBoundsA[x, y + PorN, z];
                        }
                    }
                }
            }
        }

        if ((differenceInLoc.x > 1 || differenceInLoc.x < -1) || (differenceInLoc.y > 1 || differenceInLoc.y < -1) || (differenceInLoc.z > 1 || differenceInLoc.z < -1))
        {
            //print("Updating All Star Sectors: " + differenceInLoc);
            for (int x = 0; x < gridSectorSizeM; x++)
            {
                for (int y = 0; y < gridSectorSizeM; y++)
                {
                    for (int z = 0; z < gridSectorSizeM; z++)
                    {
                        Vector3 sectorPos;
                        sectorPos.x = (gSectDiameterH * (x - gridCenterSectorM)) + camPos.x;
                        sectorPos.y = (gSectDiameterV * (y - gridCenterSectorM)) + camPos.y;
                        sectorPos.z = (gSectDiameterH * (z - gridCenterSectorM)) + camPos.z;

                        starSectorGridPositionsM[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        starSectorGridPositionsM[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                        starSectorGridPositionsM[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        GenerateStarsInArea(starSectorGridPositionsM[x, y, z], new Vector3Int(x, y, z), 'M');
                    }
                }
            }
            for (int x = 0; x < gridSectorSizeK; x++)
            {
                for (int y = 0; y < gridSectorSizeK; y++)
                {
                    for (int z = 0; z < gridSectorSizeK; z++)
                    {
                        Vector3 sectorPos;
                        sectorPos.x = (gSectDiameterH * (x - gridCenterSectorK)) + camPos.x;
                        sectorPos.y = (gSectDiameterV * (y - gridCenterSectorK)) + camPos.y;
                        sectorPos.z = (gSectDiameterH * (z - gridCenterSectorK)) + camPos.z;

                        starSectorGridPositionsK[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        starSectorGridPositionsK[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                        starSectorGridPositionsK[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        GenerateStarsInArea(starSectorGridPositionsK[x, y, z], new Vector3Int(x, y, z), 'K');
                    }
                }
            }
            for (int x = 0; x < gridSectorSizeG; x++)
            {
                for (int y = 0; y < gridSectorSizeG; y++)
                {
                    for (int z = 0; z < gridSectorSizeG; z++)
                    {
                        Vector3 sectorPos;
                        sectorPos.x = (gSectDiameterH * (x - gridCenterSectorG)) + camPos.x;
                        sectorPos.y = (gSectDiameterV * (y - gridCenterSectorG)) + camPos.y;
                        sectorPos.z = (gSectDiameterH * (z - gridCenterSectorG)) + camPos.z;

                        starSectorGridPositionsG[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        starSectorGridPositionsG[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                        starSectorGridPositionsG[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        GenerateStarsInArea(starSectorGridPositionsG[x, y, z], new Vector3Int(x, y, z), 'G');
                    }
                }
            }
            for (int x = 0; x < gridSectorSizeF; x++)
            {
                for (int y = 0; y < gridSectorSizeF; y++)
                {
                    for (int z = 0; z < gridSectorSizeF; z++)
                    {
                        Vector3 sectorPos;
                        sectorPos.x = (gSectDiameterH * (x - gridCenterSectorF)) + camPos.x;
                        sectorPos.y = (gSectDiameterV * (y - gridCenterSectorF)) + camPos.y;
                        sectorPos.z = (gSectDiameterH * (z - gridCenterSectorF)) + camPos.z;

                        starSectorGridPositionsF[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        starSectorGridPositionsF[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                        starSectorGridPositionsF[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        GenerateStarsInArea(starSectorGridPositionsF[x, y, z], new Vector3Int(x, y, z), 'F');
                    }
                }
            }
            for (int x = 0; x < gridSectorSizeA; x++)
            {
                for (int y = 0; y < gridSectorSizeA; y++)
                {
                    for (int z = 0; z < gridSectorSizeA; z++)
                    {
                        Vector3 sectorPos;
                        sectorPos.x = (gSectDiameterH * (x - gridCenterSectorA)) + camPos.x;
                        sectorPos.y = (gSectDiameterV * (y - gridCenterSectorA)) + camPos.y;
                        sectorPos.z = (gSectDiameterH * (z - gridCenterSectorA)) + camPos.z;

                        starSectorGridPositionsA[x, y, z].x = (int)((((sectorPos.x - (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        starSectorGridPositionsA[x, y, z].y = (int)(((((sectorPos.y + (gSectDiameterV / 2)) * -1) / galaxy.radiusV) + 1) * (galaxy.gridDiameterV / 2));
                        starSectorGridPositionsA[x, y, z].z = (int)((((sectorPos.z + (gSectDiameterH / 2)) / galaxy.radiusH) + 1) * (galaxy.gridDiameterH / 2));
                        GenerateStarsInArea(starSectorGridPositionsA[x, y, z], new Vector3Int(x, y, z), 'A');
                    }
                }
            }
            hasGeneratedFirstStars = true;
        }
    }

    // generates stars in a given location of the grid to a given parent
    private void GenerateStarsInArea(Vector3Int gridLoc, Vector3Int sectorID, char starClass)
    {
        if (gridLoc.x > galaxy.gridDiameterH - 1 || gridLoc.x <= 0 || gridLoc.y > galaxy.gridDiameterV - 1 || gridLoc.y <= 0 || gridLoc.z > galaxy.gridDiameterH - 1 || gridLoc.z <= 0)
        {
            // If we are outside of the galaxy grid then it's safe to assume there are no stars so throw all the ones in use back into the pool
            if (starClass == 'M')
            {
                if (sectorStarsM[sectorID.x, sectorID.y, sectorID.z].Count != 0)
                {
                    for (int i = sectorStarsM[sectorID.x, sectorID.y, sectorID.z].Count - 1; i >= 0; i--)
                    {
                        sectorStarsM[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsM[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsM[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
            }
            else if (starClass == 'K')
            {
                if (sectorStarsK[sectorID.x, sectorID.y, sectorID.z].Count != 0)
                {
                    for (int i = sectorStarsK[sectorID.x, sectorID.y, sectorID.z].Count - 1; i >= 0; i--)
                    {
                        sectorStarsK[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsK[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsK[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
            }
            else if (starClass == 'G')
            {
                if (sectorStarsG[sectorID.x, sectorID.y, sectorID.z].Count != 0)
                {
                    for (int i = sectorStarsG[sectorID.x, sectorID.y, sectorID.z].Count - 1; i >= 0; i--)
                    {
                        sectorStarsG[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsG[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsG[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
            }
            else if (starClass == 'F')
            {
                if (sectorStarsF[sectorID.x, sectorID.y, sectorID.z].Count != 0)
                {
                    for (int i = sectorStarsF[sectorID.x, sectorID.y, sectorID.z].Count - 1; i >= 0; i--)
                    {
                        sectorStarsF[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsF[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsF[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
            }
            else if (starClass == 'A')
            {
                if (sectorStarsA[sectorID.x, sectorID.y, sectorID.z].Count != 0)
                {
                    for (int i = sectorStarsA[sectorID.x, sectorID.y, sectorID.z].Count - 1; i >= 0; i--)
                    {
                        sectorStarsA[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsA[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsA[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
            }
        }
        else
        {   // lets generate some stars now
            // determine the chunk of the area this grid location represents and the number of stars that should exist here
            float areaStartX = (-galaxy.radiusH + gSectDiameterH * (gridLoc.x + 1)) - (gSectDiameterH / 2);
            float areaEndX = areaStartX + gSectDiameterH;
            float areaStartY = (galaxy.radiusV - (gSectDiameterV * (gridLoc.y))) - (gSectDiameterV / 2);
            float areaEndY = areaStartY - gSectDiameterV;
            float areaStartZ = (-galaxy.radiusH + (gSectDiameterH * (gridLoc.z + 1))) - (gSectDiameterH / 2);
            float areaEndZ = areaStartZ - gSectDiameterH;

            float probFloat = 0f; // Use this guy to store probabilities.

            // if star class counts have not been populated for this grid location then do it now
            if (galaxy.starPopGridM[gridLoc.x, gridLoc.z, gridLoc.y] + galaxy.starPopGridK[gridLoc.x, gridLoc.z, gridLoc.y] + galaxy.starPopGridG[gridLoc.x, gridLoc.z, gridLoc.y] + galaxy.starPopGridF[gridLoc.x, gridLoc.z, gridLoc.y] + galaxy.starPopGridA[gridLoc.x, gridLoc.z, gridLoc.y] != galaxy.starPopGridTotal[gridLoc.x, gridLoc.z, gridLoc.y])
            {
                Random.InitState(seed + (gridLoc.x * gridLoc.y * gridLoc.z) + galaxy.starPopGridTotal[gridLoc.x, gridLoc.z, gridLoc.y]);
                // randomly calculate the classifications of the stars in this sector
                for (int s = 0; s < galaxy.starPopGridTotal[gridLoc.x, gridLoc.z, gridLoc.y]; s++)
                {
                    float randStarClassNum = Random.Range(0f, 100f);
                    if (randStarClassNum <= 75.0f) // Class M
                        galaxy.starPopGridM[gridLoc.x, gridLoc.z, gridLoc.y]++;
                    else if (randStarClassNum <= 89.0f) // Class K
                        galaxy.starPopGridK[gridLoc.x, gridLoc.z, gridLoc.y]++;
                    else if (randStarClassNum <= 96.4f) // Class G
                        galaxy.starPopGridG[gridLoc.x, gridLoc.z, gridLoc.y]++;
                    else if (randStarClassNum <= 99.4f) // Class F
                        galaxy.starPopGridF[gridLoc.x, gridLoc.z, gridLoc.y]++;
                    else // Class A
                        galaxy.starPopGridA[gridLoc.x, gridLoc.z, gridLoc.y]++;
                }
            }

            if (starClass == 'M')
            {
                int numOfStarsToMakeM = galaxy.starPopGridM[gridLoc.x, gridLoc.z, gridLoc.y];
                Random.InitState(seed + ((gridLoc.x * gridLoc.y * gridLoc.z) + numOfStarsToMakeM) * starClass);

                // Set up the right number of M
                if (sectorStarsM[sectorID.x, sectorID.y, sectorID.z].Count > numOfStarsToMakeM)
                {
                    for (int i = sectorStarsM[sectorID.x, sectorID.y, sectorID.z].Count - 1; i > numOfStarsToMakeM - 1; i--)
                    {
                        sectorStarsM[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsM[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsM[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
                else if (sectorStarsM[sectorID.x, sectorID.y, sectorID.z].Count < numOfStarsToMakeM)
                {
                    for (int i = sectorStarsM[sectorID.x, sectorID.y, sectorID.z].Count - 1; i < numOfStarsToMakeM - 1; i++)
                    {
                        sectorStarsM[sectorID.x, sectorID.y, sectorID.z].Add(poolStars[poolStars.Count - 1]);
                        poolStars.RemoveAt(poolStars.Count - 1);
                    }
                }
                sectorStarBoundsM[sectorID.x, sectorID.y, sectorID.z].Clear();
                // Procedurally Generate M Stars
                for (int i = 0; i < sectorStarsM[sectorID.x, sectorID.y, sectorID.z].Count; i++)
                {
                    Transform star = sectorStarsM[sectorID.x, sectorID.y, sectorID.z][i];
                    star.gameObject.SetActive(true);
                    star.position = new Vector3(Random.Range(areaStartX, areaEndX), Random.Range(areaStartY, areaEndY), Random.Range(areaStartZ, areaEndZ));
                    sectorStarBoundsM[sectorID.x, sectorID.y, sectorID.z].Add(new Bounds(star.position, new Vector3(0.15f, 0.15f, 0.15f)));
                    Star starScript = star.GetComponent<Star>(); //get the star script atached to this star prefab so we can set some properties
                    starScript.spectralClass = "M";
                    probFloat = Random.Range(0f, 10f);
                    if (probFloat < 4f)
                        starScript.luminosity = Random.Range(0.001f, 0.02f);
                    else if (probFloat < 7.5f)
                        starScript.luminosity = Random.Range(0.02f, 0.04f);
                    else if (probFloat < 9.5f)
                        starScript.luminosity = Random.Range(0.04f, 0.07f);
                    else
                        starScript.luminosity = Random.Range(0.07f, 0.08f);
                    starScript.PrepStar();
                    starScript.ResizeStar(starCam.position, false);
                    star.LookAt(starCam);
                }
            }
            else if (starClass == 'K')
            {
                int numOfStarsToMakeK = galaxy.starPopGridK[gridLoc.x, gridLoc.z, gridLoc.y];
                Random.InitState(seed + ((gridLoc.x * gridLoc.y * gridLoc.z) + numOfStarsToMakeK) * starClass);

                // Set up the right number of K
                if (sectorStarsK[sectorID.x, sectorID.y, sectorID.z].Count > numOfStarsToMakeK)
                {
                    for (int i = sectorStarsK[sectorID.x, sectorID.y, sectorID.z].Count - 1; i > numOfStarsToMakeK - 1; i--)
                    {
                        sectorStarsK[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsK[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsK[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
                else if (sectorStarsK[sectorID.x, sectorID.y, sectorID.z].Count < numOfStarsToMakeK)
                {
                    for (int i = sectorStarsK[sectorID.x, sectorID.y, sectorID.z].Count - 1; i < numOfStarsToMakeK - 1; i++)
                    {
                        sectorStarsK[sectorID.x, sectorID.y, sectorID.z].Add(poolStars[poolStars.Count - 1]);
                        poolStars.RemoveAt(poolStars.Count - 1);
                    }
                }
                sectorStarBoundsK[sectorID.x, sectorID.y, sectorID.z].Clear();
                // Procedurally Generate K Stars
                for (int i = 0; i < sectorStarsK[sectorID.x, sectorID.y, sectorID.z].Count; i++)
                {
                    Transform star = sectorStarsK[sectorID.x, sectorID.y, sectorID.z][i];
                    star.gameObject.SetActive(true);
                    star.position = new Vector3(Random.Range(areaStartX, areaEndX), Random.Range(areaStartY, areaEndY), Random.Range(areaStartZ, areaEndZ));
                    sectorStarBoundsK[sectorID.x, sectorID.y, sectorID.z].Add(new Bounds(star.position, new Vector3(0.15f, 0.15f, 0.15f)));
                    Star starScript = star.GetComponent<Star>(); //get the star script atached to this star prefab so we can set some properties
                    starScript.spectralClass = "K";
                    probFloat = Random.Range(0f, 10f);
                    if (probFloat < 4f)
                        starScript.luminosity = Random.Range(0.08f, 0.16f);
                    else if (probFloat < 7.5f)
                        starScript.luminosity = Random.Range(0.16f, 0.35f);
                    else if (probFloat < 9.5f)
                        starScript.luminosity = Random.Range(0.35f, 0.5f);
                    else
                        starScript.luminosity = Random.Range(0.5f, 0.6f);
                    starScript.PrepStar();
                    starScript.ResizeStar(starCam.position, false);
                    star.LookAt(starCam);
                }
            }
            else if (starClass == 'G')
            {
                int numOfStarsToMakeG = galaxy.starPopGridG[gridLoc.x, gridLoc.z, gridLoc.y];
                Random.InitState(seed + ((gridLoc.x * gridLoc.y * gridLoc.z) + numOfStarsToMakeG) * starClass);

                // Set up the right number of G
                if (sectorStarsG[sectorID.x, sectorID.y, sectorID.z].Count > numOfStarsToMakeG)
                {
                    for (int i = sectorStarsG[sectorID.x, sectorID.y, sectorID.z].Count - 1; i > numOfStarsToMakeG - 1; i--)
                    {
                        sectorStarsG[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsG[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsG[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
                else if (sectorStarsG[sectorID.x, sectorID.y, sectorID.z].Count < numOfStarsToMakeG)
                {
                    for (int i = sectorStarsG[sectorID.x, sectorID.y, sectorID.z].Count - 1; i < numOfStarsToMakeG - 1; i++)
                    {
                        sectorStarsG[sectorID.x, sectorID.y, sectorID.z].Add(poolStars[poolStars.Count - 1]);
                        poolStars.RemoveAt(poolStars.Count - 1);
                    }
                }
                sectorStarBoundsG[sectorID.x, sectorID.y, sectorID.z].Clear();
                // Procedurally Generate G Stars
                for (int i = 0; i < sectorStarsG[sectorID.x, sectorID.y, sectorID.z].Count; i++)
                {
                    Transform star = sectorStarsG[sectorID.x, sectorID.y, sectorID.z][i];
                    star.gameObject.SetActive(true);
                    star.position = new Vector3(Random.Range(areaStartX, areaEndX), Random.Range(areaStartY, areaEndY), Random.Range(areaStartZ, areaEndZ));
                    sectorStarBoundsG[sectorID.x, sectorID.y, sectorID.z].Add(new Bounds(star.position, new Vector3(0.15f, 0.15f, 0.15f)));
                    Star starScript = star.GetComponent<Star>(); //get the star script atached to this star prefab so we can set some properties
                    starScript.spectralClass = "G";
                    probFloat = Random.Range(0f, 10f);
                    if (probFloat < 4f)
                        starScript.luminosity = Random.Range(0.6f, 1f);
                    else if (probFloat < 7.5f)
                        starScript.luminosity = Random.Range(1f, 1.2f);
                    else if (probFloat < 9.5f)
                        starScript.luminosity = Random.Range(1.2f, 1.35f);
                    else
                        starScript.luminosity = Random.Range(1.35f, 1.5f);
                    starScript.PrepStar();
                    starScript.ResizeStar(starCam.position, false);
                    star.LookAt(starCam);
                }
            }
            else if (starClass == 'F')
            {
                int numOfStarsToMakeF = galaxy.starPopGridF[gridLoc.x, gridLoc.z, gridLoc.y];
                Random.InitState(seed + ((gridLoc.x * gridLoc.y * gridLoc.z) + numOfStarsToMakeF) * starClass);

                // Set up the right number of F
                if (sectorStarsF[sectorID.x, sectorID.y, sectorID.z].Count > numOfStarsToMakeF)
                {
                    for (int i = sectorStarsF[sectorID.x, sectorID.y, sectorID.z].Count - 1; i > numOfStarsToMakeF - 1; i--)
                    {
                        sectorStarsF[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsF[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsF[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
                else if (sectorStarsF[sectorID.x, sectorID.y, sectorID.z].Count < numOfStarsToMakeF)
                {
                    for (int i = sectorStarsF[sectorID.x, sectorID.y, sectorID.z].Count - 1; i < numOfStarsToMakeF - 1; i++)
                    {
                        sectorStarsF[sectorID.x, sectorID.y, sectorID.z].Add(poolStars[poolStars.Count - 1]);
                        poolStars.RemoveAt(poolStars.Count - 1);
                    }
                }
                sectorStarBoundsF[sectorID.x, sectorID.y, sectorID.z].Clear();
                // Procedurally Generate F Stars
                for (int i = 0; i < sectorStarsF[sectorID.x, sectorID.y, sectorID.z].Count; i++)
                {
                    Transform star = sectorStarsF[sectorID.x, sectorID.y, sectorID.z][i];
                    star.gameObject.SetActive(true);
                    star.position = new Vector3(Random.Range(areaStartX, areaEndX), Random.Range(areaStartY, areaEndY), Random.Range(areaStartZ, areaEndZ));
                    sectorStarBoundsF[sectorID.x, sectorID.y, sectorID.z].Add(new Bounds(star.position, new Vector3(0.15f, 0.15f, 0.15f)));
                    Star starScript = star.GetComponent<Star>(); //get the star script atached to this star prefab so we can set some properties
                    starScript.spectralClass = "F";
                    probFloat = Random.Range(0f, 10f);
                    if (probFloat < 4f)
                        starScript.luminosity = Random.Range(1.5f, 2.25f);
                    else if (probFloat < 7.5f)
                        starScript.luminosity = Random.Range(2.25f, 3f);
                    else if (probFloat < 9.5f)
                        starScript.luminosity = Random.Range(3f, 3.5f);
                    else
                        starScript.luminosity = Random.Range(3.5f, 5f);
                    starScript.PrepStar();
                    starScript.ResizeStar(starCam.position, false);
                    star.LookAt(starCam);
                }
            }
            else if (starClass == 'A')
            {
                int numOfStarsToMakeA = galaxy.starPopGridA[gridLoc.x, gridLoc.z, gridLoc.y];
                Random.InitState(seed + ((gridLoc.x * gridLoc.y * gridLoc.z) + numOfStarsToMakeA) * starClass);

                // Setup the right number of A stars
                if (sectorStarsA[sectorID.x, sectorID.y, sectorID.z].Count > numOfStarsToMakeA)
                {
                    for (int i = sectorStarsA[sectorID.x, sectorID.y, sectorID.z].Count - 1; i > numOfStarsToMakeA - 1; i--)
                    {
                        sectorStarsA[sectorID.x, sectorID.y, sectorID.z][i].gameObject.SetActive(false);
                        poolStars.Add(sectorStarsA[sectorID.x, sectorID.y, sectorID.z][i]);
                        sectorStarsA[sectorID.x, sectorID.y, sectorID.z].RemoveAt(i);
                    }
                }
                else if (sectorStarsA[sectorID.x, sectorID.y, sectorID.z].Count < numOfStarsToMakeA)
                {
                    for (int i = sectorStarsA[sectorID.x, sectorID.y, sectorID.z].Count - 1; i < numOfStarsToMakeA - 1; i++)
                    {
                        sectorStarsA[sectorID.x, sectorID.y, sectorID.z].Add(poolStars[poolStars.Count - 1]);
                        poolStars.RemoveAt(poolStars.Count - 1);
                    }
                }
                sectorStarBoundsA[sectorID.x, sectorID.y, sectorID.z].Clear();
                // Procedurally Generate A Stars
                for (int i = 0; i < sectorStarsA[sectorID.x, sectorID.y, sectorID.z].Count; i++)
                {
                    Transform star = sectorStarsA[sectorID.x, sectorID.y, sectorID.z][i];
                    star.gameObject.SetActive(true);
                    star.position = new Vector3(Random.Range(areaStartX, areaEndX), Random.Range(areaStartY, areaEndY), Random.Range(areaStartZ, areaEndZ));
                    sectorStarBoundsA[sectorID.x, sectorID.y, sectorID.z].Add(new Bounds(star.position, new Vector3(0.15f, 0.15f, 0.15f)));
                    Star starScript = star.GetComponent<Star>(); //get the star script atached to this star prefab so we can set some properties
                    starScript.spectralClass = "A";
                    probFloat = Random.Range(0f, 10f);
                    if (probFloat < 5f)
                        starScript.luminosity = Random.Range(5f, 8f);
                    else if (probFloat < 8f)
                        starScript.luminosity = Random.Range(8f, 12f);
                    else if (probFloat < 9.75f)
                        starScript.luminosity = Random.Range(12f, 17f);
                    else
                        starScript.luminosity = Random.Range(17f, 25f);
                    starScript.PrepStar();
                    starScript.ResizeStar(starCam.position, false);
                    star.LookAt(starCam);
                }
            }
        }
    }

    // Check to see if ray has hit one of the star bounds and if so return that stars transform
    // we should definetly optimize this to only check the sectors within the cameras view or the ones that the ray actually passes through only
    public bool RaycastToStar(Ray ray, out Transform star)
    {
        star = null;
        int index = -1;
        float min = float.MaxValue;
        float current = 0;
        for (int i = 0; i < gridSectorSizeM; i++)
        {
            for (int j = 0; j < gridSectorSizeM; j++)
            {
                for (int k = 0; k < gridSectorSizeM; k++)
                {
                    for (int b = 0; b < sectorStarBoundsM[i, j, k].Count; b++)
                    {
                        if (sectorStarBoundsM[i, j, k][b].IntersectRay(ray, out current) && current < min)
                        {
                            index = i;
                            min = current;
                            star = sectorStarsM[i, j, k][b];
                            star.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsM[i, j, k], b);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < gridSectorSizeK; i++)
        {
            for (int j = 0; j < gridSectorSizeK; j++)
            {
                for (int k = 0; k < gridSectorSizeK; k++)
                {
                    for (int b = 0; b < sectorStarBoundsK[i, j, k].Count; b++)
                    {
                        if (sectorStarBoundsK[i, j, k][b].IntersectRay(ray, out current) && current < min)
                        {
                            index = i;
                            min = current;
                            star = sectorStarsK[i, j, k][b];
                            star.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsK[i, j, k], b);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < gridSectorSizeG; i++)
        {
            for (int j = 0; j < gridSectorSizeG; j++)
            {
                for (int k = 0; k < gridSectorSizeG; k++)
                {
                    for (int b = 0; b < sectorStarBoundsG[i, j, k].Count; b++)
                    {
                        if (sectorStarBoundsG[i, j, k][b].IntersectRay(ray, out current) && current < min)
                        {
                            index = i;
                            min = current;
                            star = sectorStarsG[i, j, k][b];
                            star.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsG[i, j, k], b);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < gridSectorSizeF; i++)
        {
            for (int j = 0; j < gridSectorSizeF; j++)
            {
                for (int k = 0; k < gridSectorSizeF; k++)
                {
                    for (int b = 0; b < sectorStarBoundsF[i, j, k].Count; b++)
                    {
                        if (sectorStarBoundsF[i, j, k][b].IntersectRay(ray, out current) && current < min)
                        {
                            index = i;
                            min = current;
                            star = sectorStarsF[i, j, k][b];
                            star.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsF[i, j, k], b);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < gridSectorSizeA; i++)
        {
            for (int j = 0; j < gridSectorSizeA; j++)
            {
                for (int k = 0; k < gridSectorSizeA; k++)
                {
                    for (int b = 0; b < sectorStarBoundsA[i, j, k].Count; b++)
                    {
                        if (sectorStarBoundsA[i, j, k][b].IntersectRay(ray, out current) && current < min)
                        {
                            index = i;
                            min = current;
                            star = sectorStarsA[i, j, k][b];
                            star.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsA[i, j, k], b);
                        }
                    }
                }
            }
        }
        return index >= 0;
    }

    // Check how close stars in center grid location are to a position(most likely the star/galaxy camera) and return any star that is very close to that position.
    // 1582AU == 0.025LY
    public bool CloseStar(out GameObject closeStar, Vector3 pos)
    {
        closeStar = null;
        bool foundOne = false;
        for (int s = 0; s < sectorStarsM[gridCenterSectorM, gridCenterSectorM, gridCenterSectorM].Count; s++)
        {
            if (Vector3.Distance(sectorStarsM[gridCenterSectorM, gridCenterSectorM, gridCenterSectorM][s].position, pos) < 0.016f)
            {
                closeStar = sectorStarsM[gridCenterSectorM, gridCenterSectorM, gridCenterSectorM][s].gameObject;
                int gc = gridCenterSectorM;
                closeStar.transform.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsM[gc, gc, gc], s);
                foundOne = true;
                break;
            }
        }
        if (!foundOne)
        {
            for (int s = 0; s < sectorStarsK[gridCenterSectorK, gridCenterSectorK, gridCenterSectorK].Count; s++)
            {
                if (Vector3.Distance(sectorStarsK[gridCenterSectorK, gridCenterSectorK, gridCenterSectorK][s].position, pos) < 0.016f)
                {
                    closeStar = sectorStarsK[gridCenterSectorK, gridCenterSectorK, gridCenterSectorK][s].gameObject;
                    int gc = gridCenterSectorK;
                    closeStar.transform.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsK[gc, gc, gc], s);
                    foundOne = true;
                    break;
                }
            }
        }
        if (!foundOne)
        {
            for (int s = 0; s < sectorStarsG[gridCenterSectorG, gridCenterSectorG, gridCenterSectorG].Count; s++)
            {
                if (Vector3.Distance(sectorStarsG[gridCenterSectorG, gridCenterSectorG, gridCenterSectorG][s].position, pos) < 0.016f)
                {
                    closeStar = sectorStarsG[gridCenterSectorG, gridCenterSectorG, gridCenterSectorG][s].gameObject;
                    int gc = gridCenterSectorG;
                    closeStar.transform.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsG[gc, gc, gc], s);
                    foundOne = true;
                    break;
                }
            }
        }
        if (!foundOne)
        {
            for (int s = 0; s < sectorStarsF[gridCenterSectorF, gridCenterSectorF, gridCenterSectorF].Count; s++)
            {
                if (Vector3.Distance(sectorStarsF[gridCenterSectorF, gridCenterSectorF, gridCenterSectorF][s].position, pos) < 0.016f)
                {
                    closeStar = sectorStarsF[gridCenterSectorF, gridCenterSectorF, gridCenterSectorF][s].gameObject;
                    int gc = gridCenterSectorF;
                    closeStar.transform.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsF[gc, gc, gc], s);
                    foundOne = true;
                    break;
                }
            }
        }
        if (!foundOne)
        {
            for (int s = 0; s < sectorStarsA[gridCenterSectorA, gridCenterSectorA, gridCenterSectorA].Count; s++)
            {
                if (Vector3.Distance(sectorStarsA[gridCenterSectorA, gridCenterSectorA, gridCenterSectorA][s].position, pos) < 0.016f)
                {
                    closeStar = sectorStarsA[gridCenterSectorA, gridCenterSectorA, gridCenterSectorA][s].gameObject;
                    int gc = gridCenterSectorA;
                    closeStar.transform.GetComponent<Star>().GenerateInfo(seed, starSectorGridPositionsA[gc, gc, gc], s);
                    foundOne = true;
                    break;
                }
            }
        }
        return foundOne;
    }

    // As we move through space stars will be rotated and scaled properly along with thier bounds but more often as they get closer
    // If close only is true then we iterate through only stars within 3 blocks of center grid
    // THIS IS NOT OPTIMIZED TO AN ACCEPTABLE DEGREE!!! 
    private void AdjustAllStarSectors(bool closeOnly)
    {
        if (cToScale == 0)
        {
            cToScale = 1;
            // It doesn't matter if closeOnly is true or not for M Class stars they will always be updated for now
            for (int i = 0; i < gridSectorSizeM; i++)
            {
                for (int j = 0; j < gridSectorSizeM; j++)
                {
                    for (int k = 0; k < gridSectorSizeM; k++)
                    {
                        for (int s = 0; s < sectorStarsM[i, j, k].Count; s++)
                        {
                            float scaleB = sectorStarsM[i, j, k][s].GetComponent<Star>().ResizeStar(starCam.position, false);
                            sectorStarBoundsM[i, j, k][s] = new Bounds(sectorStarsM[i, j, k][s].position, new Vector3(scaleB, scaleB, scaleB));
                            sectorStarsM[i, j, k][s].LookAt(starCam);
                        }
                    }
                }
            }
        }
        else if (cToScale == 1)
        {
            cToScale = 2;
            // again close only doesn't matter for K at the momment
            for (int i = 0; i < gridSectorSizeK; i++)
            {
                for (int j = 0; j < gridSectorSizeK; j++)
                {
                    for (int k = 0; k < gridSectorSizeK; k++)
                    {
                        for (int s = 0; s < sectorStarsK[i, j, k].Count; s++)
                        {
                            float scaleB = sectorStarsK[i, j, k][s].GetComponent<Star>().ResizeStar(starCam.position, false);
                            sectorStarBoundsK[i, j, k][s] = new Bounds(sectorStarsK[i, j, k][s].position, new Vector3(scaleB, scaleB, scaleB));
                            sectorStarsK[i, j, k][s].LookAt(starCam);
                        }
                    }
                }
            }
        }
        else if (cToScale == 2)
        {
            cToScale = 3;
            // Now G stars are out of the close only range
            int startRange, endRange;
            if(closeOnly) {
                startRange = gridCenterSectorG - 2;
                endRange = gridCenterSectorG + 2;
            }
            else {
                startRange = 0;
                endRange = gridSectorSizeG - 1;
            }
            for (int i = startRange; i <= endRange; i++)
            {
                for (int j = startRange; j <= endRange; j++)
                {
                    for (int k = startRange; k <= endRange; k++)
                    {
                        for (int s = 0; s < sectorStarsG[i, j, k].Count; s++)
                        {
                            float scaleB = sectorStarsG[i, j, k][s].GetComponent<Star>().ResizeStar(starCam.position, false);
                            sectorStarBoundsG[i, j, k][s] = new Bounds(sectorStarsG[i, j, k][s].position, new Vector3(scaleB, scaleB, scaleB));
                            sectorStarsG[i, j, k][s].LookAt(starCam);
                        }
                    }
                }
            }
        }
        else if (cToScale == 3)
        {
            cToScale = 4;
            int startRange, endRange;
            if (closeOnly)
            {
                startRange = gridCenterSectorF - 3;
                endRange = gridCenterSectorF + 3;
            }
            else
            {
                startRange = 0;
                endRange = gridSectorSizeF - 1;
            }
            for (int i = startRange; i < endRange; i++)
            {
                for (int j = startRange; j < endRange; j++)
                {
                    for (int k = startRange; k < endRange; k++)
                    {
                        for (int s = 0; s < sectorStarsF[i, j, k].Count; s++)
                        {
                            float scaleB = sectorStarsF[i, j, k][s].GetComponent<Star>().ResizeStar(starCam.position, false);
                            sectorStarBoundsF[i, j, k][s] = new Bounds(sectorStarsF[i, j, k][s].position, new Vector3(scaleB, scaleB, scaleB));
                            sectorStarsF[i, j, k][s].LookAt(starCam);
                        }
                    }
                }
            }
        }
        else if (cToScale == 4)
        {
            cToScale = 0;
            int startRange, endRange;
            if (closeOnly)
            {
                startRange = gridCenterSectorA - 4;
                endRange = gridCenterSectorA + 4;
            }
            else
            {
                startRange = 0;
                endRange = gridSectorSizeA - 1;
            }
            for (int i = startRange; i < endRange; i++)
            {
                for (int j = startRange; j < endRange; j++)
                {
                    for (int k = startRange; k < endRange; k++)
                    {
                        for (int s = 0; s < sectorStarsA[i, j, k].Count; s++)
                        {
                            float scaleB = sectorStarsA[i, j, k][s].GetComponent<Star>().ResizeStar(starCam.position, false);
                            sectorStarBoundsA[i, j, k][s] = new Bounds(sectorStarsA[i, j, k][s].position, new Vector3(scaleB, scaleB, scaleB));
                            sectorStarsA[i, j, k][s].LookAt(starCam);
                        }
                    }
                }
            }
            if (!closeOnly)
                adjustedFarStars = true;
            else
                adjustedNearStars = true;
        }
    }
}