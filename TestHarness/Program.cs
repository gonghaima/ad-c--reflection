using System.Reflection;

namespace TestHarness;

class Program
{
    const string InformationAttributeTypeName = "UTILITYFUNCTIONS.INFORMATIONATTRIBUTE";
    static void Main(string[] args)
    {
        const string TargetAssemblyFileName = "../../UtilityFunctions.dll";
        const string TargetNamespace = "UtilityFunctions";

        Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

        // Assembly assembly = Assembly.LoadFile(Path.Combine(
        //                                 AppDomain.CurrentDomain.BaseDirectory,
        //                                 TargetAssemblyFileName));


        string targetAssemblyFileName = "UtilityFunctions.dll";
        string pathToUtilityFunctions = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", "..", "UtilityFunctions", "bin", "Debug", "net7.0", targetAssemblyFileName);

        Assembly assembly;
        List<System.Type> classes = new List<Type>();


        if (File.Exists(pathToUtilityFunctions))
        {
            assembly = Assembly.LoadFile(pathToUtilityFunctions);
            Console.WriteLine("Assembly loaded successfully.");
            classes = assembly.GetTypes()
            .Where(t => t.Namespace == TargetNamespace && HasInformationAttribute(t)).ToList();
        }
        else
        {
            Console.WriteLine($"File not found: {pathToUtilityFunctions}");
        }



        WritePromptToScreen("Please press the number key associated with " +
                                    "the class you wish to test");

        DisplayProgramElementList(classes);

        Type typeChoice = ReturnProgramElementReferenceFromList(classes);


        

        object classInstance = Activator.CreateInstance(typeChoice, null);
        Console.Clear();

        WriteHeadingToScreen($"Class: {typeChoice}");

        WritePromptToScreen("Please press the number key associated with " +
                                    "the method you wish to test");

    }

    private static bool HasInformationAttribute(MemberInfo mi)
    {

        foreach (var attrib in mi.GetCustomAttributes())
        {
            Type typeAttrib = attrib.GetType();

            if (typeAttrib.ToString().ToUpper() == InformationAttributeTypeName)
            {
                return true;
            }

        }
        return false;
    }


    private static T ReturnProgramElementReferenceFromList<T>(List<T> list)
    {
        ConsoleKey consoleKey = Console.ReadKey().Key;

        switch (consoleKey)
        {
            case ConsoleKey.D1:
                return list[0];
            case ConsoleKey.D2:
                return list[1];
            case ConsoleKey.D3:
                return list[2];
            case ConsoleKey.D4:
                return list[3];

        }
        return default;
    }

    private static void DisplayProgramElementList<T>(List<T> list)
    {
        int count = 0;

        foreach (var item in list)
        {
            count++;
            Console.WriteLine($"{count}. {item}");
        }

    }
    private static void WriteHeadingToScreen(string heading)
    {
        Console.WriteLine(heading);
        Console.WriteLine(new string('-', heading.Length));
        Console.WriteLine();

    }

    private static void WritePromptToScreen(string promptText)
    {
        Console.WriteLine(promptText);
    }
}

