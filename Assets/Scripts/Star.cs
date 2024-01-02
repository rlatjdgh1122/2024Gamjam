using UnityEngine;

public class Star : MonoBehaviour
{
    public string starName, spectralClass;
    public float radius, luminosity;
    public int temperature, numberOfPlanets;
    public bool hasLife;
    public Material mat;

    public void PrepStar()
    {
        if (spectralClass[0] == 'M')
        {
            mat.color = new Color(0.9f, 0.44f, 0.32f);
        }
        else if (spectralClass[0] == 'K')
        {
            mat.color = new Color(0.85f, 0.73f, 0.41f);
        }
        else if (spectralClass[0] == 'G')
        {
            mat.color = new Color(0.9f, 0.9f, 0.68f);
        }
        else if (spectralClass[0] == 'F')
        {
            mat.color = new Color(0.99f, 0.99f, 0.99f);
        }
        else if (spectralClass[0] == 'A')
        {
            mat.color = new Color(0.8f, 0.8f, 0.99f);
        }
    }

    public float ResizeStar(Vector3 camPos, bool printBright)
    {
        float dist = Vector3.Distance(transform.position, camPos);
        float bright = (luminosity / (4f * 3.14f * (dist * dist))) * 190f;
        if (bright > 0.005f) // As stars get brighter they should gradually increase in size until they are very close but not get too big
        {
            if (bright > 0.01)
            {
                bright = 0.01f + ((bright - 0.01f) / 4f);
                if (bright > 0.02)
                {
                    bright = 0.02f + ((bright - 0.02f) / 8f);
                    if (bright > 0.03f)
                    {
                        bright = 0.03f + ((bright - 0.03f) / 16f);
                        if (bright > 0.04f)
                        {
                            bright = 0.04f + ((bright - 0.04f) / 4f);
                            if (bright > 0.1f) bright = 0.1f;
                        }
                    }
                }
            }
        }
        else // make faint stars fade out
        {
            float a = (bright / 0.005f);
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, a);
            if (bright > 0.005f) bright = 0.005f;
        }
        if (printBright) print(starName + " Brightness: " + bright);
        float scale = dist * bright;
        transform.localScale = new Vector3(scale, scale, scale);
        return scale;
    }

    public void GenerateInfo(int seed, Vector3Int loc, int starID)
    {
        Random.InitState(seed + spectralClass[0] + loc.x + loc.y + loc.z + starID);
        starName = GenerateStarName();
        numberOfPlanets = Random.Range(0, 22);

        if(spectralClass[0] == 'M')
        {
            float luminRangePercent = (luminosity - 0.001f) / (0.08f - 0.001f); // from 0f to 1f
            int tempTemp = (int)(((3700 - 2400) * luminRangePercent) + 2400);
            int tempMinTemp = tempTemp - 100;
            if (tempMinTemp < 2400) tempMinTemp = 2400;
            int tempMaxTemp = tempTemp + 100;
            if (tempMaxTemp > 3700) tempMaxTemp = 3700;
            temperature = Random.Range(tempMinTemp, tempMaxTemp);
            spectralClass = "M" + (9 - ((int)((temperature - 2400) / ((3701 - 2400) / 10)))) + "V";
        }
        else if (spectralClass[0] == 'K')
        {
            float luminRangePercent = (luminosity - 0.08f) / (0.6f - 0.08f); // from 0f to 1f
            int tempTemp = (int)(((5200 - 3701) * luminRangePercent) + 3701);
            int tempMinTemp = tempTemp - 150;
            if (tempMinTemp < 3701) tempMinTemp = 3701;
            int tempMaxTemp = tempTemp + 150;
            if (tempMaxTemp > 5200) tempMaxTemp = 5200;
            temperature = Random.Range(tempMinTemp, tempMaxTemp);
            spectralClass = "K" + (9 - ((int)((temperature - 3701) / ((5201 - 3701) / 10)))) + "V";
        }
        else if (spectralClass[0] == 'G')
        {
            float luminRangePercent = (luminosity - 0.6f) / (1.5f - 0.6f); // from 0f to 1f
            int tempTemp = (int)(((6000 - 5201) * luminRangePercent) + 5201);
            int tempMinTemp = tempTemp - 200;
            if (tempMinTemp < 5201) tempMinTemp = 5201;
            int tempMaxTemp = tempTemp + 200;
            if (tempMaxTemp > 6000) tempMaxTemp = 6000;
            temperature = Random.Range(tempMinTemp, tempMaxTemp);
            spectralClass = "G" + (9 - ((int)((temperature - 5201) / ((6001 - 5201) / 10)))) + "V";
        }
        else if (spectralClass[0] == 'F')
        {
            float luminRangePercent = (luminosity - 1.5f) / (5f - 1.5f); // from 0f to 1f
            int tempTemp = (int)(((7500- 6001) * luminRangePercent) + 6001);
            int tempMinTemp = tempTemp - 250;
            if (tempMinTemp < 6001) tempMinTemp = 6001;
            int tempMaxTemp = tempTemp + 250;
            if (tempMaxTemp > 7500) tempMaxTemp = 7500;
            temperature = Random.Range(tempMinTemp, tempMaxTemp);
            spectralClass = "F" + (9 - ((int)((temperature - 6001) / ((7501 - 6001) / 10)))) + "V";
        }
        else if (spectralClass[0] == 'A')
        {
            float luminRangePercent = (luminosity - 5f) / (25f - 5f); // from 0f to 1f
            int tempTemp = (int)(((10000 - 7501) * luminRangePercent) + 7501);
            int tempMinTemp = tempTemp - 300;
            if (tempMinTemp < 7501) tempMinTemp = 7501;
            int tempMaxTemp = tempTemp + 300;
            if (tempMaxTemp > 10000) tempMaxTemp = 10000;
            temperature = Random.Range(tempMinTemp, tempMaxTemp);
            spectralClass = "A" + (9 - ((int)((temperature - 7501) / ((10001 - 7501) / 10)))) + "V";
        }
        radius = Mathf.Pow(5772 / (float)temperature, 2) * Mathf.Pow(luminosity / 1f, 0.5f);

        hasLife = false;
        if (numberOfPlanets > 0)
        {
            float oddsOfLife = Random.Range(0.0f, 100.0f);
            if (oddsOfLife >= 99.1f) hasLife = true;
        }

    }

    private string GenerateStarName()
    {
        string[] firstNames = new string[]
        {
            "Alpha", "Beta", "Omega", "Proxima", "New", "Gamma", "Stella", "", ""
        };

        string[] middleNames = new string[]
        {
            "Centauri", "Vega", "Cyrus", "Pandora", "Celeste", "Orion", "Dawn", "Aurora", "Portia", "Andromeda", "Aquila", "Aries", "Chamaeleon", "Draco", "Gemini", "Hydra", "Hercules", "Leo", "Lynx", "Pegasus", "Phoenix", "Pisces", "Scorpius", "Ursa", "Vela", "Wolf"
        };

        string[] lastNames = new string[]
        {
            "Minor", "Major", "Nova", "Delta", "Theta", "Lambda", "", "", "", ""
        };

        string generatedStarName;
        int firstNameRandIndex = Random.Range(0, firstNames.GetLength(0));
        generatedStarName = firstNames[firstNameRandIndex];
        if (firstNames[firstNameRandIndex] != "")
            generatedStarName += " ";
        int middleNameRandIndex = Random.Range(0, middleNames.GetLength(0));
        generatedStarName += " " + middleNames[middleNameRandIndex];
        int lastNameRandIndex = Random.Range(0, lastNames.GetLength(0));
        if (lastNames[lastNameRandIndex] != "")
            generatedStarName += " " + lastNames[lastNameRandIndex];

        return generatedStarName;
    }
}