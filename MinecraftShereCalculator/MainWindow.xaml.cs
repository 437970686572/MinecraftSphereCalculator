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

namespace MinecraftShereCalculator
{
    public partial class MainWindow : Window
    {
        private int Radius;
        private int With;
        private int Hight;
        private int BlockWith;
        private int CenterX;
        private int CenterY;
        private int CenterZ;
        List<List<int>> Wall;

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool IsInSphere(int x, int y, int z)
        {
            return GetRadiusToCenter(x, y, z) < Radius;
        }

        private Double GetRadiusToCenter(int x, int y, int z)
        {
            return Math.Sqrt(((CenterX - x) * (CenterX - x)) + ((CenterY - y) * (CenterY - y)) + ((CenterZ - z) * (CenterZ - z)));
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            if (Radius > 150)
            {
                MessageBox.Show("Der Radius darf höchstens 150 sein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            sliHight.Minimum = 0;
            sliHight.Maximum = Radius * 2 + 2;
            sliHight.Value = Radius + 1;
            lblGesBlockCount.Content = ((int)(4 / 3 * (Math.PI * (Radius * Radius * Radius)))).ToString();
            Calculate();
        }

        private void Calculate()
        {
            if (string.IsNullOrEmpty(txtRadius.Text))
            {
                return;
            }

            Radius = int.Parse(txtRadius.Text);
            With = Radius * 2 + 2;
            CenterX = Radius + 1;
            CenterY = Radius + 1;
            CenterZ = Radius + 1;
            Hight = (int)sliHight.Value;
            BlockWith = (int)(canOut.Height / With);
            FillWall();
            DrawSphere();
        }

        private void FillWall()
        {
            int BlockCount = 0;
            Wall = new List<List<int>>();
            for (int i = 0; i < With; i++)
            {
                Wall.Add(new List<int>());
                for (int j = 0; j < With; j++)
                {
                    if (IsInSphere(i, j, Hight))
                    {
                        Wall[i].Add(1);
                        BlockCount++;
                    }
                    else
                    {
                        Wall[i].Add(0);
                    }
                }
            }
            lblBlockCount.Content = BlockCount.ToString();
        }

        private bool IsEdgeOfCphere(int x, int y)
        {
            return Wall[x + 1][y] == 0 || Wall[x - 1][y] == 0 || Wall[x][y + 1] == 0 || Wall[x][y - 1] == 0;
        }

        private void DrawSphere()
        {
            canOut.Children.Clear();
            for (int i = 0; i < Wall.Count; i++)
            {
                for (int j = 0; j < Wall[i].Count; j++)
                {
                    Rectangle rect = new();
                    if (Wall[i][j] == 1)
                    {
                        if (IsEdgeOfCphere(i, j))
                        {
                            rect.Stroke = new SolidColorBrush(Colors.Red);
                            rect.Fill = new SolidColorBrush(Colors.LightCyan);

                        }
                        else
                        {
                            rect.Stroke = new SolidColorBrush(Colors.LightCyan);
                            rect.Fill = new SolidColorBrush(Colors.LightCyan);
                        }
                    }
                    else
                    {
                        rect.Stroke = new SolidColorBrush(Colors.Gray);
                        rect.Fill = new SolidColorBrush(Colors.Gray);
                    }

                    rect.Width = BlockWith;
                    rect.Height = BlockWith;
                    Canvas.SetLeft(rect, i * BlockWith);
                    Canvas.SetTop(rect, j * BlockWith);
                    canOut.Children.Add(rect);
                }
            }
        }

        private void TxtRadius_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtRadius.Text, out Radius) == false && txtRadius.Text.Length > 0)
            {
                txtRadius.Text = txtRadius.Text[..^1];
            }
        }

        private void sliHight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (txtHight != null)
            {
                txtHight.Text = ((int)sliHight.Value).ToString();
            }
        }

        private void txtHight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtHight.Text, out Hight))
            {
                if (Hight > sliHight.Minimum && Hight < sliHight.Maximum)
                {
                    sliHight.Value = (int)Hight;
                    Calculate();
                }
            }
        }
    }
}
