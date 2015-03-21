/**
 * @file FindItStatistics.cs
 * @author Ján Bella
 */
using System.Collections;
using System.Timers;
using System.Collections.Generic;
using System.Diagnostics;
using System;


namespace FindIt
{
    /**
     * Records statistics about one FindIt game
     */
    public static class FindItStatistics
    {
        // stores search times for the left and right side of the grid in miliseconds
        private static List<double> findTimesLeft = new List<double>();
        private static List<double> findTimesRight = new List<double>();

        // stopwatch measuring the game time
        private static Stopwatch gameStopwatch = new Stopwatch();

        // last stopwath value when the time was read
        private static long lastTimeRecord = 0;

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

        /**
         * @return returns total number of good clicks
         */
        public static ulong GetGoodClicksTotal()
        {
            return goodClicksRight + goodClicksLeft;
        }

        /**
         * @return returns total number of wrong clicks (clicks on tiles with image different than target image)
         */
        public static ulong GetWrongClicksTotal()
        {
            return wrongClicksRight + wrongClicksLeft;
        }


        // number of good clicks on the right side of the grid
        public static ulong goodClicksRight
        {
            private set;
            get;
        }

        // number of good clicks on the left side of the grid
        public static ulong goodClicksLeft
        {
            private set;
            get;
        }

        // number of wrong clicks on the right side of the grid
        public static ulong wrongClicksRight
        {
            private set;
            get;
        }

        // number of wrong clicks on the left side of the grid
        public static ulong wrongClicksLeft
        {
            private set;
            get;
        }

        // number of turns passed
        public static int turnsPassed
        {
            get;
            set;
        }

        // number of turns that is the game played on. After this number of turns, the game finishes
        public static int expectedGameTurnsTotal
        {
            get;
            set;
        }

        /**
         * Clears variables
         */
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
        }

        /**
         * Records good click on the left side of the grid
         */
        public static void RecordLeftGoodClick()
        {
            long actualTime = gameStopwatch.ElapsedMilliseconds;

            goodClicksLeft++;
            turnsPassed++;

            findTimesLeft.Add(actualTime - lastTimeRecord);
            lastTimeRecord = actualTime;
        }

        /**
         * Records good click on the right side of the grid
         */
        public static void RecordRightGoodClick()
        {
            long actualTime = gameStopwatch.ElapsedMilliseconds;
            turnsPassed++;
            goodClicksRight++;
            findTimesRight.Add(actualTime - lastTimeRecord);
            lastTimeRecord = actualTime;
        }

        /**
         * Records wrong click on the left side of the grid
         */
        public static void RecordLeftWrongClick()
        {
            wrongClicksLeft++;
        }

        /**
         * Records wrong click on the right side of the grid
         */
        public static void RecordRightWrongClick()
        {
            wrongClicksRight++;
        }

        /**
         * Starts game Stopwatch
         */
        public static void StartMeasuringTime()
        {
            gameStopwatch.Start();
        }

        /**
         * Stops game Stopwatch
         */
        public static void StopMeasuringTime()
        {
            gameStopwatch.Stop();
        }

        /**
         * @return average time to find the target on the left side of the grid
         */
		public static double GetAverageClickFindTimeLeft()
		{
			double sum = 0;
			foreach(double time in findTimesLeft)
				sum += time;
			return Math.Round(sum / findTimesLeft.Count / 1000,3);
		}

        /**
         * @return average time to find the target on the right side of the grid
         */
		public static double GetAverageClickFindTimeRight()
		{
			double sum = 0;
			foreach(double time in findTimesRight)
				sum += time;

			return Math.Round(sum / findTimesRight.Count / 1000,3);
		}

        /**
         * @return average time to find the target anywhere
         */
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
