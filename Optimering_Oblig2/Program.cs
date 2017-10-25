using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimering_Oblig2 {
    class Program {
        static void Main(string[] args) {
            // Random number
            Random rnd = new Random();
            // Array for colours (Black, White, Red)
            char[] colours = {'B', 'W', 'R'};
            // Create an 5*5 2D array (graph with 5 nodes)
            int[,] graph = { {0, 1, 0, 0, 1}, {1, 0, 1, 1, 0}, {0, 1, 0, 1, 1}, {0, 1, 1, 0, 0}, {1, 0, 1, 0, 0}};
            // Array for fitness
            int[] fitness = {int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue};

            Console.Write("Colours:\n");
            WriteArray(colours);
            Console.Write("Graph matrix:\n");
            WriteMultiArray(graph, 4, 4);

            // Generate 2 initial populations (solution)
            char[] initSolution1 = new char[graph.GetLength(0)];
            char[] initSolution2 = new char[graph.GetLength(0)];
            CreateInitSolution(initSolution1, colours, rnd);
            CreateInitSolution(initSolution2, colours, rnd);
            // Write the solutions
            Console.Write("Solution 0: ");
            WriteArray(initSolution1);
            Console.Write("Solution 1: ");
            WriteArray(initSolution2);

            // Find fitness to initial solutions
            fitness[0] = FindFitness(initSolution1, graph);
            Console.Write("Fitness: {0}\n", fitness[0]);
            fitness[1] = FindFitness(initSolution2, graph);
            Console.Write("Fitness: {0}\n", fitness[1]);
            
            //while() {
                // Select parents
                SelectParents(fitness);
            //}
            
            Console.Read();
            
        }

        // Create initial solution (population)
        static void CreateInitSolution(char[] initSolution, char[] colours, Random rnd) {
            for (int i = 0; i < initSolution.GetLength(0); i++) {
                initSolution[i] = colours[rnd.Next(0, 3)];
            }
        }

        // Find the fitness to the solution. Fitness is the number of edges in the graph
        // that has the same colour associated with its connecting nodes
        static int FindFitness(char[] solution, int [,] graph) {
            int fitness = 0;
            int length = graph.GetLength(0);
            for (int i = 0; i < length; i++) {
                for (int j = i; j < length; j++) {
                    if (graph[i,j] == 1) {
                        if (solution[i].Equals(solution[j])) {
                            Console.Write("{0}({1}) og {2}({3}) er like!\n", i, solution[i], j, solution[j]);
                            fitness++;
                        }
                    }
                }
            }
            return fitness;
        }

        static void SelectParents(int[] fitness) {
            int parent1;
            int parent2;
            int bestFitnessIndex1 = CompareFitness(fitness, int.MaxValue);
            int bestFitnessIndex2 = CompareFitness(fitness, bestFitnessIndex1);
            Console.Write("Foreldre:\n {0} - {1} fitness\n {2} - {3} fitness\n", bestFitnessIndex1, fitness[bestFitnessIndex1], bestFitnessIndex2, fitness[bestFitnessIndex2]);
            for (int i = 0; i < fitness.GetLength(0); i++) {

            }
        }

        static int CompareFitness(int[] fitness, int ignore) {
            int bestFitnessIndex = int.MaxValue;
            int bestFitness = int.MaxValue;
            for (int i = 0; i < fitness.GetLength(0); i++) {
                if (i == ignore) {
                    Console.Write("Beste fra tidligere: {0} (ignoreres)\n", ignore);
                    continue;
                }
                if (fitness[i] < bestFitness) {
                    bestFitnessIndex = i;
                    bestFitness = fitness[i];
                }
            }
            return bestFitnessIndex;
        }

        static void CrossOver() {

        }
        
        static void Mutation() {

        }

        // Write array to console
        static void WriteArray(int[] a) {
            foreach (var item in a)
                Console.Write("{0} ", item);
            Console.Write("\n\n");
        }

        // Write char array to console
        static void WriteArray(char[] a) {
            foreach (var item in a)
                Console.Write("{0} ", item);
            Console.Write("\n\n");
        }

        // Write Multi array to console
        static void WriteMultiArray(int[,] a, int numb, int numb2) {
            for (int i = 0; i < numb; i++) {
                for (int j = 0; j < numb2; j++) {
                    Console.Write(a[i, j] + " ");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }

    }
}
