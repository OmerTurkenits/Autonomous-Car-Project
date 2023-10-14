using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(NNet))]
public class CarController : MonoBehaviour
{
    #region ======================   VARIABLES  =======================
    private Vector3 startPosition, startRotation;
    public NNet network;

    [Range(-1f, 1f)]
    public float a, t;

    public float timeSinceStart = 0f;

    [Header("Fitness")]
    public float overallFitness;
    public float distanceMultiplier;
    public float avgSpeedMultiplier;
    public float sensorMultiplier;

    [Header("Network Options")]
    public int LAYERS; 
    public int NEURONS; 

    private Vector3 lastPosition;
    private float totalDistanceTravelled;
    private float avgSpeed;

    private float aSensor, bSensor, cSensor;

    private float maxScore = 0;
    public TextMeshProUGUI maxScoreText;

    private bool isSaved = false;

    public TextMeshProUGUI SuccessText;
    public GameObject genomeManager;
    #endregion

    /// <summary>
    /// A function that runs at the programs' start.
    /// </summary>
    private void Awake()
    {
        distanceMultiplier = NNetValues.DistanceMultiplier; //1.4f;
        avgSpeedMultiplier =  NNetValues.AverageSpeedMultiplier; //0.2f;
        sensorMultiplier = NNetValues.SensorMultiplier; //0.1f;

        LAYERS =  NNetValues.HiddenLayerCount; //1;
        NEURONS =  NNetValues.HiddenNeuronCount; //10;

        Debug.Log(distanceMultiplier);

        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        
        network = GetComponent<NNet>();

    }

    /// <summary>
    /// A function that sets a new neural network to the car and resets it.
    /// </summary>
    /// <param name="net"> A new neural network </param>
    public void ResetWithNetwork(NNet net)
    {
        network = net;
        Reset();
    }


    /// <summary>
    /// A function that resets the cars parameters, setting it up for a new network
    /// </summary>
    public void Reset()
    {

        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        avgSpeed = 0f;
        lastPosition = startPosition;
        overallFitness = 0f;
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
    }

    /// <summary>
    /// A function that detects if the car hit a wall object.
    /// </summary>
    /// <param name="collision"> A collision object (Unity) </param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            Death();
        }
    }

    /// <summary>
    /// A function that occurreds once every frame.
    /// </summary>
    private void FixedUpdate()
    {
        //if the car is ready to run
        if (genomeManager.active)
        {
            InputSensors();
            lastPosition = transform.position;

            (a, t) = network.RunNetwork(aSensor, bSensor, cSensor);

            MoveCar(a, t);

            timeSinceStart += Time.deltaTime;
            
            CalculateFitness();
        }
    }

    /// <summary>
    /// A function that ends the current networks' process.
    /// </summary>
    private void Death()
    {
        GameObject.FindObjectOfType<GeneticManager>().Death(overallFitness, network);
    }

    /// <summary>
    /// A function that calculates the networks' current fitness
    /// </summary>
    private void CalculateFitness()
    {

        totalDistanceTravelled += Vector3.Distance(transform.position, lastPosition);
        avgSpeed = totalDistanceTravelled / timeSinceStart;

        overallFitness = (totalDistanceTravelled * distanceMultiplier) + (avgSpeed * avgSpeedMultiplier) + (((aSensor + bSensor + cSensor) / 3) * sensorMultiplier);

        maxScore = Math.Max(maxScore, overallFitness);

        maxScoreText.text = "Best Score: " + (int)maxScore;

        if (timeSinceStart > 20 && overallFitness < 40)
        {
            Death();
        }

        if (overallFitness >= 3500 && !isSaved)
        {
            SuccessText.gameObject.SetActive(true);
            SaveSystem.network = network;
            SaveSystem.isNetwork = true;

            isSaved = true;
        }

    }

    /// <summary>
    /// A function that gets the data from the 3 rays (the inputs)
    /// </summary>
    private void InputSensors()
    {

        Vector3 a = (transform.forward + transform.right);
        Vector3 b = (transform.forward);
        Vector3 c = (transform.forward - transform.right);

        Ray r = new Ray(new Vector3(transform.position.x, transform.position.y+5,transform.position.z), a);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit))
        {
            aSensor = hit.distance / 20;
            Debug.DrawLine(r.origin, hit.point, Color.blue);
        }

        r.direction = b;

        if (Physics.Raycast(r, out hit))
        {
            bSensor = hit.distance / 20;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

        r.direction = c;

        if (Physics.Raycast(r, out hit))
        {
            cSensor = hit.distance / 20;
            Debug.DrawLine(r.origin, hit.point, Color.green);
        }

    }

    private Vector3 inp;

    /// <summary>
    /// A function that moves the car according to the neural network outputs
    /// </summary>
    /// <param name="v"> The cars' acceleration (an output of the neural network) </param>
    /// <param name="h"> The cars' turn radius (an output of the neural network) </param>
    public void MoveCar(float v, float h)
    {
        inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, v * 11.4f), 0.02f);
        inp = transform.TransformDirection(inp);
        transform.position += inp;

        transform.eulerAngles += new Vector3(0, (h * 90) * 0.02f, 0);
    }

}
