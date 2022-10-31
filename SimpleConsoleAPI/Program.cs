// See https://aka.ms/new-console-template for more information
using System.Data;

class Program
{
    //swap parameter
    public static void Swap(ref int x, ref int y)
    {
        int tmp = x;
        x = y;
        y = tmp;
    }
    public static void SwapExample()
    {
        int i = 1;
        int j = 2;
        Swap(ref i, ref j);
        Console.WriteLine($"{i} {j}");
    }

    //out parameter
    static void findResult(int x, int y, out int quotient, out int remainder)
    {
        quotient = x / y;
        remainder = x % y;
    }

    public static void divideExample()
    {
        int x = 10;
        int y = 3;
        findResult(x, y, out int quo, out int rem);
        Console.WriteLine($"{quo} {rem}");
    }

    static void F() => Console.WriteLine("F()");
    static void F(object x) => Console.WriteLine("F(object)");
    static void F(int x) => Console.WriteLine("F(int)");
    static void F(double x) => Console.WriteLine("F(double)");
    static void F<T>(T x) => Console.WriteLine($"F<T>(T), T is {typeof(T)}");
    static void F(double x, double y) => Console.WriteLine("F(double, double)");
    public static void UsageExample()
    {
        F(); // Invokes F()
        F(1); // Invokes F(int)
        F(1.0); // Invokes F(double)
        F("abc"); // Invokes F<T>(T), T is System.String
        F((double)1); // Invokes F(double)
        F((object)1); // Invokes F(object)
        F<int>(1); // Invokes F<T>(T), T is System.Int32
        F(1, 1); // Invokes F(double, double)
    }

}

class Runtime
{
    public virtual void sum(int x, int y) => Console.WriteLine("Print Sum : " + (x + y));

}

class Test : Runtime
{
    public override void sum(int x, int y) =>  Console.WriteLine("Print Mul : "+ (x* y));
    
}

//Delegates and lambdaexpressions

public delegate double Function(double x);

class Multiplier {
    public double _factor;
    public Multiplier(double factor)
    {
        _factor = factor;
    }   
    public double multiply(double x) =>  x * _factor;
}

class DelegateExample
{
    public static double[] Apply(double[] a, Function f)
    {
        var result = new double[a.Length];
        for(int i=0; i<a.Length; i++) result[i] = f(a[i]);
        return result;
    }
    public static void test()
    {
        double[] a = { 0.0, 0.5, 1.0 };
        double[] square = Apply(a, (x) => x * x);
        Console.Write("Square value : ");
        for (int i = 0; i < square.Length; i++)
        {
            Console.Write(square[i] + " ");
        }
        Console.WriteLine();
        double[] sine = Apply(a, Math.Sin);
        Console.Write("Sine value : ");
        for (int i = 0; i < sine.Length; i++)
        {
            Console.Write(sine[i] + " ");
        }
        Console.WriteLine();

        Multiplier m = new(2.0);
        double[] mult = Apply(a, m.multiply);
        Console.Write("Multiplication value : ");
        for (int i = 0; i < mult.Length; i++)
        {
            Console.Write(mult[i] + " ");
        }
        Console.WriteLine();
    }
}
