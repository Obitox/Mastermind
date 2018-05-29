using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace PuzzleGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly ImageSource imageSource1 = new BitmapImage(new Uri("/images/1.jpg", UriKind.Relative));
        public static readonly ImageSource imageSource2 = new BitmapImage(new Uri("/images/2.png", UriKind.Relative));
        public static readonly ImageSource imageSource3 = new BitmapImage(new Uri("/images/3.jpg", UriKind.Relative));
        public static readonly ImageSource imageSource4 = new BitmapImage(new Uri("/images/4.jpg", UriKind.Relative));
        public static readonly ImageSource imageSource5 = new BitmapImage(new Uri("/images/5.png", UriKind.Relative));
        public static readonly ImageSource imageSource6 = new BitmapImage(new Uri("/images/6.png", UriKind.Relative));

        private ImageSource[] arrayOfSignSpots;
        private static int signSpotsCounter = 0;

        private List<Image> combinationArray = new List<Image>();
        private List<Image> userCombination = new List<Image>();
        private int[] numberOfElementArray = {0, 0, 0, 0, 0, 0};

        private Element[] elements =
        {
            new Element(), new Element(), new Element(), new Element(), new Element(),
            new Element()
        };

        private List<int> accuracyArray = new List<int>();

        /*CIRCLES*/
        private int circlesCount = 0;

        private int playerTime;

        

       // private delegate void SetValueCallback(int value);


        public MainWindow()
        {
            InitializeComponent();
            
            try
            {
                using (Stream stream = File.Open("Time.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    {
                        playerTime = (int)bin.Deserialize(stream);
                        Console.WriteLine(playerTime);
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error, deserilizing from the list");
            }
            SignPicker1Image.Source = imageSource1;
            SignPicker2Image.Source = imageSource2;
            SignPicker3Image.Source = imageSource3;
            SignPicker4Image.Source = imageSource4;
            SignPicker5Image.Source = imageSource5;
            SignPicker6Image.Source = imageSource6;
            

            combination();
            numberOfElementInCombination();

            Task task = new Task(() => { DoRun(); });
            task.Start();

            //var t = Task.Run(() => ShowThreadInfo(playerTime));
            //t.Wait();

            //Thread t = new Thread(new System.Threading.ThreadStart(DoWork));
            //t.Start();

            //this.Dispatcher.Invoke(
            //    DispatcherPriority.Background,
            //    new ThreadStart(() => {
            //        for (int n = 0; n < playerTime; n++)
            //        {
            //            Console.WriteLine("test");
            //            Thread.Sleep(500);
            //            ProgressBar1.Value = n;
            //        }
            //    }));
        }


        //public void DoWork()
        //{
        //    System.Threading.Thread.Sleep(1000);
        //    SetValueCallback d = new SetValueCallback(SetProcessBarValue);
        //    d(playerTime);
        //    ProgressBar1.Dispatcher.BeginInvoke(d, 100);
        //}

        private async void SetProcessBarValue(int value)
        {

            this.Dispatcher.Invoke((Action)(() =>
                {
                    for (int i = 0; i < value; i++)
                    {
                        ProgressBar1.Value += i;
                        Console.WriteLine(i);
                        Thread.Sleep(1000);
                    }
                }));
        }

        public async Task DoRun()
        {
            await DoRunAsync();
        }

        public async Task DoRunAsync()
        {
            await Task.Run(() => { SetProcessBarValue(playerTime); });
        }





        public void setImage(Image image, Canvas canvas)
        {
            Border border = canvas.Children.OfType<Border>().FirstOrDefault();

            Image newImage = new Image();
            newImage.Source = image.Source;
            border.Child = newImage;
        }

        public void setUserCombination(Image image, Canvas canvas)
        {
            setImage(image, canvas);
            userCombination.Add(image);

            Console.WriteLine(signSpotsCounter);

            if (signSpotsCounter % 4 == 0)
            {
                checkSolution();

                foreach (Element e in elements)
                {
                    e.resetNumberOfOccurence();
                    e.resetIsTrue();
                    e.resetIsFalse();
                }

                accuracyArray.Clear();
                userCombination.Clear();
            }
        }

        public void numberOfElementInCombination()
        {
            int counter = 0;

            foreach (Image image in combinationArray)
            {
                ImageSource source = image.Source;
                
                if (Equals(source, SignPicker1Image.Source)) numberOfElementArray[0]++;
                if (Equals(source, SignPicker2Image.Source)) numberOfElementArray[1]++;
                if (Equals(source, SignPicker3Image.Source)) numberOfElementArray[2]++;
                if (Equals(source, SignPicker4Image.Source)) numberOfElementArray[3]++;
                if (Equals(source, SignPicker5Image.Source)) numberOfElementArray[4]++;
                if (Equals(source, SignPicker6Image.Source)) numberOfElementArray[5]++;
            }
        }

        public int getElementNumber(ImageSource source)
        {
            if (Equals(source, SignPicker1Image.Source)) return 0;
            if (Equals(source, SignPicker2Image.Source)) return 1;
            if (Equals(source, SignPicker3Image.Source)) return 2;
            if (Equals(source, SignPicker4Image.Source)) return 3;
            if (Equals(source, SignPicker5Image.Source)) return 4;
            if (Equals(source, SignPicker6Image.Source)) return 5;
            return -1;
        }

        public void checkSolution()
        {
            int noOfElement = 0;

            for (int i = 0; i < 4; i++)
            {
                noOfElement = getElementNumber(userCombination.ElementAt(i).Source);
                elements[noOfElement].setNumberOfOccurence();

                for (int j = 0; j < 4; j++)
                {
                    if (Equals(userCombination.ElementAt(i).Source, combinationArray.ElementAt(j).Source) && i == j)
                    {
                        elements[noOfElement].setIsTrue();
                    }
                    else if (Equals(userCombination.ElementAt(i).Source, combinationArray.ElementAt(j).Source))
                    {
                        elements[noOfElement].setIsFalse();
                    }
                }
            }

            for (int i = 0; i < 6; i++)
            {
                int truth = elements[i].getIsTrue();
                int falseness = elements[i].getIsFalse();

                //Console.WriteLine("Truth " + truth);
                //Console.WriteLine("falseness " + falseness);
                //Console.WriteLine("GNOO " + elements[i].getNumberOfOccurence());
                //Console.WriteLine("NOEA " + numberOfElementArray[i]);

                int j = 0;

                if (truth > 0 || falseness > 0)
                {
                    while (j != numberOfElementArray[i])
                    {
                        while (truth != 0)
                        {
                            accuracyArray.Add(1);
                            truth--;

                            j++;
                        }

                        if (j == numberOfElementArray[i]) break;

                        while (falseness != 0)
                        {
                            if (j == elements[i].getNumberOfOccurence())
                            {
                                j++;
                                break;
                            }

                            accuracyArray.Add(0);
                            falseness--;

                            j++;
                            Console.WriteLine("Number of Occurence: " + elements[i].getNumberOfOccurence());
                            if (j == numberOfElementArray[i]) break;
                            
                        }
                    }
                }
            }

            sortAccuracyArray();

            Console.WriteLine("Working Accuracy Array :3");
            foreach (int boob in accuracyArray)
            {
                Console.WriteLine(boob);
            }

            colorCircles();
        }

        public void sortAccuracyArray()
        {
            for (int i = 0; i < accuracyArray.Count-1; i++)
            {
                for (int j = i+1; j < accuracyArray.Count; j++)
                {
                    if (accuracyArray[i] < accuracyArray[j])
                    {
                        int pom = accuracyArray[i];
                        accuracyArray[i] = accuracyArray[j];
                        accuracyArray[j] = pom;
                    }
                }
            }
        }

        public void colorCircles()
        {
            int i = 0;
            while (i != accuracyArray.Count)
            {
                string name = "ellipse" + ++circlesCount;
                Ellipse circle = (Ellipse)this.FindName(name);
                if (accuracyArray[i] == 1) circle.Fill = Brushes.Red;
                if (accuracyArray[i] == 0) circle.Fill = Brushes.Yellow;
                i++;
            }

            while (i != 4)
            {
                circlesCount++;
                i++;
            }
            
            if (circlesCount == 20) showCombination();
        }

        public void combination()
        {
            Random random = new Random();

            while (combinationArray.Count < 4)
            {
                int number = random.Next(6) + 1;
                switch (number)
                {
                    case 1:
                        combinationArray.Add(SignPicker1Image);
                        break;
                    case 2:
                        combinationArray.Add(SignPicker2Image);
                        break;
                    case 3:
                        combinationArray.Add(SignPicker3Image);
                        break;
                    case 4:
                        combinationArray.Add(SignPicker4Image);
                        break;
                    case 5:
                        combinationArray.Add(SignPicker5Image);
                        break;
                    case 6:
                        combinationArray.Add(SignPicker6Image);
                        break;
                }
            }
        }

        public void showCombination()
        {
            int counter = 1;

            foreach (Image imageNoob in combinationArray)
            {
                string name = "Sign2" + counter++; /*I AM hot :3*/
                Canvas canvas = (Canvas)this.FindName(name);

                setImage(imageNoob, canvas);
            }
        }

        private void SignPicker1Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string name = "Sign" + ++signSpotsCounter;
            Canvas canvas = (Canvas) this.FindName(name);

            setUserCombination((Image)sender, canvas);      
        }
        private void SignPicker2Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string name = "Sign" + ++signSpotsCounter;
            Canvas canvas = (Canvas)this.FindName(name);

            setUserCombination((Image)sender, canvas);
        }
        private void SignPicker3Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string name = "Sign" + ++signSpotsCounter;
            Canvas canvas = (Canvas)this.FindName(name);

            setUserCombination((Image)sender, canvas);
        }
        private void SignPicker4Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string name = "Sign" + ++signSpotsCounter;
            Canvas canvas = (Canvas)this.FindName(name);

            setUserCombination((Image)sender, canvas);
        }
        private void SignPicker5Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string name = "Sign" + ++signSpotsCounter;
            Canvas canvas = (Canvas)this.FindName(name);

            setUserCombination((Image)sender, canvas);
        }
        private void SignPicker6Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string name = "Sign" + ++signSpotsCounter;
            Canvas canvas = (Canvas)this.FindName(name);

            setUserCombination((Image)sender, canvas);
        }

    }
}
