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
        int count = 0;
        foreach (var c in classes)
        {
            count++;
            WritePromptToScreen($"{count} - {c}");
        }
        ConsoleKey key = Console.ReadKey().Key;
        Type classChoice = null;
        switch (key)
        {
            case ConsoleKey.D1:
                classChoice = classes[0];
                break;
            case ConsoleKey.D2:
                classChoice = classes[1];
                break;
            case ConsoleKey.D3:
                classChoice = classes[2];
                break;
            case ConsoleKey.D4:
                classChoice = classes[3];
                break;
            default:
                WritePromptToScreen("Invalid choice");
                break;
        };

        object classInstance = Activator.CreateInstance(classChoice, null);
        Console.Clear();

        WriteHeadingToScreen($"Class: {classChoice}");

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

