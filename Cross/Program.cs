using System;

namespace Cross
{
    class Program
    {
        static void Main(string[] args)
        {
            MenuClass menu=new MenuClass();
            menu.DrawMenu();
            menu.SelectMenu();
            




            //FieldClass field=new FieldClass(5,5,'X','O');
            //field.DrawField();
            //Console.ReadLine();
        }
    }
}
