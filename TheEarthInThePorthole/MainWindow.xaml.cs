using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheEarthInThePorthole
{
    public class Circle //создаём класс и приватные радиус, длину, площадь и координаты центра
    {
        private double radius { get; set; }
        private double length { get; set; }
        private double area { get; set; }
        private coord center { get; set; }

        public Circle()
        {
            radius = 0;
            length = 0;
            area = 0;
            center = new coord(0, 0);
        }

        public void AddCircle(int op, double val, coord c) //метод для добавления
        {
            center = c;
            switch(op)
            {
                case 0:
                    radius = val;
                    length = 2 * Math.PI * radius;
                    area = Math.PI * (radius * radius);
                    break;
                case 1:
                    length = val;
                    radius = length / (2 * Math.PI);
                    area = Math.PI * (radius * radius);
                    break;
                case 2:
                    area = val;
                    radius = Math.Sqrt((area / Math.PI));
                    length = 2 * Math.PI * radius;
                    break;
            }
        }

        public string Info() //метод для сбора информации
        {
            return "Радиус равен "+Math.Round(radius, 3).ToString()+", длина окружности равна "+Math.Round(length, 3).ToString()+", площадь равна "+Math.Round(area, 3).ToString()+", координаты центра: X равен "+ Math.Round(center.X, 3).ToString()+", Y равен "+ Math.Round(center.Y, 3).ToString();
        }
        public bool IsInto(double x, double y) 
        { 
            return radius*radius > Math.Pow(x-center.X,2) + Math.Pow(y - center.Y, 2); 
        }
    }

    public class coord //создаём класс для координат - это упростит нам работу в будущем
    {
        public double X { get; private set; }
        public double Y { get; set; }

        public coord(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] separatingStrings = { " ", ";", "; "};
        static void Resize(ref Circle[] array, int newSize) //сделаем функцию, которая нам поможет в дальнейшем, т.к. нормальных стандартных решений я не нашёл
        {
            Circle[] newArray = new Circle[newSize];

            for (int i = 0; i < newSize - 1; i++)
            {
                newArray[i] = array[i];
            }
            array = newArray;
        }

        int number = 0;

        Circle[] Clown = new Circle[1]; //создаём массив объектов класса

        public MainWindow()
        {
            InitializeComponent();
        }

        private void B_ADD_Click(object sender, RoutedEventArgs e)
        {
            Array.Resize(ref Clown, number+1);
            try //обрабатывам исключения, на всякий случай
            {
                string[] crd = TB_C.Text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                double a = double.Parse(crd[0]);
                double b = double.Parse(crd[1]);
                string[] xar = TB_IN.Text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                double c = double.Parse(xar[0]);

                if (crd.Length > 2 || xar.Length > 1)
                {
                    throw new Exception("Алё, так нельзя! Многовато циферок!");
                }
                if (c < 0)
                {
                    throw new Exception("Алё, так нельзя! Отрицательный радиус!");
                }
            }
            catch (Exception ex)
            {
                TB_XAR.Text = "";
                TB_XAR.Text += "Неверный формат данных! " + "(" + ex.Message + ")";
                return;
            }

            if (CB_IN.SelectedIndex == -1)
            {
                MessageBox.Show("А всё-таки надо что-то выбрать!");
                return;
            }

            string[] coordinates = TB_C.Text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries); //теперь всё создаём, заполняем и т.д.
            double x = double.Parse(coordinates[0]);
            double y = double.Parse(coordinates[1]);

            Clown[number] = new Circle();
            Clown[number].AddCircle(CB_IN.SelectedIndex, double.Parse(TB_IN.Text), new coord(x, y));
            CB_CCH.Items.Add((number+1).ToString());
            CB_CCH.IsEnabled = true;
            TB_XAR.TextAlignment = TextAlignment.Left;
            TB_XAR.Text = Clown[number].Info();
            number++;
        }

        int check = 0;

        private void B_INF_Click(object sender, RoutedEventArgs e) //обрабатываем информационную кнопку, в том числе и приверяем на имключения
        {
            if (CB_ACT.SelectedIndex == -1)
            {
                MessageBox.Show("Надо выбрать действие!");
                return;
            }
            if (CB_CCH.SelectedIndex == -1)
            {
                MessageBox.Show("Надо выбрать номер добавленного в базу круга!");
                return;
            }

            TB_OUT.TextAlignment = TextAlignment.Left;
            if (CB_ACT.SelectedIndex == 1)
            {
                TB_OUT.Text = "";
                TB_OUT.IsReadOnly = false;
                TB_OUT.TextAlignment = TextAlignment.Center;
                TB_OUT.Text += "Введите координаты точки (с любым разделителем!) здесь и нажмите ENTER!";
                check = 2;
            }
            else
            {
                TB_OUT.Text = Clown[CB_CCH.SelectedIndex].Info();
                TB_OUT.IsReadOnly = true;
            }

        }

        private void TB_OUT_GotFocus(object sender, RoutedEventArgs e) //здесь мы продолжаем обрабатывать поле текстбокса, хитрой перемнной контролируя этап
        {
            if (check == 2)
            {
                TB_OUT.TextAlignment = TextAlignment.Left;
                TB_OUT.Text = "";
                check = 1;
            }              
        }

        private void TB_OUT_KeyDown(object sender, KeyEventArgs e) //продолжаем
        {
            if ((e.Key == Key.Enter) && (check == 1))
            {
                try
                {
                    string[] crd = TB_OUT.Text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                    double a = double.Parse(crd[0]);
                    double b = double.Parse(crd[1]);

                    if (crd.Length > 2)
                    {
                        throw new Exception("Алё, так нельзя! Многовато координат!");
                    }
                }
                catch (Exception ex)
                {
                    TB_OUT.Text = "";
                    TB_OUT.Text += "Неверный формат данных! " +"("+ex.Message+")";
                    TB_OUT.IsReadOnly = true;
                    return;
                }

                string[] coordcheck = TB_OUT.Text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                TB_OUT.Text = "";
                TB_OUT.TextAlignment = TextAlignment.Center;
                if (Clown[CB_CCH.SelectedIndex].IsInto(double.Parse(coordcheck[0]), double.Parse(coordcheck[1])))
                {
                    TB_OUT.Text += "Ваша точка находится ВНУТРИ круга.";
                    TB_OUT.IsReadOnly = true;
                }
                else
                {
                    TB_OUT.Text += "Ваша точка находится ВНЕ круга.";
                    TB_OUT.IsReadOnly = true;
                }
                check = 0;
            }
        }

        private void TB_IN_GotFocus(object sender, RoutedEventArgs e) //наводим красоту
        {
            TB_IN.Text = "";
            if (number > 0)
            {
                TB_XAR.Text = "";
            }          
        }

        private void TB_C_GotFocus(object sender, RoutedEventArgs e) //тоже наводим красоту
        {
            TB_C.Text = "";
            if (number > 0)
            {
                TB_XAR.Text = "";
            }
        }
    }
}