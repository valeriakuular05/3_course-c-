using System;
using System.Text;


namespace Variant_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Вариант 2\n");

            // 1. Создать объект типа DataItemD, вывести его данные с помощью метода ToString()
            Console.WriteLine("1. DataItemD:");
            DataItemD dataItem = new DataItemD(1.5, 2.5, (3.14, 2.71));
            Console.WriteLine(dataItem.ToString());
            Console.WriteLine();

            // 2. Создать объект типа V1DataArray, вывести его данные с помощью метода ToLongString()
            Console.WriteLine("2. V1DataArray:");
            V1DataArray v1Array = new V1DataArray("TestData", (3, 0.5f), (2, 0.4f));
            Console.WriteLine(v1Array.ToLongString());
            Console.WriteLine();

            // 3. Для объекта V1DataArray вывести значения свойств Count и MinMax
            Console.WriteLine("3. Свойства V1DataArray:");
            Console.WriteLine($"Count: {v1Array.Count}");
            (float min, float max) minMax = v1Array.MinMax;
            Console.WriteLine($"MinMax: ({minMax.min:F3}, {minMax.max:F3})");
            Console.WriteLine();

            // 4. Для объекта V1DataArray вывести значения индексатора для двух комбинаций значений индексов
            Console.WriteLine("4. Индексатор V1DataArray:");
            
            // Валидные индексы
            Console.WriteLine("Валидные индексы (1, 1):");
            DataItemD? validItem = v1Array[1, 1];
            if (validItem.HasValue)
                Console.WriteLine(validItem.Value.ToString());
            else
                Console.WriteLine("null");
            
            // Невалидные индексы
            Console.WriteLine("Невалидные индексы (5, 5):");
            DataItemD? invalidItem = v1Array[5, 5];
            if (invalidItem.HasValue)
                Console.WriteLine(invalidItem.Value.ToString());
            else
                Console.WriteLine("null");
            Console.WriteLine();

            // 5. Преобразовать V1DataArray в V2JDataArray и вывести с помощью метода ToLongString()
            Console.WriteLine("5. Преобразование в V2JDataArray:");
            V2JDataArray v2Array = (V2JDataArray)v1Array;
            Console.WriteLine(v2Array.ToLongString());
            Console.WriteLine();

            // 6. Для V2JDataArray вывести значения свойств Count и MaxDifference
            Console.WriteLine("6. Свойства V2JDataArray:");
            Console.WriteLine($"Count: {v2Array.Count}");
            (double maxDiff, int maxI, int maxJ) maxDifference = v2Array.MaxDifference;
            Console.WriteLine($"MaxDifference: ({maxDifference.maxDiff:F3}, {maxDifference.maxI}, {maxDifference.maxJ})");
            Console.WriteLine();

            // 7. Для объекта V2JDataArray вывести значения индексатора для двух комбинаций значений индексов
            Console.WriteLine("7. Индексатор V2JDataArray:");
            
            // Валидные индексы
            Console.WriteLine("Валидные индексы (0, 1):");
            (double, double) validValues = v2Array[0, 1];
            Console.WriteLine($"({validValues.Item1:F3}, {validValues.Item2:F3})");
            
            // Невалидные индексы
            Console.WriteLine("Невалидные индексы (10, 10):");
            (double, double) invalidValues = v2Array[10, 10];
            Console.WriteLine($"({invalidValues.Item1:F3}, {invalidValues.Item2:F3})");
            Console.WriteLine();

            // 8. Изменить данные для одного из узлов сетки и вывести объект V2JDataArray
            Console.WriteLine("8. Изменение данных через индексатор:");
            Console.WriteLine("До изменения - узел (1, 0):");
            (double, double) beforeChange = v2Array[1, 0];
            Console.WriteLine($"({beforeChange.Item1:F3}, {beforeChange.Item2:F3})");
            
            // Изменяем данные
            v2Array[1, 0] = (99.999, 88.888);
            
            Console.WriteLine("После изменения - узел (1, 0):");
            (double, double) afterChange = v2Array[1, 0];
            Console.WriteLine($"({afterChange.Item1:F3}, {afterChange.Item2:F3})");
            
            Console.WriteLine("\nПолный объект после изменения:");
            Console.WriteLine(v2Array.ToLongString());
        }
    }


public struct DataItemD{
    public double Y{get; set;}
    public double X{get; set;}
    public (double, double) data{get; set;}

    public DataItemD(double x, double y, (double, double) v){
        Y = y; X = x; data = v;
    }

    public override string ToString() =>  $"X: {X} | Y: {Y} | data: {data.Item1:F3} {data.Item2:F3}";
}

class V1DataArray{
    private float[] mas; //измеренные данные
    public string Key{get; set;}
    public (int, float) Xgrid{get; private set;}
    public (int, float) Ygrid{get; private set;}

    public int Count{
        get {return Xgrid.Item1 * Ygrid.Item1;}
    }

    public V1DataArray(string str, (int, float) x, (int, float) y){
        Key = str;
        Xgrid = x;
        Ygrid = y;

        mas = new float[Count * 2]; 
        Random rand = new Random();
        for (int i = 0; i < mas.Length; i++){
            mas[i] = (float)(rand.NextDouble() * 23);
        }
    }
    public (float, float) MinMax{
        get{
            if (mas.Length == 0) return (0, 0);
            float min = mas[0];
            float max = mas[0];
            for (int i = 0; i < mas.Length; i++){
                if (mas[i] < min) min = mas[i];
                if (mas[i] > max) max = mas[i];
            }
            return (min, max);
        }
    }

    public DataItemD? this[int x, int y]{
        get{
            if ((x >= Xgrid.Item1) || (y >= Ygrid.Item1) || (x < 0) || (y < 0)) return null;
            int index = (x * Ygrid.Item1 + y) * 2;

            float coordX = x * Xgrid.Item2;
            float coordY = y * Ygrid.Item2;

            (float, float) v = (mas[index], mas[index + 1]);

            return new DataItemD(coordX, coordY, v);
        }
    }

    public override string ToString() => $"Key: {Key} | Xgrid.Item1: {Xgrid.Item1} Xgrid.Item2: {Xgrid.Item2:F3} | Ygrid.Item1: {Ygrid.Item1} Ygrid.Item2: {Ygrid.Item2:F3}";
    public string ToLongString(){
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(ToString());
        sb.AppendLine("Array float");
        
        for (int i = 0; i < Xgrid.Item1; i++)
            for (int j = 0; j < Ygrid.Item1; j++){
                int index = (i * Ygrid.Item1 + j) * 2;
                sb.Append($"[{i * Xgrid.Item2}:{j * Ygrid.Item2}] [{i}:{j}]{mas[index]:F3} {mas[index + 1]:F3} | ");
        };
        return sb.ToString();
    }

    public static explicit operator V2JDataArray(V1DataArray v1){
        V2JDataArray v2 = new V2JDataArray(v1.Key + "_converted", (v1.Xgrid.Item1, (double)v1.Xgrid.Item2, v1.Ygrid.Item1, (double)v1.Ygrid.Item2));

        for (int i = 0; i < v1.Xgrid.Item1; i++){
            for (int j = 0; j < v1.Ygrid.Item1; j++){
                DataItemD? dataItem = v1[i, j];
                if (dataItem.HasValue){
                    v2[i, j] = ((double)dataItem.Value.data.Item1, (double)dataItem.Value.data.Item2);
                }
                else v2[i, j] = (double.NaN, double.NaN);
            }
        }
        return v2;
    }
}

class V2JDataArray{
    private double[][] mas;
    public string Key{get; set;}
    public (int, double, int, double) XYgrid{get; private set;} //число узлов сетки и шаг сетки по оси Ox; число узлов и шаг сетки по оси Oy;

    public int Count{
        get{ return XYgrid.Item1 * XYgrid.Item3;}
    }

    public V2JDataArray(string str, (int, double, int, double) v){
        Key = str;
        XYgrid = v;

        mas = new double[XYgrid.Item1][];
        for (int i = 0; i < XYgrid.Item1; i++) mas[i] = new double[XYgrid.Item3 * 2];
        Random rand = new Random();
        for (int i = 0; i < XYgrid.Item1; i++){
            for (int j = 0; j < (XYgrid.Item3 * 2); j++)
                mas[i][j] = rand.NextDouble() * 50;
        }
    }

    public override string ToString() => $"Key: {Key} | XYgrid.Item1: {XYgrid.Item1} XYgrid.Item2: {XYgrid.Item2:F3} | XYgrid.Item3: {XYgrid.Item3} XYgrid.Item4: {XYgrid.Item4:F3}";

    public string ToLongString(){
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(ToString());
        for (int i = 0; i < XYgrid.Item1; i++){
            int ind2 = 0;
            for (int j = 0; j < (XYgrid.Item3 * 2); j += 2){
                sb.Append($"[{i * XYgrid.Item2}:{ind2 * XYgrid.Item4}] [{i}:{ind2}]{mas[i][j]:F3}, {mas[i][j + 1]:F3} | ");
                ind2 += 1;}
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public (double, int, int) MaxDifference{
        get{
            double max = 0;
            int ind1 = 0;
            int ind2 = 0;
            for (int i = 0; i < XYgrid.Item1; i++){
                for (int j = 0; j < (XYgrid.Item3 * 2); j += 2){
                    if ((Math.Abs(mas[i][j] - mas[i][j + 1])) > max){
                      max = Math.Abs(mas[i][j] - mas[i][j + 1]);
                      ind1 = i;
                      ind2 = j;
                    }
                }
            }
            return (max, ind1, ind2);
        }
    }

    public (double, double) this[int x, int y]{
        get{
            if ((x >= XYgrid.Item1) || (y >= XYgrid.Item3) || (x < 0) || (y < 0)) return (double.NaN, double.NaN); 
            return (mas[x][y * 2], mas[x][y * 2 + 1]);
        }
        set{
            if ((x < XYgrid.Item1) && (y < XYgrid.Item3) && (x >= 0) && (y >= 0)){
                mas[x][y * 2] = value.Item1;
                mas[x][y * 2 + 1] = value.Item2;
        }
        }
    }
    }
}