using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimering_Oblig2 {
    class Program {

        // Random number
        static Random rnd = new Random();
        // Array for colours (Black, White, Red)
        static char[] colours = {'B', 'W', 'R'};
        // Create an 5*5 2D array (graph with 5 nodes)
        static int[,] graph = { {0, 1, 0, 0, 1}, {1, 0, 1, 1, 0}, {0, 1, 0, 1, 1}, {0, 1, 1, 0, 0}, {1, 0, 1, 0, 0}};
        // Array for fitness
        static int[] fitness = {int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue};
        // Population (solutions array list
        static List<char[]> population = new List<char[]>();
        // Parents array
        static int[] parents = new int[2];
        static int[] childs = {-1, -1};

        static void Main(string[] args) {

            Console.Write("Colours:\n");
            WriteArray(colours);
            Console.Write("Graph matrix:\n");
            WriteMultiArray(graph, 4, 4);

            // Generate 2 initial populations (solution)
            char[] initSolution1 = new char[graph.GetLength(0)];
            char[] initSolution2 = new char[graph.GetLength(0)];
            // Crate the solutions
            CreateInitSolution(initSolution1);
            CreateInitSolution(initSolution2);
            // Add the solutions to the list
            population.Add(initSolution1);
            population.Add(initSolution2);
            // Write the solutions
            WriteArrayList(population);

            // Find fitness to initial solutions
            Console.Write("---- Solution 0 -----\n");
            fitness[0] = FindFitness(population[0]);
            Console.Write("Fitness: {0}\n", fitness[0]);
            Console.Write("---- Solution 1 -----\n");
            fitness[1] = FindFitness(population[1]);
            Console.Write("Fitness: {0}\n", fitness[1]);

            //while() {

                // Select parents
            SelectParents(parents);
            // Write about selected parents
            Console.Write("Valgte foreldre:\n");
            Console.Write(" {0} - Fitness: {1} - Populasjon: ", parents[0], fitness[parents[0]]);
            WriteArrayList(population, parents[0]);
            Console.Write(" {0} - Fitness: {1} - Populasjon: ", parents[1], fitness[parents[1]]);
            WriteArrayList(population, parents[1]);

            // Crossover
            WriteArray(childs);
            CrossOver();
            //}
            
            Console.Read();
            
        }

        // Create initial solution (population)
        static void CreateInitSolution(char[] initSolution) {
            for (int i = 0; i < initSolution.GetLength(0); i++) {
                initSolution[i] = colours[rnd.Next(0, 3)];
            }
        }

        // Find the fitness to the solution. Fitness is the number of edges in the graph
        // that has the same colour associated with its connecting nodes
        static int FindFitness(char[] solution) {
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

        static void SelectParents(int[] parents) {
            // parent1 and parent2 is the index of the 2 best values in the fitness array
            int parent1 = CompareFitness(int.MaxValue);
            int parent2 = CompareFitness(parent1);
            Console.Write("Foreldre:\n {0} - {1} fitness\n {2} - {3} fitness\n", parent1, fitness[parent1], parent2, fitness[parent2]);
            parents[0] = parent1;
            parents[1] = parent2;

            // Oppdate the child array
            int ignore = -1;
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 4; j++) {
                    if (!parents.Contains(j) && ignore != j) {
                        Console.Write("{0} er en IKKE forelder!\n", j);
                        childs[i] = j;
                        ignore = j;
                        break;
                    }
                }
            }
        }

        static int CompareFitness(int ignore) {
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
            //WriteArray(childs);
            int range1 = rnd.Next(0, graph.GetLength(0));
            int range2 = rnd.Next(0, graph.GetLength(0));
            while (range1 == range2) {
                range2 = rnd.Next(0, graph.GetLength(0));
            }
            Console.Write("Range1: {0}\nRange2: {1}\n", range1, range2);
            for (int i = 0; i < graph.GetLength(0); i++) {
                // If i is within the range bounderies
                if ((i > range1 && i < range2) || (i > range2 && i < range1)) {
                    // Child 1 inherits from parent 1
                    population[childs[0]][i] = population[parents[0]][i];
                    // Child 2 inherits from parent 2
                    population[childs[1]][i] = population[parents[1]][i];
                } else {
                    // Child 1 inherits from parent 2
                    population[childs[0]][i] = population[parents[1]][i];
                    // Child 2 inherits from parent 1
                    population[childs[1]][i] = population[parents[0]][i];
                }
            }

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

        // Print array list
        static void WriteArrayList(List<char[]> list) {
            for (int i = 0; i < list.Count(); i++) {
                for (int j = 0; j < list[i].Count(); j++) {
                    Console.Write(list[i][j] + " ");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
        }

        // Print single array in list
        static void WriteArrayList(List<char[]> list, int number) {
            for (int i = 0; i < list.Count(); i++) {
                if (i != number) { 
                    continue;
                }
                for (int j = 0; j < list[i].Count(); j++) {
                    Console.Write(list[i][j] + " ");
                }
            }
            Console.Write("\n");
        }

    }
}
