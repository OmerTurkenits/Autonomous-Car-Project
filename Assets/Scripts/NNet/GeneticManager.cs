using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MathNet.Numerics.LinearAlgebra;

public class GeneticManager : MonoBehaviour
{
    #region ======================   VARIABLES  =======================
    [Header("References")]
    public CarController controller;

    [Header("Controls")]
    public int initialPopulation = 85;
    [Range(0.0f, 1.0f)]
    public float mutationRate; //0.055f

    [Header("Crossover Controls")]
    public int bestAgentSelection = 8;
    public int worstAgentSelection = 3;
    public int numberToCrossover;

    private List<int> genePool = new List<int>();

    private int naturallySelected;

    private NNet[] population;

    [Header("Public View")]
    public int currentGeneration;
    public int currentGenome = 0;

    public TextMeshProUGUI generationText;
    public TextMeshProUGUI genomeText;

    public Toggle preMade;
    #endregion

    /// <summary>
    /// A function that runs at the programs' start.
    /// </summary> 
    private void Awake()
    {
        mutationRate = NNetValues.MutationRate;
        
    }

    /// <summary>
    /// A function that called before the first frame update
    /// </summary>
    private void Start()
    {
        CreatePopulation();
    }

    /// <summary>
    /// A function that creates a new network population.
    /// </summary>
    private void CreatePopulation()
    {
        population = new NNet[initialPopulation];
        FillPopulationWithRandomValues(population, 0);
        ResetToCurrentGenome();
    }

    /// <summary>
    /// A function that sends the current network on the poulation or the pre saved network to the car controller.
    /// </summary>
    public void ResetToCurrentGenome()
    {
      
        if (preMade.isOn)
        {
            
            controller.ResetWithNetwork(SaveSystem.network);
            
        }
        else
        {
            
            Debug.Log("ResetToCurrentGenome(): "+population[currentGenome].biases);
            controller.ResetWithNetwork(population[currentGenome]);
            
        }
        

    }

    /// <summary>
    /// A function that gets the population and fills the networks with random values.
    /// </summary>
    /// <param name="newPopulation"></param>
    /// <param name="startingIndex"></param>
    private void FillPopulationWithRandomValues(NNet[] newPopulation, int startingIndex)
    {
        while (startingIndex < initialPopulation)
        {
            newPopulation[startingIndex] = new NNet();
            newPopulation[startingIndex].Initialise(controller.LAYERS, controller.NEURONS);
            startingIndex++;
        }
    }

    /// <summary>
    /// A function that "kills" the neural network and saves its fitness.
    /// </summary>
    /// <param name="fitness"> The networks' fitness </param>
    /// <param name="network"> The network object</param>
    public void Death(float fitness, NNet network)
    {

        if (currentGenome < population.Length - 1)
        {

            population[currentGenome].fitness = fitness;
            currentGenome++;
            genomeText.text = "Genome: " + currentGenome;

            ResetToCurrentGenome();

        }
        else
        {
            RePopulate();
        }

    }

    /// <summary>
    /// A function that ends a populations' process. (sorts the networks, picks the best, mutates...)
    /// </summary>
    private void RePopulate()
    {
        genePool.Clear();
        currentGeneration++;
        generationText.text = "Generation: "+currentGeneration;

        naturallySelected = 0;
        SortPopulation();

        NNet[] newPopulation = PickBestPopulation();

        Crossover(newPopulation);
        Mutate(newPopulation);

        FillPopulationWithRandomValues(newPopulation, naturallySelected);

        population = newPopulation;

        currentGenome = 0;

        ResetToCurrentGenome();

    }

    /// <summary>
    /// A function that sends a weight matrix of a certain layer to mutation.
    /// </summary>
    /// <param name="newPopulation"> The newly created network population </param>
    private void Mutate(NNet[] newPopulation)
    {

        for (int i = 0; i < naturallySelected; i++)
        {

            for (int c = 0; c < newPopulation[i].weights.Count; c++)
            {

                if (Random.Range(0.0f, 1.0f) < mutationRate)
                {
                    newPopulation[i].weights[c] = MutateMatrix(newPopulation[i].weights[c]);
                }

            }

        }

    }

    /// <summary>
    /// A function that get a weight matrix and mutates it.
    /// </summary>
    /// <param name="A"> a weight matrix </param>
    /// <returns></returns>
    Matrix<float> MutateMatrix(Matrix<float> A)
    {

        int randomPoints = Random.Range(1, (A.RowCount * A.ColumnCount) / 7);

        Matrix<float> C = A;

        for (int i = 0; i < randomPoints; i++)
        {
            int randomColumn = Random.Range(0, C.ColumnCount);
            int randomRow = Random.Range(0, C.RowCount);

            C[randomRow, randomColumn] = Mathf.Clamp(C[randomRow, randomColumn] + Random.Range(-1f, 1f), -1f, 1f);
        }

        return C;

    }

    /// <summary>
    /// A function that crosses over 2 parents and output new children.
    /// </summary>
    /// <param name="newPopulation"> The population </param>
    private void Crossover(NNet[] newPopulation)
    {
        for (int i = 0; i < numberToCrossover; i += 2)
        {
            int AIndex = i;
            int BIndex = i + 1;

            if (genePool.Count >= 1)
            {
                for (int l = 0; l < 100; l++)
                {
                    AIndex = genePool[Random.Range(0, genePool.Count)]; //A random network in the gene pool
                    BIndex = genePool[Random.Range(0, genePool.Count)];

                    if (AIndex != BIndex) //If the indexes are different:
                        break;
                }
            }

            NNet Child1 = new NNet();
            NNet Child2 = new NNet();

            Child1.Initialise(controller.LAYERS, controller.NEURONS); //Create the new children
            Child2.Initialise(controller.LAYERS, controller.NEURONS);

            Child1.fitness = 0;
            Child2.fitness = 0;


            for (int w = 0; w < Child1.weights.Count; w++)
            {

                if (Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    Child1.weights[w] = population[AIndex].weights[w];
                    Child2.weights[w] = population[BIndex].weights[w];
                }
                else
                {
                    Child2.weights[w] = population[AIndex].weights[w];
                    Child1.weights[w] = population[BIndex].weights[w];
                }

            }


            for (int w = 0; w < Child1.biases.Count; w++)
            {

                if (Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    Child1.biases[w] = population[AIndex].biases[w];
                    Child2.biases[w] = population[BIndex].biases[w];
                }
                else
                {
                    Child2.biases[w] = population[AIndex].biases[w];
                    Child1.biases[w] = population[BIndex].biases[w];
                }

            }

            newPopulation[naturallySelected] = Child1;
            naturallySelected++;

            newPopulation[naturallySelected] = Child2;
            naturallySelected++;

        }
    }

    /// <summary>
    /// A function that picks the best networks that advance and sets the gene pool.
    /// </summary>
    /// <returns></returns>
    private NNet[] PickBestPopulation()
    {

        NNet[] newPopulation = new NNet[initialPopulation];

        for (int i = 0; i < bestAgentSelection; i++)
        {
            newPopulation[naturallySelected] = population[i].InitialiseCopy(controller.LAYERS, controller.NEURONS);
            newPopulation[naturallySelected].fitness = 0;
            naturallySelected++;

            int f = Mathf.RoundToInt(population[i].fitness * 10); 

            for (int c = 0; c < f; c++)
            {
                genePool.Add(i);
            }

        }

        for (int i = 0; i < worstAgentSelection; i++)
        {
            int last = population.Length - 1;
            last -= i;

            int f = Mathf.RoundToInt(population[last].fitness * 10);

            for (int c = 0; c < f; c++)
            {
                genePool.Add(last);
            }

        }

        return newPopulation;

    }

    /// <summary>
    /// A function that bubble sorts the network population by their fitness score.
    /// </summary>
    private void SortPopulation()
    {
        for (int i = 0; i < population.Length; i++)
        {
            for (int j = i; j < population.Length; j++)
            {
                if (population[i].fitness < population[j].fitness)
                {
                    NNet temp = population[i];
                    population[i] = population[j];
                    population[j] = temp;
                }
            }
        }

    }
}
