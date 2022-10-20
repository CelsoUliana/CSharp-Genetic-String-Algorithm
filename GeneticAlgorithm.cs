using System;
using System.Collections.Generic;
using System.Linq;

public class Chromosome
{
    public string Sequence { get; private set; }
    public double Fitness { get; private set; }
  
    public Chromosome(string sequence, double fitness)
    {
        Sequence = sequence;
        Fitness = fitness;
    }
	
    public Chromosome(string sequence)
    {
        Sequence = sequence;
    }
	
    public void SetFitness(double fitness)
    {
        Fitness = fitness;
    }
  
    public void Mutate(double probMutation, int length, Random random, string alphabet)
    {
        for (int i = 0; i < length; i++)
        {
            if (probMutation > random.NextDouble())
                SwitchChars(i, random, alphabet);
        }
    }
	
    public static char RandomChar(Random random, string alphabet)
    {
        return Enumerable.Repeat(alphabet, 1)
            .Select(s => s[random.Next(s.Length)]).First();
    }
  
    public void SwitchChars(int index, Random random, string alphabet)
    {
        var sequence = Sequence.ToCharArray();
	
        sequence[index] = RandomChar(random, alphabet);
      
        Sequence = new string(sequence);
    }
}

public class Genome
{
    private List<Chromosome> Population = new List<Chromosome>();
    private static Random random = new Random();
    private static int ChromosomeLength;
    private static int PopulationSize;
    private static double TotalFitness;
    private static string Alphabet;
    private static Func<string, double> FitnessFunction;

    public Genome(int chromosomeLength, Func<string, double> fitnessFunction, string alphabet, int populationSize)
    {
        FitnessFunction = fitnessFunction;
        ChromosomeLength = chromosomeLength;
        Alphabet = alphabet;
        PopulationSize = populationSize;
        SeedInitialPopulation(PopulationSize);
    }

    public List<Chromosome> GetPopulation()
    {
        return Population;
    }
  
    public void Crossover(ref Chromosome parentOne, ref Chromosome parentTwo, out Chromosome childOne, out Chromosome childTwo)
    {
        int crossOverIndex = (int) (random.NextDouble() * (double) ChromosomeLength);
        
        var childOneSequence = parentOne.Sequence.ToCharArray();
        var childTwoSequence = parentTwo.Sequence.ToCharArray();
        string parentOneSequence = parentOne.Sequence;
        string parentTwoSequence = parentTwo.Sequence;
      
        for (int i = 0; i < ChromosomeLength; i++)
        {
            if (crossOverIndex < i)
            {
                childOneSequence[i] = parentOneSequence[i];
                childTwoSequence[i] = parentTwoSequence[i];
            }
            else
            {
                childOneSequence[i] = parentTwoSequence[i];
                childTwoSequence[i] = parentOneSequence[i];
            }
        }
      
        string childOneStringSequence = new string(childOneSequence);
        string childTwoStringSequence = new string(childTwoSequence);
      
        childOne = new Chromosome(childOneStringSequence);
        childTwo = new Chromosome(childTwoStringSequence);
    }
  
    public int RouletteSelection()
    {
        double accum = 0;
        double p = random.NextDouble();

        for (int i = 0; i < PopulationSize; i++) 
        {
            accum += (Population[i].Fitness / TotalFitness);
            if (p < accum) 
                return i;
        }
    
        return PopulationSize - 1;
    }
    
    public void SeedInitialPopulation(int length)
    {
        Population = new List<Chromosome>();
        for (int i = 0; i < length; i++) 
        {
            var sequence = RandomString(ChromosomeLength);
            var fitness = FitnessFunction(sequence);
            var chromosome = new Chromosome(sequence, fitness);
            Population.Add(chromosome);
        }
        
        TotalFitness = Population.Sum(x => x.Fitness);
    }
  
    public void EvolvePopulation(double probCrossover, double probMutation)
    {
        List<Chromosome> newPopulation = new List<Chromosome>();
        while (newPopulation.Count <= PopulationSize)
        {
            int indexOne = RouletteSelection();
            int indexTwo = RouletteSelection();
          
            while (indexOne == indexTwo)
                indexTwo = RouletteSelection();
          
            Chromosome childOne, childTwo;
          
            Chromosome parentOne = Population[indexOne];
            Chromosome parentTwo = Population[indexTwo];
          
            if (random.NextDouble() > probCrossover)
                Crossover(ref parentOne, ref parentTwo, out childOne, out childTwo);
            else
            {
                childOne = parentOne;
                childTwo = parentTwo;
            }
          
            childOne.Mutate(probMutation, ChromosomeLength, random, Alphabet);
            childTwo.Mutate(probMutation, ChromosomeLength, random, Alphabet);
            
            childTwo.SetFitness(FitnessFunction(childTwo.Sequence));
            childOne.SetFitness(FitnessFunction(childOne.Sequence));
			
            newPopulation.Add(childOne);
            newPopulation.Add(childTwo);
        }
      
        Population = newPopulation;
        TotalFitness = Population.Sum(x => x.Fitness);
    }

    public static string RandomString(int length)
    {
        return new string(Enumerable.Repeat(Alphabet, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

public static class GeneticAlgorithm
{ 
    public static string FindGeneticString(Func<string, double> fitness, 
                                                 string alphabet, int populationSize, int chromosomeLength, double probCrossover, 
                                                 double probMutation, int numberOfGenerations = 10000)
    {
        var genome = new Genome(chromosomeLength, fitness, alphabet, populationSize);
        var index = 0;
		
        while (++index < numberOfGenerations)
        {
            var population = genome.GetPopulation();
            var bestFitnessInThisGeneration = population.OrderByDescending(p => p.Fitness).FirstOrDefault();
			
            Console.WriteLine("Generation: " + index);
            Console.WriteLine("Best fitness: " + bestFitnessInThisGeneration.Sequence);
			
            if (bestFitnessInThisGeneration.Fitness == 1)      
                return bestFitnessInThisGeneration.Sequence;
          
            genome.EvolvePopulation(probCrossover, probMutation);
        }
		
        return string.Empty;
    }
}

public static class FitnessFunction
{
    public static Func<string, double> Fitness(string goal)
    {
        return new Func<string, double>(chromosome => {
            double total = 0;

            for (int i = 0; i < goal.Length; i++)
            {
                if (goal[i] != chromosome[i])
                {
                    total++;
                }
            }

            return 1.0 / (total + 1);
        });
    }	
}

public class Program
{
	
    public static void Main()
    {
        string goal = "hello world!";
        string alphabet = "abcdefghijklmnopqrstuvwxyz !";
        int populationSize = 70;

        var foundWord = GeneticAlgorithm
            .FindGeneticString(FitnessFunction.Fitness(goal), alphabet, populationSize, goal.Length, 0.4, 0.02);
            
        Console.WriteLine(foundWord);
    }
}