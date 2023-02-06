using System;
using System.Linq;

namespace MemeticApp
{
    class Program
    {
        static int[] weights = {2,3,5,7,1,4,1 };
        static int[] values = { 10, 5, 15, 7, 6, 18, 3 };
        static int capacity = 15;

        static int populationSize = 20;
        static double crossoverProbability = 0.7;
        static double mutationProbability = 0.1;
        static int tournamentSize = 2;
        static int maxIterations = 100;

        static int[][] population;
        static int[] fitness;

        static int[] bestSolution;
        static int bestFitness;

        static Random rand = new Random();

        static void Main()
        {
            population = InitializePopulation();
            fitness = new int[populationSize];

            for (int i = 0; i < 100; i++)
            {
                EvaluateFitness();

                int[] parent1 = TournamentSelection();
                int[] parent2 = TournamentSelection();

                int[] child = Crossover(parent1, parent2);
                Mutate(child);

                ReplaceWorst(child);
                LocalSearch(child);
                EvaluateFitness();

                int bestIndex = Array.IndexOf(fitness, fitness.Max());
                if (fitness[bestIndex] > bestFitness)
                {
                    bestSolution = (int[])population[bestIndex].Clone();
                    bestFitness = fitness[bestIndex];
                }
            }

            Console.WriteLine("Best solution: " + string.Join(", ", bestSolution));
            Console.WriteLine("Value: " + bestFitness);
        }

        static int[][] InitializePopulation()
        {
            return Enumerable.Range(0, populationSize)
                             .Select(i => Enumerable.Range(0, weights.Length)
                                                   .Select(j => rand.Next(0, 2))
                                                   .ToArray())
                             .ToArray();
        }

        static void EvaluateFitness()
        {
            for (int i = 0; i < populationSize; i++)
            {
                fitness[i] = Fitness(population[i]);
            }
        }

        static int[] TournamentSelection()
        {
            int[] tournamentPopulation = new int[tournamentSize];
            for (int i = 0; i < tournamentSize; i++)
            {
                tournamentPopulation[i] = rand.Next(0, populationSize);
            }

            int bestIndex = Array.IndexOf(fitness, tournamentPopulation.Select(i => fitness[i]).Max());
            return population[bestIndex];
        }

        static int[] Crossover(int[] parent1, int[] parent2)
        {
            if (rand.NextDouble() < crossoverProbability)
            {
                int crossoverPoint = rand.Next(1, weights.Length);

                int[] child = new int[weights.Length];
                Array.Copy(parent1, child, crossoverPoint);
                Array.Copy(parent2, crossoverPoint, child, crossoverPoint, weights.Length - crossoverPoint);
                return child;
            }
            else
            {
                return (int[])parent1.Clone();
            }
        }
        static void Mutate(int[] child)
        {
            for (int i = 0; i < child.Length; i++)
            {
                if (rand.NextDouble() < mutationProbability)
                {
                    child[i] = 1 - child[i];
                }
            }
        }
        static void LocalSearch(int[] solution)
        {
            int[] currentSolution = (int[])solution.Clone();
            int currentFitness = Fitness(currentSolution);

            for (int i = 0; i < maxIterations; i++)
            {
                int index = rand.Next(0, weights.Length);
                int[] newSolution = (int[])currentSolution.Clone();
                newSolution[index] = 1 - newSolution[index];
                int newFitness = Fitness(newSolution);

                if (newFitness > currentFitness)
                {
                    currentSolution = newSolution;
                    currentFitness = newFitness;
                }
            }

            ReplaceWorst(currentSolution);
        }

        static void ReplaceWorst(int[] child)
        {
            int worstIndex = Array.IndexOf(fitness, fitness.Min());
            population[worstIndex] = child;
            fitness[worstIndex] = Fitness(child);
        }

        static int Fitness(int[] solution)
        {
            int weight = 0;
            int value = 0;

            for (int i = 0; i < solution.Length; i++)
            {
                if (solution[i] == 1)
                {
                    weight += weights[i];
                    value += values[i];
                }
            }

            if (weight > capacity)
            {
                return 0;
            }
            else
            {
                return value;
            }
        }
    }
}
