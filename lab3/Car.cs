using System.Xml.Serialization;
using lab3;

namespace Lab3
{
    public class Car
    {
        public Car(string model, Engine motor, int year)
        {
            this.Model = model;
            this.Motor = motor;
            this.Year = year;
        }

        public Car()
        {
        }

        private string model;
        
        //[XmlElement("Engine")]
        private Engine motor;
        
        private int year;

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public Engine Motor
        {
            get { return motor; }
            set { motor = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }
    }
}