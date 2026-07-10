using System;

namespace DesignPatternPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            DungeonGenerator generator = new DungeonGenerator();

            generator.GenerateDungeon();
        }
    }
}