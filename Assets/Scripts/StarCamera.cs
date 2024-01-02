using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarCamera : MonoBehaviour
{
    public float rotationSpeed = 3.0f; // Speed the camera will rotate around the rig
    public float moveSpeed = 15.0f;

    public GameObject target;
    public Vector3 lastStarPos;

    private StarGeneration starGener;
    private TravelManager travelManager;

    private GameObject solarCam;

    private void Start()
    {
        starGener = GameObject.FindObjectOfType<StarGeneration>();
        travelManager = GameObject.FindObjectOfType<TravelManager>();
        //transform.position = new Vector3(transform.position.x, transform.position.y, -starGener.galaxy.radiusH / 2f);
        //solarCam = GameObject.FindObjectOfType<SolarSystemCamera>().gameObject;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

        if (travelManager.travelMode == TravelManager.TravelMode.Stellar)
        {
            //GameObject.Find("SpeedMax").GetComponent<Text>().text = moveSpeed.ToString("F2") + " ly/sec";

            // Do something with key input
            // move camera
            if (Input.GetAxis("Vertical") != 0f)
            {
                if (Input.GetAxis("Horizontal") != 0f)
                    transform.Translate(((Vector3.forward * Input.GetAxis("Vertical")) + (Vector3.right * Input.GetAxis("Horizontal"))).normalized * Time.deltaTime * moveSpeed);
                else
                    transform.Translate((Vector3.forward * Input.GetAxis("Vertical")) * Time.deltaTime * moveSpeed);
            }
            else if (Input.GetAxis("Horizontal") != 0f)
                transform.Translate((Vector3.right * Input.GetAxis("Horizontal")) * Time.deltaTime * moveSpeed);

            //if (Input.GetAxis("Jump") != 0f)
            //    transform.Translate(Vector3.up * Time.deltaTime * (moveSpeed * Input.GetAxis("Jump")));

            // rotate camera while middle mouse click
            //if (Input.GetKey(KeyCode.Mouse2))
            //    transform.Rotate(-rotationSpeed * Input.GetAxis("Mouse Y"), rotationSpeed * Input.GetAxis("Mouse X"), 0.0f);
            //else if ((Input.GetAxis("Pitch") != 0f || Input.GetAxis("Yaw") != 0f) && !Input.GetKey(KeyCode.Joystick1Button9))
            //{
            //    transform.Rotate(((rotationSpeed * 360f) * Input.GetAxis("Pitch")) * Time.deltaTime, ((rotationSpeed * 360f) * Input.GetAxis("Yaw")) * Time.deltaTime, 0.0f);
            //}

            //if (Input.GetAxis("Roll") != 0)
            //    transform.Rotate(0f, 0f, -Input.GetAxis("Roll") * 45 * Time.deltaTime);

            //if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.JoystickButton0))
            //{
            //    // Used to detect if and what star the mouse has clicked on
            //    Transform hitTarget = null;
            //    Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            //    bool hasHit = GameObject.FindObjectOfType<StarGeneration>().RaycastToStar(ray, out hitTarget);
            //    if (hasHit)
            //    {
            //        target = hitTarget.gameObject;
            //        Star targStar = target.transform.GetComponent<Star>();
            //        targStar.ResizeStar(transform.position, true);
            //        GameObject.Find("StarNameLabel").GetComponent<Text>().text = "Star";
            //        GameObject.Find("StarPlanetsLabel").GetComponent<Text>().text = "Planets";
            //        GameObject.Find("StarName").GetComponent<Text>().text = targStar.starName;
            //        GameObject.Find("StarTypeLabel").GetComponent<Text>().text = "Classification";
            //        GameObject.Find("StarType").GetComponent<Text>().text = targStar.spectralClass;
            //        GameObject.Find("StarLuminosity").GetComponent<Text>().text = targStar.luminosity.ToString("F5") + "L☉";
            //        GameObject.Find("StarRadius").GetComponent<Text>().text = targStar.radius.ToString("F3") + "R☉";
            //        GameObject.Find("StarTemperature").GetComponent<Text>().text = targStar.temperature + "K";
            //        GameObject.Find("StarPlanets").GetComponent<Text>().text = targStar.numberOfPlanets + "";
            //        lastStarPos = target.transform.position;
            //        GameObject.FindObjectOfType<TargetTracker>().target = lastStarPos;
            //        if (targStar.hasLife)
            //            GameObject.Find("StarPlanets").GetComponent<Text>().text += "  Life Detected!";
            //    }
            //}
            //if (GameObject.Find("StarNameLabel").GetComponent<Text>().text == "Galaxy")
            //{
            //    if (GameObject.Find("StarName").GetComponent<Text>().text != "The Joshy Way" || GameObject.Find("StarPlanetsLabel").GetComponent<Text>().text != "Stars")
            //    {
            //        GameObject.Find("StarNameLabel").GetComponent<Text>().text = "Galaxy";
            //        GameObject.Find("StarName").GetComponent<Text>().text = "The Joshy Way";
            //        GameObject.Find("StarTypeLabel").GetComponent<Text>().text = "Type";
            //        GameObject.Find("StarType").GetComponent<Text>().text = "Spiral";
            //        GameObject.Find("StarLuminosity").GetComponent<Text>().text = "10,000";
            //        GameObject.Find("StarRadius").GetComponent<Text>().text = starGener.galaxy.radiusH + "ly";
            //        GameObject.Find("StarTemperature").GetComponent<Text>().text = "n/a";
            //        GameObject.Find("StarPlanetsLabel").GetComponent<Text>().text = "Stars";
            //        GameObject.Find("StarPlanets").GetComponent<Text>().text = starGener.galaxy.totalStars.ToString("N0");
            //    }
            //}

            //// output distance in light years the camera is from the target
            //if (GameObject.Find("StarName").GetComponent<Text>().text != "")
            //    GameObject.Find("StarDistance").GetComponent<Text>().text = Vector3.Distance(transform.position, lastStarPos).ToString("F2") + " ly";

            //if (Input.GetKeyUp(KeyCode.Mouse1))
            //{
            //    TargetTracker targTracker = GameObject.FindObjectOfType<TargetTracker>();
            //    if (targTracker.tracking)
            //        targTracker.tracking = false;
            //    else
            //        targTracker.tracking = true;
            //}

            //if (Input.GetKeyUp(KeyCode.G))
            //{
            //    GameObject.Find("StarNameLabel").GetComponent<Text>().text = "Galaxy";
            //    lastStarPos = new Vector3(0, 0, 0);
            //    GameObject.FindObjectOfType<TargetTracker>().target = lastStarPos;
            //}
        }
        else if(travelManager.travelMode == TravelManager.TravelMode.Solar)
        {
            transform.rotation = solarCam.transform.rotation;
            transform.position = (((solarCam.transform.position * 0.1f) / 632.4f) * 0.01f) + target.transform.position;
        }
    }

    void LateUpdate()
    {
        if (travelManager.travelMode == TravelManager.TravelMode.Stellar)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                moveSpeed += Input.GetAxis("Mouse ScrollWheel") * 10f;
                if (moveSpeed > 175f) moveSpeed = 175f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                moveSpeed += Input.GetAxis("Mouse ScrollWheel") * 10f;
                if (moveSpeed < 0.05f) moveSpeed = 0.05f;
            }
            else if (Input.GetKey(KeyCode.Joystick1Button9))
            {
                if ((Input.GetAxis("Pitch") * -1f) > 0f)
                {
                    moveSpeed += (Input.GetAxis("Pitch") * -1f) * 10f;
                    if (moveSpeed > 175f) moveSpeed = 175f;
                }
                else if ((Input.GetAxis("Pitch") * -1f) < 0f)
                {
                    moveSpeed += (Input.GetAxis("Pitch") * -1f) * 10f;
                    if (moveSpeed < 0.05f) moveSpeed = 0.05f;
                }
            }

            // If we are close to a star then enter its solar system
            GameObject closeStar;
            bool isCloseStar = starGener.CloseStar(out closeStar, transform.position);
            if (isCloseStar)
            {
                travelManager.travelMode = TravelManager.TravelMode.Solar;
                target = closeStar;
                solarCam.transform.rotation = transform.rotation;
                solarCam.transform.position = ((target.transform.position - transform.position) * 632400f) * -1f;
                solarCam.GetComponent<SolarSystemCamera>().moveSpeed = 750f;
                GameObject.FindObjectOfType<SolarSystem>().InitSolarSystem(target.transform.GetComponent<Star>());
                GameObject.FindObjectOfType<TargetTracker>().cam = solarCam.transform.GetComponent<Camera>();
                GameObject.FindObjectOfType<TargetTracker>().target = new Vector3(0f,0f,0f);
                print("Star: " + closeStar.transform.GetComponent<Star>().starName + " is within 0.01 ly of star camera.");
            }
        }
    }
}