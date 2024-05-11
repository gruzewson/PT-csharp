using lab3;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            
            List<Car> myCars = new List<Car>(){
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

            var query1 = 
                from car in myCars
                where car.Model == "A6"
                let engineType = car.Motor.Model == "TDI" ? "diesel" : "petrol"
                let h = car.Motor.HorsePower / car.Motor.Displacement
                select new { model = engineType, hppl = h };
            
            var query2 = 
                from item in query1
                group item by item.model into grouped
                select new
                {
                    Model = grouped.Key,
                    AvgHPPL = grouped.Average(x => x.hppl)
                };
            
            foreach (var result in query2)
            {
                Console.WriteLine($"{result.Model}: {result.AvgHPPL}");
            }

        }
    }
}