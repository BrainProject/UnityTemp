using System.Collections;
using System.Timers;

namespace Puzzle
{
    public static class PuzzleStatistics //: MonoBehaviour
    {
        private static Timer timer = new Timer();

        public static ulong gameTime
        {
            get;
            private set;
        }

        public static string pictureName
        {
            get;
            set;
        }

        public static int numberPieces
        {
            get;
            set;
        }

        public static ulong GetNumberClicksTotal()
        {
            return numberClicksWithConnection + numberClicksWithoutConnection;
        }

        public static ulong numberClicksWithConnection
        {
            private set;
            get;
        }

        public static ulong numberClicksWithoutConnection
        {
            private set;
            get;
        }

        public static void Clear()
        {
            gameTime = 0;
            numberClicksWithConnection = 0;
            numberClicksWithoutConnection = 0;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += TimerTick;
        }

        public static void RegisterClickWithConnection()
        {
            numberClicksWithConnection++;
        }

        public static void RegisterClickWithoutConnection()
        {
            numberClicksWithoutConnection++;
        }

        public static void StartMeasuringTime()
        {
            timer.Start();
        }

        public static void StopMeasuringTime()
        {
            timer.Stop();
        }

        private static void TimerTick(object o, System.EventArgs e)
        {
            gameTime++;
        }
    }
}
