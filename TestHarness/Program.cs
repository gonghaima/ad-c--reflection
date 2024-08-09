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

        WriteHeadingToScreen($"Class: '{typeChoice}'");

        WritePromptToScreen("Please press the number key associated with " +
                                    "the method you wish to test");

        List<MethodInfo> methods = typeChoice.GetMethods().Where(m => HasInformationAttribute(m)).ToList();

        DisplayProgramElementList(methods);

        MethodInfo methodChoice = ReturnProgramElementReferenceFromList(methods);

        if (methodChoice != null)
        {
            Console.Clear();

            WriteHeadingToScreen($"Class: '{typeChoice}' - Method: '{methodChoice.Name}'");

            DisplayElementDescription(ReturnInformationCustomAttributeDescription(methodChoice));

            ParameterInfo[] parameters = methodChoice.GetParameters();

            object result = GetResult(classInstance, methodChoice, parameters);

            WriteResultToScreen(result);


        }

        Console.WriteLine();
        WritePromptToScreen("Please press the 'Spacebar' key to end the application " +
            "or any other key to continue...");

        //if (Console.ReadKey().Key == ConsoleKey.Spacebar)
        //{
        //    break;
        //}

    }

    private static string ReturnInformationCustomAttributeDescription(MemberInfo mi)
    {
        const string InformationAttributeDescriptionPropertyName = "Description";

        foreach (var attrib in mi.GetCustomAttributes())
        {
            Type typeAttrib = attrib.GetType();

            if (typeAttrib.ToString().ToUpper() == InformationAttributeTypeName)
            {
                PropertyInfo propertyInfo = typeAttrib.GetProperty(InformationAttributeDescriptionPropertyName);
                if (propertyInfo != null)
                {
                    object s = propertyInfo.GetValue(attrib, null);

                    if (s != null)
                        return s.ToString();
                }
            }

        }
        return null;

    }

    private static void DisplayElementDescription(string elementDescription)
    {
        if (elementDescription != null)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(elementDescription);
            Console.ResetColor();
            Console.WriteLine();
        }
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


    private static void WriteResultToScreen(object result)
    {
        Console.WriteLine();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Result: {result}");
        Console.ResetColor();
        Console.WriteLine();
    }

    private static object GetResult(Object classInstance, MethodInfo methodInfo,
                                        ParameterInfo[] parameters)
    {
        object result = null;

        if (parameters.Length == 0)
        {
            result = methodInfo.Invoke(classInstance, null);
        }
        else
        {
            var paramValueArray = ReturnParameterValueInputAsObjectArray(parameters);
            result = methodInfo.Invoke(classInstance, paramValueArray);
        }
        return result;
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
    private static object[] ReturnParameterValueInputAsObjectArray(ParameterInfo[] parameters)
    {
        object[] paramValues = new object[parameters.Length];
        int itemCount = 0;

        foreach (ParameterInfo parameterInfo in parameters)
        {

            WritePromptToScreen($"Please enter a value for the parameter named, '{parameterInfo.Name}'");

            if (parameterInfo.ParameterType == typeof(string))
            {
                string inputString = Console.ReadLine();
                paramValues[itemCount] = inputString;
            }
            else if (parameterInfo.ParameterType == typeof(int))
            {
                int inputInt = Int32.Parse(Console.ReadLine());
                paramValues[itemCount] = inputInt;
            }
            else if (parameterInfo.ParameterType == typeof(double))
            {
                double inputDouble = Double.Parse(Console.ReadLine());
                paramValues[itemCount] = inputDouble;
            }

            itemCount++;

        }
        return paramValues;
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

