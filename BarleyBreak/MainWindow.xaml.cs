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

namespace Demo.Containers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int count_p = 0;
        class GamePlace
        {
            public int x;
            public int y;
        }

        int[,] _gameField = { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 15, 0 } };

        GamePlace _zeroPoint = new GamePlace { x = 3, y = 3 };

        public MainWindow()
        {
            InitializeComponent();

            Rasstanovka();

            SetButtonsPosition();
        }

        private void SetButtonsPosition()
        {
            for (int n = 0, i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j, ++n)
                {
                    //if (n > 14) break;
                    if (_gameField[i, j] == 0)
                    {
                        --n;
                        continue;
                    }

                    Button btn = _field.Children[n] as Button;
                    btn.SetValue(Grid.RowProperty, i);
                    btn.SetValue(Grid.ColumnProperty, j);

                    btn.Tag = new GamePlace { x = i, y = j };

                    btn.Content = _gameField[i, j];
                }
            }

        }

        private void Rasstanovka()
        {
            int[] mas = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0 };

            //Реализация стандартного алгоритма равновероятностного перемешивания
            #region Random Shuffle

            Random rnd = new Random();
            for (int i = 0; i < 16; ++i)
            {
                int r = rnd.Next(16);
                int tmp = mas[r];
                mas[r] = mas[i];
                mas[i] = tmp;
            }

            #endregion

            //Проверяем на собираемость https://ru.wikipedia.org/wiki/%D0%9F%D1%8F%D1%82%D0%BD%D0%B0%D1%88%D0%BA%D0%B8#.D0.9C.D0.B0.D1.82.D0.B5.D0.BC.D0.B0.D1.82.D0.B8.D1.87.D0.B5.D1.81.D0.BA.D0.BE.D0.B5_.D0.BE.D0.BF.D0.B8.D1.81.D0.B0.D0.BD.D0.B8.D0.B5
            //int[] M = new int[16];

            for (int n = 0, i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j, ++n)
                {
                    _gameField[i, j] = mas[n];
                    if (mas[n] == 0)
                    {
                        _zeroPoint.x = i;
                        _zeroPoint.y = j;
                    }
                }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            GamePlace place = btn.Tag as GamePlace;

            if( (place.x==0 || _gameField[place.x-1, place.y]!=0)&&
                (place.x==3 || _gameField[place.x+1, place.y]!=0)&&
                (place.y==0 || _gameField[place.x, place.y-1]!=0)&&
                (place.y==3 || _gameField[place.x, place.y+1]!=0))
            {
                MessageBox.Show("И куда же это перемещать?");
                return;
            }

            _gameField[_zeroPoint.x, _zeroPoint.y] = _gameField[place.x, place.y];
            _gameField[place.x, place.y] = 0;

            btn.Tag = new GamePlace { x = _zeroPoint.x, y = _zeroPoint.y };

            _zeroPoint = place;

            btn.SetValue(Grid.RowProperty, (btn.Tag as GamePlace).x);
            btn.SetValue(Grid.ColumnProperty, (btn.Tag as GamePlace).y);

            count_p++;

            if (Proverka())
                if (MessageBox.Show("Вы выиграли! Количество перемещений: " + count_p + "\nНачать заново?", "Пятнашки", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Rasstanovka();
                    SetButtonsPosition();
                }
                else
                {
                    this.Close();
                }
        }

        private bool Proverka()
        {
            int count = 0;
            int[,] _gameField_2 = { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 }, { 13, 14, 15, 0 } };
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    if (_gameField[i,j] == _gameField_2[i,j])
                        count++;
                }
            if (count == 16)
                return true;
            return false;
        }
    }
}
