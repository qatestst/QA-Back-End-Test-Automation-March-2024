// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace CodeAnalyzerDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hi!");
        }

        private static string MakeGreeting(string name)
        {
            // No braces and spacing issues
            if (name == null) 
            {
                throw new ArgumentNullException("name");
            }

            return "Hello, " + name;
        }
    }
}