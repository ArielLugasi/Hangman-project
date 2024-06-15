using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace HangmanEX
{

    public sealed partial class MainPage : Page
    {
        private List<Button> buttons;
        private readonly List<BitmapImage> images;
        private List<TextBlock> fieldChar;
        private string _word;
        private int _counterMiss;
        public MainPage()
        {
            this.InitializeComponent();
            images = new List<BitmapImage>();
            LoadImage();
            DoWordArea();
        }

        private void LoadImage()
        {
            for (int i = 1; i < 10; i++)
            {
                var image = new BitmapImage(new Uri(@"ms-appx:/Images/img" + i.ToString() + ".png"));
                images.Add(image);
            }
        }

        private void DoWordArea()
        {
            _counterMiss = 0;
            CreateKeyBoard();
            this._word = WordBank.ReturnWord();
            imageMiss.Source = images[0];
            fieldChar = new List<TextBlock>();
            wordArea.Children.Clear();
            for (int i = 0; i < this._word.Length; i++)
            {
                TextBlock textBlock = new TextBlock()
                {
                    Text = "_",
                    Margin = new Thickness(10),
                    FontSize = 50,
                };
                wordArea.Children.Add(textBlock);
                fieldChar.Add(textBlock);
            }
            fieldChar[0].Text = this._word[0].ToString(); //first char in word
            fieldChar[this._word.Length - 1].Text = this._word[this._word.Length - 1].ToString();//last char in word
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoWordArea();
        }

        private void CreateKeyBoard()
        {
            buttons = new List<Button>();
            firstRow.Children.Clear();
            secRow.Children.Clear();
            thirdRow.Children.Clear();
            for (int i = 65; i < 91; i++)//Ascii [a z]
            {
                Button btn = new Button()
                {
                    Content = ((char)i).ToString(),
                    FontSize = 55,
                    Width = 120,
                    Height = 120,
                    Margin = new Thickness(2)
                };
                btn.Click += BT_Click_key;
                if (i % 65 < 8) firstRow.Children.Add(btn);
                else if (i % 65 >= 8 && i % 65 < 16) secRow.Children.Add(btn);
                else thirdRow.Children.Add(btn);
                buttons.Add(btn);

            }
        }

        private void BT_Click_key(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string character = button.Content.ToString();
            bool hit = false;
            for (int i = 1; i < this._word.Length - 1; i++)// without first and last
            {
                if (this._word[i].ToString().ToLower() == character.ToLower())
                {
                    hit = true;
                    fieldChar[i].Text = character.ToLower();
                }
            }
            if (hit == false)
            {
                _counterMiss += 1;
                imageMiss.Source = images[_counterMiss];
            }
            // lose
            if (_counterMiss == 8)
            {
                MeesageToUserAsync("You Lose :(");
            }
            //win
            int count = 0;
            for (int i = 0; i < this._word.Length; i++)
            {
                if (fieldChar[i].Text != "_") count++;
            }
            if (count == this._word.Length)
            {
                MeesageToUserAsync("Well Done, You Won");
            }
            button.IsEnabled = false;
        }

        private async void MeesageToUserAsync(string statement)
        {

            MessageDialog messageDialog = new MessageDialog("Please , play again", statement);
            await messageDialog.ShowAsync();
            DoWordArea();// new game
        }
    }
}
