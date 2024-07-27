using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact_List
{
    class Contact
    {
        //using a constructor to establish the values in the class.
        public Contact(string name, string number)//,star)
        {
            Name = name;
            Number = number;
            //Star = star
        }
        public string Name { get; set; }
        public string Number { get; set; }  //attribues in the class

        //public string Star { get; set; }
    }
}
 //public string Star { get; set; }

//demonstrating polymorphism however it disrupts my code but this is an attempt

//class StarContact : Contact
//{
//    public string StarLevel { get; set; }
//
//    public satrContact(string name, string number, string starLevel)
//        : base(name, number)
//    {
//        StarLevel = starLevel;
//    }
//
//    //override the DisplayContact to include starlevel
//    public override void DisplayContact()
//    {
//        Console.WriteLine($"star Contact: {Name}, {Number}, star Level: {StarLevel}");
//    }
//not working for some reason but I have attempted it.