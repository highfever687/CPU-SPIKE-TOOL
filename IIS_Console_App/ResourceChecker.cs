using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Microsoft.Web.Administration;
using System.Timers;
using IIS_Console_App.Models;
using System.Net.Mail;

/*
Basically this class checks to see if the CPU is running over 85%
 */

namespace IIS_Console_App
{
    class ResourceChecker
    {
       private string message = "";
       //pivate string elapsedTime = "";
       public static IEnumerable<ResourceMessage> cpuOverUse()
        {
            Stopwatch stopwatch = new Stopwatch();
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //System.Threading.Thread.Sleep(1000);
            float usage = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            usage = cpuCounter.NextValue();
                // Check for high cpu usage
                if (usage > 98)
                {
                    stopwatch.Start();
                    System.Threading.Thread.Sleep(100);
                    stopwatch.Stop();
                    TimeSpan theTimeStamp = stopwatch.Elapsed;
                    Console.Write(usage.ToString());
                    Console.Write("\n");
                    yield return new ResourceMessage
                    {
                        Message = cpuCounter.ToString() ,
                        NeedsAttention = true,
                        elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        theTimeStamp.Hours, theTimeStamp.Minutes, theTimeStamp.Seconds,
                        theTimeStamp.Milliseconds / 10),                                     
                    };
                    //WriteToDataBase.SendToDataBase(message, elapsedTime);
                }
                cpuCounter.Dispose();
            }
        }      
    }