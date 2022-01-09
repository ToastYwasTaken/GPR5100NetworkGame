using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomNameGenerator
{
    private static string[] firstNames = { "Ava", "Alexander", "Amelia", "Aiden", "Emma", "Luna", "Elijah", "Evelyn", "Ezra", "Liam", "Sebastian", "Owen", "Sophia", "Scarlett", "Santiago", "Mary", "James", "Robert", "Patricia", "Jennifer"  };
    private static string[] lastNames = { "Smith", "Johnson", "Brown", "Jones", "Miller", "Thomas", "Taylor",  "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores", "Green", "Adams", "Nelson", "Baker" };


    public static string GenerateRandomName()
    {
        int seed = (int)System.DateTime.Now.Ticks;
        System.Random rdm = new System.Random(seed);

        int rdmFirstNameIdx = rdm.Next(0, firstNames.Length+1);
        int rdmLastNameIdx = rdm.Next(0, lastNames.Length + 1);

        return firstNames[rdmFirstNameIdx] + lastNames[rdmLastNameIdx] + seed.ToString();
    }
}
