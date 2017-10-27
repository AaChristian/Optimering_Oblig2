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
        // Create an 12*12 array (graph with 12 nodes)
        /*static int[,] graph = {
            {0,1,0,1,0,0,0,0,0,0,0,0}, // 0
            {1,0,1,0,1,1,0,0,0,0,0,0}, // 1
            {0,1,0,0,0,1,1,0,0,1,0,0}, // 2
            {1,0,0,0,1,0,0,1,0,0,0,0}, // 3
            {0,1,0,1,0,0,0,1,1,0,0,0}, // 4
            {0,1,1,0,0,0,0,0,1,1,0,0}, // 5
            {0,0,1,0,0,0,0,0,0,1,0,0}, // 6
            {0,0,0,1,1,0,0,0,1,0,1,0}, // 7
            {0,0,0,0,1,1,0,1,0,0,0,1}, // 8
            {0,0,1,0,0,1,1,0,0,0,0,1}, // 9
            {0,0,0,0,0,0,0,1,0,0,0,1}, // 10
            {0,0,0,0,0,0,0,0,1,1,1,0}  // 11
        };*/
        //static int[,] graph = new int[8,8];
        // Startpopulation
        static int numbOfPopulation = 4;
        // Array for fitness (4 values since max 4 solutions)
        static int[] fitness = {int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue};
        // Population (solutions array list
        static List<char[]> population = new List<char[]>();
        // Parents array
        static int[] parents = new int[2];
        static int[] childs = {-1, -1};

        static void Main(string[] args) {
            Console.Write("Colours:\n");
            WriteArray(colours);
            // Generate random matrix (random graph)
            //CreateSymmetricMatrix(graph, graph.GetLength(0));
            Console.Write("Graph matrix:\n");
            WriteMultiArray(graph, graph.GetLength(0), graph.GetLength(0));

            // Populate the list with populations(solutions)
            for (int i = 0; i < numbOfPopulation; i++){
                population.Add(new char[graph.GetLength(0)]);
            }
            // Generate 2 initial populations (solution)
            char[] initSolution1 = new char[graph.GetLength(0)];
            char[] initSolution2 = new char[graph.GetLength(0)];
            // Crate the solutions
            CreateInitSolution(initSolution1);
            CreateInitSolution(initSolution2);
            // Add the solutions to the list
            population[0] = initSolution1;
            population[1] = initSolution2;
            // Write the solutions
            Console.Write("Starting solutions:\n");
            WriteArrayList(population);

            // Find fitness to initial solutions
            Console.Write("Find starting fitness:\n");
            Console.Write("---- Solution 0 -----\n");
            fitness[0] = FindFitness(population[0]);
            Console.Write("Fitness: {0}\n", fitness[0]);
            Console.Write("---- Solution 1 -----\n");
            fitness[1] = FindFitness(population[1]);
            Console.Write("Fitness: {0}\n", fitness[1]);

            bool stop = false;
            int iteration = 1;
            while(!stop) {
                Console.Write("========== Iteration {0} ============\n", iteration);
                // Select parents
                SelectParents(parents);
                // Write about selected parents
                Console.Write("Valgte foreldre:\n");
                Console.Write(" {0} - Fitness: {1} - Populasjon: ", parents[0], fitness[parents[0]]);
                WriteArrayList(population, parents[0]);
                Console.Write(" {0} - Fitness: {1} - Populasjon: ", parents[1], fitness[parents[1]]);
                WriteArrayList(population, parents[1]);

                // Crossover
                Console.Write("--- Cross-Over ---- \n");
                Console.Write("The childs are solutions: ");
                WriteArray(childs);
                CrossOver();
                Console.Write("Populasjonen etter Cross-Over:\n");
                WriteArrayList(population);
                // Mutation
                Console.Write("--- Mutation ---- \n");
                double pm = rnd.NextDouble();
                Console.Write(pm);
                if (pm > 0.5) {
                    Console.Write("\nMutasjon skjer!\n");
                    Mutation();
                    WriteArrayList(population);
                }
                // Calculate fitness
                Console.Write("--- Find fitness ---- \n");
                for (int i = 0; i < population.Count; i++) {
                    Console.Write("Løsning {0}\n", i);
                    fitness[i] = FindFitness(population[i]);
                }
                WriteArray(fitness);

                // Stop criteria
                int numberOfBestFitness = 0;
                for (int i = 0; i < fitness.GetLength(0); i++) {
                    if (fitness[i] == 0) {
                        numberOfBestFitness++;
                    }
                }
                Console.Write("Number of solutions with best fitness: {0}\n", numberOfBestFitness);
                // If the fitness to all the solutions is 0, stop!
                // else if number of iterations exceeds a certain number and at least one solution has fitness 0, stop!
                // else stop after 500 iterations if no perfect fitness is found in any solution
                if (numberOfBestFitness == fitness.GetLength(0)) {
                    Console.Write("***** Found perfect fitness to all solutions after {0} iterations. *****\n", iteration);
                    stop = true;
                } else if (iteration >= 10 && numberOfBestFitness != 0) {
                    Console.Write("***** Found perfect fitness to {0} solution(s) after {1} iterations. *****\n", numberOfBestFitness, iteration);
                    stop = true;
                } else if (iteration >= 500 && numberOfBestFitness == 0) {
                    Console.Write("***** Found no perfect fitness to any solutions after {1} iterations. *****\n", numberOfBestFitness, iteration);
                    stop = true;
                }

                iteration++;
            }
            
            Console.Write("Siste populasjonen:\n");
            WriteArrayList(population);
            Console.Write("Med fitness:\n");
            WriteArray(fitness);
            
            int[,] graphTest = new int[4,4];
            CreateSymmetricMatrix(graphTest, graphTest.GetLength(0));
            WriteMultiArray(graphTest, graphTest.GetLength(0), graphTest.GetLength(0));
            CheckIfGraphConnected(graphTest, graphTest.GetLength(0));
            
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

        // Select parents
        static void SelectParents(int[] parents) {
            // parent1 and parent2 is the index of the 2 best values in the fitness array
            int parent1 = CompareFitness(int.MaxValue);
            int parent2 = CompareFitness(parent1);
            //Console.Write("Foreldre:\n {0} - {1} fitness\n {2} - {3} fitness\n", parent1, fitness[parent1], parent2, fitness[parent2]);
            parents[0] = parent1;
            parents[1] = parent2;

            // Oppdate the child array
            int ignore = -1;
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 4; j++) {
                    if (!parents.Contains(j) && ignore != j) {
                        //Console.Write("{0} er IKKE forelder!\n", j);
                        childs[i] = j;
                        ignore = j;
                        break;
                    }
                }
            }
        }

        // Compare fitness
        static int CompareFitness(int ignore) {
            int bestFitnessIndex = int.MaxValue;
            int bestFitness = int.MaxValue;
            for (int i = 0; i < fitness.GetLength(0); i++) {
                if (i == ignore) {
                    //Console.Write("Beste fra tidligere: {0} (ignoreres)\n", ignore);
                    continue;
                }
                if (fitness[i] < bestFitness) {
                    bestFitnessIndex = i;
                    bestFitness = fitness[i];
                }
            }
            return bestFitnessIndex;
        }

        // Crossover
        static void CrossOver() {
            //WriteArray(childs);
            int range1 = rnd.Next(0, graph.GetLength(0));
            int range2 = rnd.Next(0, graph.GetLength(0));
            while (range1 == range2) {
                range2 = rnd.Next(0, graph.GetLength(0));
            }
            if (range1 < range2) {
                Console.Write("Range: {0} - {1}\n", range1, range2);
            } else if (range2 < range1) {
                Console.Write("Range: {0} - {1}\n", range2, range1);
            }
            //Console.Write(population[1]);
            //Console.Write(population[0][0]);
            for (int i = 0; i < graph.GetLength(0); i++) {
                // If i is within the range bounderies
                //Console.Write(population[childs[0]][0]);
                if ((i >= range1 && i <= range2) || (i >= range2 && i <= range1)) {
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
        
        // Do mutation on childs
        static void Mutation() {
            int pos1;
            int posColour;
            for (int i = 0; i < 2; i++) {
                // Get a random position on population
                pos1 = rnd.Next(0, graph.GetLength(0));
                // Get a random colour
                posColour = rnd.Next(0, colours.GetLength(0));
                Console.Write("Bytte posisjon {0} i solution {1} til farge {2}\n", pos1, childs[i], colours[posColour]);
                // If the randomly selected colour is already the value in the position, select a new colour
                while (population[childs[i]][pos1].Equals(colours[posColour])) {
                    posColour = rnd.Next(0, colours.GetLength(0));
                    Console.Write("NVM!!!!! Bytte posisjon {0} i solution {1} til farge {2}\n", pos1, childs[i], colours[posColour]);
                }
                // Assign the new colour to the position
                population[childs[i]][pos1] = colours[posColour];
            }
        }

        // Create a random symmetric matrix.
        static void CreateSymmetricMatrix(int[,] matrix, int length) {
            int randomInt = 0;
            for (int i = 0; i < length; i++) {
                for (int j = i; j < length; j++) {
                    if (i == j) {
                        matrix[i,j] = 0;
                    } else {
                        randomInt = rnd.Next(0, 2);
                        matrix[i,j] = randomInt;
                        matrix[j,i] = randomInt;
                    }
                }
            }
        }

        static void CheckIfGraphConnected(int[,] matrix, int length) {
            int[] connectedNodes = new int[length];
            for (int i = 0; i < length; i++) {
                for (int j = i; j < length; j++) {
                    if (matrix[i,j] == 1) {
                        Console.Write("{0} er koblet med {1}\n", i, j);
                        if(!connectedNodes.Contains(i)) {
                            connectedNodes[i] == i;
                        }
                        if(!connectedNodes.Contains(j)) {
                            connectedNodes[i] == j;
                        }
                    }
                }
            }
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
