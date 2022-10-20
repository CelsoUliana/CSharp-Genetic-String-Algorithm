# CSharp-Genetic-String-Algorithm
A simple C# implementation of a genetic string algorithm made with OOP. Made to work with online C# compilers (single file)
Such as [.NET fiddle](https://dotnetfiddle.net/).

Works with strings in general, just need to alter the alphabet (represents all possible characters for the problem, could be binary for example).

Supply the fitness function with a goal, and it will measure fitness relative to the goal.

Population size should be around 50-100, crossover chance should be less than 1/2 and mutation should be less than 1/10. These parameters can be fine-tuned for the alphabet/problem.