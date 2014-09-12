using System.Collections;
using System.Timers;
using System.Collections.Generic;
using System.Diagnostics;
using System;


namespace FindIt
{

    public static class FindItStatistics //: MonoBehaviour
    {
       // private static Timer timer = new Timer();

        private static List<double> findTimesLeft = new List<double>();
        private static List<double> findTimesRight = new List<double>();

        // stopwatch measuring the game time
        private static Stopwatch gameStopwatch = new Stopwatch();

        // last stopwath value when the time was read
        private static double lastTimeRecord = 0;

        public static ulong gameTime
        {
            get;
            private set;
        }

        // name of the set
        public static string resourcePackName
        {
            get;
            set;
        }

        // number of images to play with
        public static int numberPieces
        {
            get;
            set;
        }

        public static ulong GetGoodClicksTotal()
        {
            return goodClicksRight + goodClicksLeft;
        }

        public static ulong GetWrongClicksTotal()
        {
            return wrongClicksRight + wrongClicksLeft;
        }

        public static ulong goodClicksRight
        {
            private set;
            get;
        }

        public static ulong goodClicksLeft
        {
            private set;
            get;
        }

        public static ulong wrongClicksRight
        {
            private set;
            get;
        }

        public static ulong wrongClicksLeft
        {
            private set;
            get;
        }

        public static int turnsPassed
        {
            get;
            set;
        }

        public static int expectedGameTurnsTotal
        {
            get;
            set;
        }

        public static void Clear()
        {
            FindItStatistics.turnsPassed = 0;
            FindItStatistics.goodClicksLeft = 0;
            FindItStatistics.goodClicksRight = 0;
            FindItStatistics.wrongClicksLeft = 0;
            FindItStatistics.wrongClicksRight = 0;
            findTimesLeft.Clear();
            findTimesRight.Clear();
            gameStopwatch.Stop();
            gameStopwatch.Reset();





            FindItStatistics.gameTime = 0;

           /* timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += TimerTick;*/
        }

        public static void RecordLeftGoodClick()
        {
            double actualTime = gameStopwatch.ElapsedMilliseconds;

            goodClicksLeft++;
            turnsPassed++;

            findTimesLeft.Add(actualTime - lastTimeRecord);
            lastTimeRecord = actualTime;
        }

        public static void RecordRightGoodClick()
        {
            double actualTime = gameStopwatch.ElapsedMilliseconds;
            turnsPassed++;
            goodClicksRight++;
            findTimesRight.Add(actualTime - lastTimeRecord);
            lastTimeRecord = actualTime;
        }

        public static void RecordLeftWrongClick()
        {
            wrongClicksLeft++;
        }

        public static void RecordRightWrongClick()
        {
            wrongClicksRight++;
        }


        public static void StartMeasuringTime()
        {
            //timer.Start();
            gameStopwatch.Start();
        }

        public static void StopMeasuringTime()
        {
            //timer.Stop();
            gameStopwatch.Stop();
        }

        private static void TimerTick(object o, System.EventArgs e)
        {
            gameTime++;
        }

		public static double GetAverageClickFindTimeLeft()
		{
			double sum = 0;
			foreach(double time in findTimesLeft)
				sum += time;
			return Math.Round(sum / findTimesLeft.Count / 1000,3);
		}

		public static double GetAverageClickFindTimeRight()
		{
			double sum = 0;
			foreach(double time in findTimesRight)
				sum += time;

			return Math.Round(sum / findTimesRight.Count / 1000,3);
		}

		public static double GetAverageClickFindTimeTotal()
		{
			double sum = 0;
			foreach(double time in findTimesLeft)
				sum += time;
			foreach(double time in findTimesRight)
				sum += time;
			return Math.Round(sum / (findTimesLeft.Count + findTimesRight.Count) / 1000,3);
		}
    }
}
