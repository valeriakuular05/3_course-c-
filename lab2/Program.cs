using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Variant_2_labs_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа 2. Вариант 2 ===\n");
            // 1. Создать объект типа V2JDataArray, вывести его данные с помощью метода ToLongString()
            Console.WriteLine("1. V2JDataArray:");
            var grid = (3, 1.3, 2, 1.2); // 3 узла по X с шагом 1.3, 2 узла по Y с шагом 1.2
            var dataArray = new V2JDataArray("data_array_1", grid, Funcs.Function1);
            Console.WriteLine(dataArray.ToLongString());
            
            // 2. Вывести все данные объекта V2JDataArray через IEnumerable<DataItemD>
            Console.WriteLine("2. Данные V2JDataArray через IEnumerable:");
            foreach (var item in dataArray)
            {
                Console.WriteLine($"   {item}");
            }
            Console.WriteLine($"   Key: {dataArray.Key}");
            Console.WriteLine($"   Count: {dataArray.Count}");
            Console.WriteLine($"   MaxDifference: {dataArray.MaxDifference:F4}");
            var farthest1 = dataArray.Farthest(0, 0);
            Console.WriteLine($"   Farthest from (0,0): {farthest1}");
            Console.WriteLine();

            // 3. Создать объект типа V2JList, вывести его данные
            Console.WriteLine("3. V2JList:");
            var list = new V2JList("list_1", grid, Funcs.Function1, 2, Funcs.GenerateListData);
            Console.WriteLine(list.ToLongString());
            
            // 4. Вывести все данные объекта V2JList через IEnumerable<DataItemD>
            Console.WriteLine("4. Данные V2JList через IEnumerable:");
            foreach (var item in list)
            {
                Console.WriteLine($"   {item}");
            }
            Console.WriteLine($"   Key: {list.Key}");
            Console.WriteLine($"   Count: {list.Count}");
            Console.WriteLine($"   MaxDifference: {list.MaxDifference:F4}");
            var farthest2 = list.Farthest(1, 1);
            Console.WriteLine($"   Farthest from (1,1): {farthest2}");
            Console.WriteLine();

            // 5. Создать объект типа V2JDict, вывести его данные
            Console.WriteLine("5. V2JDict:");
            var dict = new V2JDict("dict_1", grid, Funcs.Function2, 2, Funcs.GenerateDictData);
            Console.WriteLine(dict.ToLongString());
            
            // 6. Вывести все данные объекта V2JDict через IEnumerable<DataItemD>
            Console.WriteLine("6. Данные V2JDict через IEnumerable:");
            foreach (var item in dict)
            {
                Console.WriteLine($"   {item}");
            }
            Console.WriteLine($"   Key: {dict.Key}");
            Console.WriteLine($"   Count: {dict.Count}");
            Console.WriteLine($"   MaxDifference: {dict.MaxDifference:F4}");
            var farthest3 = dict.Farthest(2, 2);
            Console.WriteLine($"   Farthest from (2,2): {farthest3}");
            Console.WriteLine();

            // 7. Создать массив типа IDataInfo[3]
            Console.WriteLine("7. Массив IDataInfo:");
            IDataInfo[] dataInfos = new IDataInfo[3];
            dataInfos[0] = dataArray;
            dataInfos[1] = list;
            dataInfos[2] = dict;

            for (int i = 0; i < dataInfos.Length; i++)
            {
                Console.WriteLine($"   Элемент {i}:");
                Console.WriteLine($"   Key: {dataInfos[i].Key}");
                Console.WriteLine($"   Count: {dataInfos[i].Count}");
                Console.WriteLine($"   MaxDifference: {dataInfos[i].MaxDifference:F4}");
                var farthest = dataInfos[i].Farthest(0.5, 0.5);
                Console.WriteLine($"   Farthest from (0.5,0.5): {farthest}");
                Console.WriteLine();
            }
            V2JDataArray d = list;
            foreach (var item in d)
                Console.WriteLine(item);
            Console.WriteLine(d.Count);
        }
    }
}

public struct DataItemD{
    public double Y{get; set;}
    public double X{get; set;}
    public (double, double) data{get; set;}

    public DataItemD(double x, double y, (double, double) v){
        Y = y; X = x; data = v;
    }

    public override string ToString() =>  $"X: {X:F3} | Y: {Y:F3} | data: {data.Item1:F3} {data.Item2:F3}";
}

public class V2JDataArray: IDataInfo, IEnumerable<DataItemD>
{
    protected (double, double)[][] mas;
    public virtual string Key{get; set;}
    public (int, double, int, double) XYgrid{get; private set;}

    public V2JDataArray(string key, (int, double, int, double) XYgrid, 
                        System.Func<double, double, (double, double)>FJ)
    {
        Key = key;
        this.XYgrid = XYgrid;
        mas = new (double, double)[XYgrid.Item1][];
        for (int i = 0; i < XYgrid.Item1; i++){
            mas[i] = new (double, double)[XYgrid.Item3];

            for (int j = 0; j < XYgrid.Item3; j++){
                double x_ = i * XYgrid.Item2;
                double y_ = j * XYgrid.Item4;
                mas[i][j] = FJ(x_, y_);
            };
        }
    }

    public override string ToString() => $"Key: {Key} | X_count: {XYgrid.Item1} X_step: {XYgrid.Item2:F3} \n Y_count: {XYgrid.Item3} Y_step: {XYgrid.Item4:F3}";
    public virtual string ToLongString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(ToString());
        for (int i = 0; i < XYgrid.Item1; i++)
        {
            for (int j = 0; j < (XYgrid.Item3); j++)
                sb.Append($"({i}:{j}) (x={i * XYgrid.Item2:F3} y={j * XYgrid.Item4:F3}) ({mas[i][j].Item1:F3}, {mas[i][j].Item2:F3}) |");
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public virtual int Count{
        get{ return XYgrid.Item1 * XYgrid.Item3;}
    }

    public virtual double MaxDifference{
        get{
            double max = Math.Abs(mas[0][0].Item1 - mas[0][0].Item2);
            for (int i = 0; i < XYgrid.Item1; i++){
                for (int j = 0; j < XYgrid.Item3; j++){
                    if (Math.Abs(mas[i][j].Item1 - mas[i][j].Item2) > max)
                        max = Math.Abs(mas[i][j].Item1 - mas[i][j].Item2);
                }
            }
            return max;
        }
    }

    public virtual DataItemD Farthest (double x, double y){
        double dist = 0;
        DataItemD farthestItem = default;
        for (int i = 0; i < XYgrid.Item1; i++){
            for (int j = 0; j < XYgrid.Item3; j++){
                double d = Math.Sqrt(Math.Pow(i * XYgrid.Item2 - x, 2) + Math.Pow(j * XYgrid.Item4 - y, 2));
                if (d > dist){
                    dist = d;
                    farthestItem = new DataItemD(i * XYgrid.Item2, j * XYgrid.Item4, mas[i][j]);
                }
            }
        }
        return farthestItem;
    }

    public virtual IEnumerator<DataItemD> GetEnumerator(){
        for (int i = 0; i < XYgrid.Item1; i++){
            for (int j = 0; j < XYgrid.Item3; j++){
                double x = i * XYgrid.Item2;
                double y = j * XYgrid.Item4;
                yield return new DataItemD(x, y, mas[i][j]);
            }
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

interface IDataInfo{
    string Key {get; set;}
    int Count {get;}
    double MaxDifference {get;}
    DataItemD Farthest (double x, double y);
}

class V2JList: V2JDataArray, IDataInfo, IEnumerable<DataItemD>
{
    private List<DataItemD> lst;
    private int nList;
    
    public V2JList(string key, (int, double, int, double) XYgrid, 
                            System.Func<double, double, (double, double)> FJ, 
                            int nList, System.Func<int,DataItemD> FL): base(key, XYgrid, FJ)
        {     
        this.nList = nList;
        lst = new List<DataItemD>();
        for (int i = 0; i < nList; i++){
            DataItemD tmp = FL(i);
            lst.Add(tmp);
        }
    }
    
    public void Add(DataItemD item){
        lst.Add(item);
    }
    
    public override string ToString()=>$"Key: {Key}, X_count: {XYgrid.Item1}, X_step: {XYgrid.Item2:F3}, Y_count: {XYgrid.Item3}, Y_step: {XYgrid.Item4:F3}, nList: {nList}";
    
    public override string ToLongString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(ToString());
        sb.AppendLine();
        
        // Grid data from base class
        for (int i = 0; i < XYgrid.Item1; i++)
        {
            for (int j = 0; j < XYgrid.Item3; j++)
                sb.Append($"({i}:{j}) (x={i * XYgrid.Item2:F3} y={j * XYgrid.Item4:F3}) ({mas[i][j].Item1:F3},{mas[i][j].Item2:F3}) | ");
            sb.AppendLine();
        }
        
        // Additional list data
        sb.AppendLine("Additional List Data:");
        int index = 0;
        foreach (var item in lst)
        {
            sb.AppendLine($"[Additional Point {index++}]: {item}");
        }
        return sb.ToString();
    }
    
    public override int Count {
        get{
            return lst.Count + XYgrid.Item1 * XYgrid.Item3;
        }
    }
    
    public override double MaxDifference 
    {
        get{
            double max = base.MaxDifference;
            foreach (var item in lst){
                if ((Math.Abs(item.data.Item1 - item.data.Item2)) > max) 
                    max = Math.Abs(item.data.Item1 - item.data.Item2); 
            }
            return max;     
        }
    }
    
    public override DataItemD Farthest (double x, double y){
        double dist = 0;
        DataItemD farthestItem = default;
        
        // Check grid points
        for (int i = 0; i < XYgrid.Item1; i++){
            for (int j = 0; j < XYgrid.Item3; j++){
                double d = Math.Sqrt(Math.Pow(i * XYgrid.Item2 - x, 2) + Math.Pow(j * XYgrid.Item4 - y, 2));
                if (d > dist) 
                {
                    dist = d;
                    farthestItem = new DataItemD(i * XYgrid.Item2, j * XYgrid.Item4, mas[i][j]);
                }
            }
        }
        
        // Check list points
        foreach (var item in lst)
        {
            double d = Math.Sqrt(Math.Pow(item.X - x, 2) + Math.Pow(item.Y - y, 2));
            if (d > dist)
            {
                dist = d;
                farthestItem = item;
            }
        }
        return farthestItem;
    }
    
    public override IEnumerator<DataItemD> GetEnumerator(){
        // Grid points
        for (int i = 0; i < XYgrid.Item1; i++){
            for (int j = 0; j < XYgrid.Item3; j++){
                double x = i * XYgrid.Item2;
                double y = j * XYgrid.Item4;
                yield return new DataItemD(x, y, mas[i][j]);
            }
        }
        
        // List points
        foreach (var item in lst)
            yield return item;
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class V2JDict: IDataInfo, IEnumerable<DataItemD>
{
    private V2JDataArray baseArray;
    private Dictionary <(double, double), DataItemD> dict;
    private int nDict;

    public V2JDict (string key, (int, double, int, double) XYgrid, 
                    System.Func<double, double, (double, double)> FJ, 
                    int nDict, System.Func<int, ((double, double), DataItemD)> FD)
    {
        Key = key;
        this.nDict = nDict;
        baseArray = new V2JDataArray(key, XYgrid, FJ);
        dict = new Dictionary<(double, double), DataItemD>();
        for (int i = 0; i < nDict; i++)
        {
            var pair = FD(i);
            dict[pair.Item1] = pair.Item2;
        }
    }
    
    public string Key {get; set;}
    
    public void Add(DataItemD item){
        dict[(item.X, item.Y)] = item;
    }
    
    public override string ToString() => $"Key: {Key}, X_count: {baseArray.XYgrid.Item1}, X_step: {baseArray.XYgrid.Item2:F3}, Y_count: {baseArray.XYgrid.Item3}, Y_step: {baseArray.XYgrid.Item4:F3}, nDict: {nDict}";

    public string ToLongString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(ToString());
        sb.AppendLine("Grid Data:");
        sb.AppendLine(baseArray.ToLongString());
        sb.AppendLine("Additional Dictionary Data:");
        int index = 0;
        foreach (var item in dict.Values)
        {
            sb.AppendLine($"[Additional Point {index++}]: {item}");
        }
        return sb.ToString();
    }
    
    public int Count {
        get {
            return baseArray.Count + dict.Count;
        }
    }
    
    public double MaxDifference {
        get {
            double max = baseArray.MaxDifference;
            foreach (var item in dict.Values)
            {
                double diff = Math.Abs(item.data.Item1 - item.data.Item2);
                if (diff > max)
                    max = diff;
            }
            return max;
        }
    }
    
    public DataItemD Farthest(double x, double y)
    {
        double maxDist = 0;
        DataItemD farthestItem = default;
        
        foreach (var gridItem in baseArray)
        {
            double d = Math.Sqrt(Math.Pow(gridItem.X - x, 2) + Math.Pow(gridItem.Y - y, 2));
            if (d > maxDist)
            {
                maxDist = d;
                farthestItem = gridItem;
            }
        }
        
        foreach (var dictItem in dict.Values)
        {
            double d = Math.Sqrt(Math.Pow(dictItem.X - x, 2) + Math.Pow(dictItem.Y - y, 2));
            if (d > maxDist)
            {
                maxDist = d;
                farthestItem = dictItem;
            }
        }
        return farthestItem;
    }
    
    public IEnumerator<DataItemD> GetEnumerator()
    {
        foreach (var item in baseArray)
        {
            yield return item;
        }
        foreach (var item in dict.Values)
        {
            yield return item;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public static class Funcs
{
    public static (double, double) Function1(double x, double y)
    {
        return (x * y, x + y);
    }
    
    public static (double, double) Function2(double x, double y)
    {
        return (Math.Sin(x) * Math.Cos(y), Math.Cos(x) * Math.Sin(y));
    }
    
    public static DataItemD GenerateListData(int index)
    {
        double x = 0.5 + index * 0.3;
        double y = 0.7 + index * 0.4;
        return new DataItemD(x, y, (x * y, x + y));
    }
    
    public static ((double, double), DataItemD) GenerateDictData(int index)
    {
        double x = 1.2 + index * 0.5;
        double y = 0.8 + index * 0.6;
        var dataItem = new DataItemD(x, y, (Math.Sin(x), Math.Cos(y)));
        return ((x, y), dataItem);
    }
}



























// var grid2 = (3, 1.3, 2, 1.2);
            // var dl = new V2JList("list_new", grid2, Funcs.Function1, 2, Funcs.GenerateListData);
            // V2JDataArray da2 = dl;
            // foreach (var point in da2)
            //     Console.WriteLine(point);








// using System;
// using System.Text;
// using System.Collections;
// using System.Collections.Generic;
// namespace Variant_2_labs_2
// {
//     class Program
//     {
//         static void Main(string[] args)
//         {
//             Console.WriteLine("=== Лабораторная работа 2. Вариант 2 ===\n");

//             // 1. Создать объект типа V2JDataArray, вывести его данные с помощью метода ToLongString()
//             Console.WriteLine("1. V2JDataArray:");
//             var grid = (3, 1.3, 2, 1.2); // 3 узла по X с шагом 1.0, 2 узла по Y с шагом 1.5
//             var dataArray = new V2JDataArray("data_array_1", grid, Funcs.Function1);
//             Console.WriteLine(dataArray.ToLongString());
            
//             // 2. Вывести все данные объекта V2JDataArray через IEnumerable<DataItemD>
//             Console.WriteLine("2. Данные V2JDataArray через IEnumerable:");
//             foreach (var item in dataArray)
//             {
//                 Console.WriteLine($"   {item}");
//             }
//             Console.WriteLine($"   Key: {dataArray.Key}");
//             Console.WriteLine($"   Count: {dataArray.Count}");
//             Console.WriteLine($"   MaxDifference: {dataArray.MaxDifference:F4}");
//             var farthest1 = dataArray.Farthest(0, 0);
//             Console.WriteLine($"   Farthest from (0,0): {farthest1}");
//             Console.WriteLine();

//             // 3. Создать объект типа V2JList, вывести его данные
//             Console.WriteLine("3. V2JList:");
//             var list = new V2JList("list_1", grid, Funcs.Function1, 2, Funcs.GenerateListData);
//             Console.WriteLine(list.ToLongString());
            
//             // 4. Вывести все данные объекта V2JList через IEnumerable<DataItemD>
//             Console.WriteLine("4. Данные V2JList через IEnumerable:");
//             foreach (var item in list)
//             {
//                 Console.WriteLine($"   {item}");
//             }
//             Console.WriteLine($"   Key: {list.Key}");
//             Console.WriteLine($"   Count: {list.Count}");
//             Console.WriteLine($"   MaxDifference: {list.MaxDifference:F4}");
//             var farthest2 = list.Farthest(1, 1);
//             Console.WriteLine($"   Farthest from (1,1): {farthest2}");
//             Console.WriteLine();

//             // 5. Создать объект типа V2JDict, вывести его данные
//             Console.WriteLine("5. V2JDict:");
//             var dict = new V2JDict("dict_1", grid, Funcs.Function2, 2, Funcs.GenerateDictData);
//             Console.WriteLine(dict.ToLongString());
            
//             // 6. Вывести все данные объекта V2JDict через IEnumerable<DataItemD>
//             Console.WriteLine("6. Данные V2JDict через IEnumerable:");
//             foreach (var item in dict)
//             {
//                 Console.WriteLine($"   {item}");
//             }
//             Console.WriteLine($"   Key: {dict.Key}");
//             Console.WriteLine($"   Count: {dict.Count}");
//             Console.WriteLine($"   MaxDifference: {dict.MaxDifference:F4}");
//             var farthest3 = dict.Farthest(2, 2);
//             Console.WriteLine($"   Farthest from (2,2): {farthest3}");
//             Console.WriteLine();

//             // 7. Создать массив типа IDataInfo[3]
//             Console.WriteLine("7. Массив IDataInfo:");
//             IDataInfo[] dataInfos = new IDataInfo[3];
//             dataInfos[0] = dataArray;
//             dataInfos[1] = list;
//             dataInfos[2] = dict;

//             for (int i = 0; i < dataInfos.Length; i++)
//             {
//                 Console.WriteLine($"   Элемент {i}:");
//                 Console.WriteLine($"   Key: {dataInfos[i].Key}");
//                 Console.WriteLine($"   Count: {dataInfos[i].Count}");
//                 Console.WriteLine($"   MaxDifference: {dataInfos[i].MaxDifference:F4}");
//                 var farthest = dataInfos[i].Farthest(0.5, 0.5);
//                 Console.WriteLine($"   Farthest from (0.5,0.5): {farthest}");
//                 Console.WriteLine();
//             }
//         }
//     }
// }

// public struct DataItemD{
//     public double Y{get; set;}
//     public double X{get; set;}
//     public (double, double) data{get; set;}

//     public DataItemD(double x, double y, (double, double) v){
//         Y = y; X = x; data = v;
//     }

//     public override string ToString() =>  $"X: {X} | Y: {Y} | data: {data.Item1:F3} {data.Item2:F3}";
// }

// public class V2JDataArray: IDataInfo, IEnumerable<DataItemD>
// {
//     protected (double, double)[][] mas;
//     public virtual string Key{get; set;}
//     public (int, double, int, double) XYgrid{get; private set;}

//     public V2JDataArray(string key, (int, double, int, double) XYgrid, 
//                         System.Func<double, double, (double, double)>FJ)
//     {
//         Key = key;
//         this.XYgrid = XYgrid;
//         mas = new (double, double)[XYgrid.Item1][];
//         for (int i = 0; i < XYgrid.Item1; i++){
//             mas[i] = new (double, double)[XYgrid.Item3];

//             for (int j = 0; j < XYgrid.Item3; j++){
//                 double x_ = i * XYgrid.Item2;
//                 double y_ = j * XYgrid.Item4;
//                 mas[i][j] = FJ(x_, y_);
//             };
//         }
//     }

//     public override string ToString() => $"Key: {Key} | X_count: {XYgrid.Item1} X_step: {XYgrid.Item2:F3} \n Y_count: {XYgrid.Item3} Y_step: {XYgrid.Item4:F3}";
//     public virtual string ToLongString()
//     {
//         StringBuilder sb = new StringBuilder();
//         sb.AppendLine(ToString());
//         for (int i = 0; i < XYgrid.Item1; i++)
//         {
//             for (int j = 0; j < (XYgrid.Item3); j++)
//                 sb.Append($"({i}:{j}) (x={i * XYgrid.Item2} y={j * XYgrid.Item4}) ({mas[i][j].Item1:F3}, {mas[i][j].Item2:F3}) |");
//             sb.AppendLine();
//         }
//         return sb.ToString();
//     }

//     public virtual int Count{
//         get{ return XYgrid.Item1 * XYgrid.Item3;}
//     }

//     public virtual double MaxDifference{
//         get{
//             double max = Math.Abs(mas[0][0].Item1 - mas[0][0].Item2);
//             for (int i = 0; i < XYgrid.Item1; i++){
//                 for (int j = 0; j < XYgrid.Item3; j++){
//                     if (Math.Abs(mas[i][j].Item1 - mas[i][j].Item2) > max)
//                         max = Math.Abs(mas[i][j].Item1 - mas[i][j].Item2);
//                 }
//             }
//             return max;
//         }
//     }

//     public virtual DataItemD Farthest (double x, double y){
//         double dist = 0;
//         DataItemD farthestItem = default;
//         for (int i = 0; i < XYgrid.Item1; i++){
//             for (int j = 0; j < XYgrid.Item3; j++){
//                 double d = Math.Sqrt(Math.Pow(i * XYgrid.Item2 - x, 2) + Math.Pow(j * XYgrid.Item4 - y, 2));
//                 if (d > dist){
//                     farthestItem = new DataItemD(i * XYgrid.Item2, j * XYgrid.Item4, mas[i][j]);  //double x, double y, (double, double) v
//                 }
//             }
//         }
//         return farthestItem;
//     }

//     public virtual IEnumerator<DataItemD> GetEnumerator(){
//         for (int i = 0; i < XYgrid.Item1; i++){
//             for (int j = 0; j < XYgrid.Item3; j++){
//                 double x = i * XYgrid.Item2;
//                 double y = j * XYgrid.Item4;
//                 yield return new DataItemD(x, y, mas[i][j]);
//             }
//         }
//     }
//     IEnumerator IEnumerable.GetEnumerator()
//     {
//         return GetEnumerator();
//     }
// }

// interface IDataInfo{
//     string Key {get; set;}
//     int Count {get;}
//     double MaxDifference {get;}
//     DataItemD Farthest (double x, double y);
// }

// class V2JList: V2JDataArray, IDataInfo, IEnumerable<DataItemD>
// {
//     private List<DataItemD> lst;
//     private int nList;
//     public override string Key{get; set;}
//     public V2JList(string key, (int, double, int, double) XYgrid, 
//                             System.Func<double, double, (double, double)> FJ, 
//                             int nList, System.Func<int,DataItemD> FL)
//         {     
//         Key = key;   
//         this.nList = nList;
//         lst = new List<DataItemD>();
//         for (int i = 0; i< nList; i++){
//             DataItemD tmp = FL(i);
//             lst.Add(tmp);
//         }
//     }
//     public void Add(DataItemD item){
//         lst.Add(item);
//     }
//     public override string ToString()=>$"Key: {Key}, X_count: {XYgrid.Item1}, X_step: {XYgrid.Item2}, Y_count: {XYgrid.Item3}, Y_step: {XYgrid.Item4}, nList: {nList}";
//     public override string ToLongString()
//     {
//         StringBuilder sb = new StringBuilder();
//         sb.AppendLine(ToString());
//         sb.AppendLine();
//         sb.AppendLine(base.ToLongString());
//         // for (int i = 0; i < XYgrid.Item1; i++)
//         // {
//         //     for (int j = 0; j < (XYgrid.Item3); j++)
//         //         sb.Append($"({i}:{j}) (x={i * XYgrid.Item2} y={j * XYgrid.Item4}) ({mas[i][j].Item1:F3},{mas[i][j].Item2:F3}) | ");
//         //     sb.AppendLine();
//         // }
//         int index = 0;
//         foreach (var item in lst)
//         {
//             sb.AppendLine($"[Additional Point {index++}]: {item}");
//         }
//         return sb.ToString();
//     }
//     public override int Count {
//         get{
//             return lst.Count() + XYgrid.Item1 * XYgrid.Item3;
//         }
//     }
//     public override double MaxDifference 
//     {
//         get{
//             double max = base.MaxDifference;
//             foreach (var item in lst){
//                 if ((Math.Abs(item.data.Item1 - item.data.Item2)) > max) max = Math.Abs(item.data.Item1 - item.data.Item2); 
//             }
//             return max;     
//         }
//     }
//     public override DataItemD Farthest (double x, double y){
//         double dist = 0;
//         DataItemD farthestItem = default;
//         for (int i = 0; i < XYgrid.Item1; i++){
//             for (int j = 0; j < XYgrid.Item3; j++){
//                 double d = Math.Sqrt(Math.Pow(i * XYgrid.Item2 - x, 2) + Math.Pow(j * XYgrid.Item4 - y, 2));
//                 if (d > dist) farthestItem = new DataItemD(i * XYgrid.Item2, j * XYgrid.Item4, mas[i][j]);  //double x, double y, (double, double) v
//             }
//         }
//         foreach (var item in lst)
//         {
//             double d = Math.Sqrt(Math.Pow(item.X - x, 2) + Math.Pow(item.Y - y, 2));
//             if (d > dist)
//             {
//                 dist = d;
//                 farthestItem = item;
//             }
//         }
//         return farthestItem;
//     }
//     public override IEnumerator<DataItemD> GetEnumerator(){
//         for (int i = 0; i < XYgrid.Item1; i++){
//             for (int j = 0; j < XYgrid.Item3; j++){
//                 double x = i * XYgrid.Item1;
//                 double y = j * XYgrid.Item3;
//                 yield return new DataItemD(x, y, mas[i][j]);
//             }
//         }
//         foreach (var item in lst)
//             yield return item;
//     }
//     IEnumerator IEnumerable.GetEnumerator()
//     {
//         return GetEnumerator();
//     }
// }

// class V2JDict: IDataInfo, IEnumerable<DataItemD>
// {
//     private V2JDataArray baseArray;
//     private Dictionary <(double, double), DataItemD> dict;
//     private int nDict;

//     public V2JDict (string key, (int, double, int, double) XYgrid, 
//                     System.Func<double, double, (double, double)> FJ, 
//                     int nDict, System.Func<int, ((double, double), DataItemD)> FD){
//         this.nDict = nDict;
//         baseArray = new V2JDataArray(key, XYgrid, FJ);
//         dict = new Dictionary<(double, double), DataItemD>();
//         for (int i = 0; i < nDict; i++)
//         {
//             var pair = FD(i);
//             dict[pair.Item1] = pair.Item2;
//         }
//     }
//     public string Key {get; set;}
//     public void Add(DataItemD item){
//         dict[(item.X, item.Y)] = item;
//     }
//     public override string ToString() => $"Key: {Key}, X_count: {baseArray.XYgrid.Item1}, X_step: {baseArray.XYgrid.Item2}, Y_count: {baseArray.XYgrid.Item3}, Y_step: {baseArray.XYgrid.Item4}, nDict: {nDict}";

//     public string ToLongString()
//     {
//         StringBuilder sb = new StringBuilder();
//         sb.AppendLine(ToString());
//         sb.AppendLine("Grid Data:");
//         sb.AppendLine(baseArray.ToLongString());
//         sb.AppendLine("Additional Dictionary Data:");
//         int index = 0;
//         foreach (var item in dict.Values)
//         {
//             sb.AppendLine($"[Additional Point {index++}]: {item}");
//         }
//         return sb.ToString();
//     }
//     public int Count {
//         get {
//             return baseArray.Count + dict.Count;
//         }
//     }
//     public double MaxDifference {
//         get {
//             double max = baseArray.MaxDifference;
//             foreach (var item in dict.Values)
//             {
//                 double diff = Math.Abs(item.data.Item1 - item.data.Item2);
//                 if (diff > max)
//                     max = diff;
//             }
//             return max;
//         }
//     }
//     public DataItemD Farthest(double x, double y)
//     {
//         double maxDist = 0;
//         DataItemD farthestItem = default;
//         foreach (var gridItem in baseArray)
//         {
//             double d = Math.Sqrt(Math.Pow(gridItem.X - x, 2) + Math.Pow(gridItem.Y - y, 2));
//             if (d > maxDist)
//             {
//                 maxDist = d;
//                 farthestItem = gridItem;
//             }
//         }
//         foreach (var dictItem in dict.Values)
//         {
//             double d = Math.Sqrt(Math.Pow(dictItem.X - x, 2) + Math.Pow(dictItem.Y - y, 2));
//             if (d > maxDist)
//             {
//                 maxDist = d;
//                 farthestItem = dictItem;
//             }
//         }
//         return farthestItem;
//     }
//     public IEnumerator<DataItemD> GetEnumerator()
//     {
//         foreach (var item in baseArray)
//         {
//             yield return item;
//         }
//         foreach (var item in dict.Values)
//         {
//             yield return item;
//         }
//     }
    
//     IEnumerator IEnumerable.GetEnumerator()
//     {
//         return GetEnumerator();
//     }
// }


//     public static class Funcs
//     {
//         public static (double, double) Function1(double x, double y)
//         {
//             return (x * y, x + y);
//         }
//         public static (double, double) Function2(double x, double y)
//         {
//             return (Math.Sin(x) * Math.Cos(y), Math.Cos(x) * Math.Sin(y));
//         }
//         public static DataItemD GenerateListData(int index)
//         {
//             double x = 0.5 + index * 0.3;
//             double y = 0.7 + index * 0.4;
//             return new DataItemD(x, y, (x * y, x + y));
//         }
        
//         public static ((double, double), DataItemD) GenerateDictData(int index)
//         {
//             double x = 1.2 + index * 0.5;
//             double y = 0.8 + index * 0.6;
//             var dataItem = new DataItemD(x, y, (Math.Sin(x), Math.Cos(y)));
//             return ((x, y), dataItem);
//         }
//     }