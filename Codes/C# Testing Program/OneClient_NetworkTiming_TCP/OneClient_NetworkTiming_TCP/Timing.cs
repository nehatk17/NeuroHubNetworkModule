using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;


namespace HighResTimer
{
    public class Timing
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out ulong lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out ulong lpFrequency);

        public Timing()
        {
            if (QueryPerformanceFrequency(out freq) == false)
            {
                // high-performance counter not supported

                //throw new Win32Exception();
            }
            else
            {
                QueryPerformanceCounter(out startTime);
            }

        }

        private ulong startTime, curTime;
        private ulong freq;

        private bool started = false;

        public void Start()
        {
            // record start time
            started = true;
            QueryPerformanceCounter(out startTime);
        }

        public double Duration
        {            
            get
            {
                if (started == false) return 0;
                QueryPerformanceCounter(out curTime);
                return (double)(curTime - startTime) / (double)freq;
            }
        }

        public ulong GetStartCode()
        {
            return startTime;
        }
        public ulong GetFrequency()
        {
            return freq;
        }
    }
}
