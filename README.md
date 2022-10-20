# CSharp-Genetic-String-Algorithm
A simple C# implementation of a genetic string algorithm made with OOP. Made to work with online C# compilers (single file)
Such as [.NET fiddle](https://dotnetfiddle.net/).

Works with strings in general, just need to alter the alphabet (represents all possible characters for the problem, could be binary for example).

Supply the fitness function with a goal, and it will measure fitness relative to the goal.

Population size should be around 50-100, crossover chance should be less than 1/2 and mutation should be less than 1/10. These parameters can be fine-tuned for the alphabet/problem.

# Algorithm Parts/Modules Explanation:

### 1 - There is a chromosome object, which you can access the individual genes/sequence.
### 2 - There is a fitness function, which takes a chromosome and outputs a value that represents the fitness of that chromosome.
### 3 - There is a selection function, which select chromosomes to mate. This can be done at random, but some sort of biased towards high fitness selection like roulette selection works best.
### 4 - There is a crossover function, which is used to "mate" two chromosomes, that is, exchange sequence/genes and outputs two new chromosomes. Happens randomly, but should be quite often.
### 5 - There is a mutation function, which randomly alters the genes/sequence of a chromosome based on a random chance.



# Algorithm Explanation:

### 1 - The algorithm receives initial parameters (chance of mutation, chance of crossover, fitness function, chromosome length, population size, the problem alphabet, the number of generations).

### 2 - A genome is created (collection of chromosomes) with the first generation chromosomes being generated randomly, then will search for the perfect fitness (solution to the problem).

### 3 - If a solution is not found, then the generation evolves. The evolution happens like this: 
### New list of chromosomes is created.
### Two chromosomes are selected randomly, but biased towards higher fitness.
### There is a chance the crossover will occur and sequences will be overlapped to the sons chromosomes.
### There is a chance that a mutation will occur and change the sequence/genes at a single character.
### These two chromosomes are added to the the the new list.
### After the new list of chromosomes reaches the population size, the older population is replaced.
### New generation fitness will be evaluated.
### Algorithm repeats while the sequence/genes with perfect fitness is not found or number of generation exceeds the parameter. Each repetition is called a "generation". 
