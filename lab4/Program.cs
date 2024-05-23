using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace lab4
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var elementsQuery = 
                from car in SharedData.myCars
                where car.Model == "A6"
                let engineType = car.Motor.Model == "TDI" ? "diesel" : "petrol"
                group car by engineType into g
                select new
                {
                    engineType = g.Key,
                    avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement)
                } into result
                orderby result.avgHPPL descending
                select result;


            Console.WriteLine("---------Task 1.1---------");
            foreach (var e in elementsQuery)
            {
                Console.WriteLine($"{e.engineType}: {e.avgHPPL}");
            }
            
            var elementsMethod = SharedData.myCars
                .Where(car => car.Model == "A6")
                .GroupBy(car => car.Motor.Model == "TDI" ? "diesel" : "petrol")
                .Select(g => new
                {
                    engineType = g.Key,
                    avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement)
                })
                .OrderByDescending(result => result.avgHPPL);

            
            Console.WriteLine("---------Task 1.2---------");
            foreach (var e in elementsMethod)
            {
                Console.WriteLine($"{e.engineType}: {e.avgHPPL}");
            }
            
            //task 2
            Func<Car, Car, int> arg1 = Func;
            Predicate<Car> arg2 = Predicate;
            Action<Car> arg3 = Action;
            SharedData.myCars.Sort(new Comparison<Car>(arg1));
            SharedData.myCars.FindAll(arg2).ForEach(arg3);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        
        private static int Func(Car car, Car b)
        {
            if (car.Motor.HorsePower > b.Motor.HorsePower)
            {
                return 1;
            }

            if (car.Motor.HorsePower < b.Motor.HorsePower)
            {
                return -1;
            }

            return 0;
        }

        private static bool Predicate(Car a)
        {
            return a.Motor.Model == "TDI";
        }

        private static void Action(Car a)
        {
            MessageBox.Show("2. Model: " + a.Model + " Silnik: " + a.Motor + " Rok: " + a.Year);
        }
        
        private static void Sort<T>(this BindingList<T> list, Comparison<T> comparison)
        {
            List<T> sortableList = list.ToList();
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++)
            {
                list[i] = sortableList[i];
            }
        }

        private static List<T> FindAll<T>(this BindingList<T> list, Predicate<T> match)
        {
            return list.Where(item => match(item)).ToList();
        }
    }

    public static class SharedData
    {
        public static BindingList<Car> myCars = new BindingList<Car>
        {
            new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
            new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
            new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
            new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
            new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
            new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
            new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
            new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
            new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
        };
    }
    
    
}