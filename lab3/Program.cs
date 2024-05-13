using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
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
            
            Task1(myCars);
            
            SerializeCars("cars.xml", myCars);
            List<Car> deserializedCars = DeserializeCars("cars.xml");
            
            Console.WriteLine("\n--------------Task2--------------");
            foreach (var car in deserializedCars)
            {
                Console.WriteLine($"Model: {car.Model}, Year: {car.Year}, Engine: {car.Motor.Model}");
            }
            
            Console.WriteLine("\n--------------Task3--------------");
            XElement rootNode = XElement.Load("cars.xml");
            double avgHP = (double) rootNode.XPathEvaluate("sum(//car[engine/@model != 'TDI']/engine/horsePower) div count(//car[engine/@model != 'TDI']/engine)");
            Console.WriteLine("avg HP: "+ avgHP);
            
            IEnumerable<string> models = rootNode.XPathSelectElements("//car/model").Select(x => x.Value).Distinct();
            Console.WriteLine("\nCar Models:");
            foreach (var model in models)
            {
                Console.WriteLine(model);
            }
        }

        static void Task1(List<Car> myCars)
        {
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
            
            Console.WriteLine("\n--------------Task1--------------");
            foreach (var result in query2)
            {
                Console.WriteLine($"{result.Model}: {result.AvgHPPL}");
            }
        }
        
        

        static void SerializeCars(string filePath, List<Car> cars)
        {
            XElement carsXml = new XElement("cars",
                from car in cars
                select new XElement("car",
                    new XElement("model", car.Model),
                    new XElement("engine",
                        new XAttribute("model", car.Motor.Model),
                        new XElement("displacement", car.Motor.Displacement),
                        new XElement("horsePower", car.Motor.HorsePower)
                    ),
                    new XElement("year", car.Year)
                )
            );
            carsXml.Save(filePath);
        }
        static List<Car> DeserializeCars(string filePath)
        {
            XElement root = XElement.Load(filePath);

            List<Car> cars = (
                from carElement in root.Elements("car")
                select new Car(
                    (string)carElement.Element("model"),
                    new Engine(
                        (double)carElement.Element("engine").Element("displacement"),
                        (int)carElement.Element("engine").Element("horsePower"),
                        (string)carElement.Element("engine").Attribute("model")
                    ),
                    (int)carElement.Element("year")
                )
            ).ToList();

            return cars;
        }
    }
}