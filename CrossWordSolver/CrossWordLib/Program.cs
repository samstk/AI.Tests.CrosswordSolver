// See https://aka.ms/new-console-template for more information

using SSDK.AI.CSP;

public static class Program {
    public class CrossWord : CSP
    {
        public Dictionary<string, CSPVariable> VDict = new Dictionary<string, CSPVariable>();
        public string[] Words;
        public void AddVariable(string key, int length)
        {
            CSPVariable variable = key;
            variable.Domain = Words;
            // Length constraint
            AddConstraint(new CSPConstraint(() =>
            {
                if (variable.Solution != null)
                {
                    string word = (string)variable.Solution;
                    if (word.Length != length)
                        return false;
                }
                return true;
            }, variable, name: $"Length Constraint ({length})"));
            VDict.Add(key, variable);
            Variables.Add(variable);
        }

        public void AddIntersection(string key1, string key2, int index1, int index2)
        {
            CSPVariable variable1 = VDict[key1];
            CSPVariable variable2 = VDict[key2];
            AddConstraint(new CSPConstraint(() =>
            {
                if(variable1.Solution != null && variable2.Solution != null)
                {
                    string sol1 = (string)variable1.Solution;
                    string sol2 = (string)variable2.Solution;
                    return index1 < sol1.Length && index2 < sol2.Length
                    && sol1[index1] == sol2[index2];
                }
                return true;
            }, new CSPVariable[] { variable1, variable2 }));
        }
        public CrossWord()
        {
            Words = new string[]
            {
                "AFT", "ALE", "EEL", "HEEL",
                "HIKE", "HOSES", "KEEL", "KNOT",
                "LASER", "LEE", "LINE", "SAILS",
                "SHEET", "STEER", "TIE"
            };
            // Enter crossword variables
            AddVariable("1A", 5);
            AddVariable("2D", 5);
            AddVariable("3D", 5);
            AddVariable("4A", 4);
            AddVariable("5D", 4);
            AddVariable("7A", 3);
            AddVariable("6D", 3);
            AddVariable("8A", 5);

            AddIntersection("1A", "2D", 2, 0);
            AddIntersection("1A", "3D", 4, 0);
            AddIntersection("2D", "4A", 2, 1);
            AddIntersection("2D", "7A", 3, 0);
            AddIntersection("2D", "8A", 4, 2);
            AddIntersection("3D", "4A", 2, 3);
            AddIntersection("3D", "7A", 3, 2);
            AddIntersection("3D", "8A", 4, 4);
            AddIntersection("4A", "5D", 2, 0);
            AddIntersection("5D", "7A", 1, 1);
            AddIntersection("6D", "8A", 1, 0);

            AddUniqueConstraint(Variables.ToArray());

            ReduceVariableDomains();
        }
    }
    public static void Main(string[] args)
    {
        CrossWord crossword = new CrossWord();
        crossword.Solve();

        foreach(CSPVariable variable in crossword.Variables)
        {
            Console.WriteLine($"{variable.Name}: {variable.Solution}");
        }
    }
}
