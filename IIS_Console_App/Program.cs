using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIS_Console_App.Models;
using System.Net.Mail;
using System.Timers;
using System.IO;
namespace IIS_Console_App
{
    class Program
    {
 
        static void Main(string[] args)
        {
            const int maxMenuItems = 3;
            int selector = 1;
            int cpuThreshCount = 0;
            int occured = 0;
            bool flagSet = false;
            bool flagSet2 = false;
            float usage = 0;
            string machineName = System.Environment.MachineName;

            while (selector != maxMenuItems)
            {
                Console.Clear();
                //forces the loop to run constantly
                while (true)
                {
                    DateTime present = DateTime.Now;
                    //string machineName = System.Environment.MachineName;
                    List<ResourceMessage> results = ResourceChecker.cpuOverUse().ToList();
                    var needingAttention = results.Where(t => t.NeedsAttention);
                //checks to see if there was an error and if the bool needing attention is set it passes this to the if statment
                    if (needingAttention.Any())
                    {
                        //checks the performace again
                        //notice the thread.sleep and the two next values they are there so that the cpu can - 
                        //get a sample reading of the cpu at a given moment and then compare that against the next sample. With only one smaple is there is 
                        //nothing for the program to compare against and because of this the value will always be 100 percent 
                        var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                        usage = cpuCounter.NextValue();
                        System.Threading.Thread.Sleep(1000);
                        usage = cpuCounter.NextValue();
                       // if the usage is higher than 95 percent increment the results, otherwise reset the thresh hold to zero. If this is the second time since after 12 cpu spikes reset the email
                        //flag, otherwise it will never send and email again 
                        if (usage >= 95)
                        {
                            if (flagSet == true)
                            {
                                flagSet = false;
                                flagSet2 = true;
                                //cpuThreshCount = 0;
                                break;
                            }
                            else
                            {
                                cpuThreshCount++;
                                Console.Write(" Atleast one Error has occurred " + "cpuThreshCount" + cpuThreshCount);
                                Console.Write("\n");
                                Console.Write("The usage at the time was: " + usage);
                                Console.Write("\n");
                            }
                        }
                        else
                        {
                            if (cpuThreshCount >= 0 && usage <= 94 && flagSet2 == true)
                            {       
                                //if everything goes back to normal send out an email and tell the user everthing is ok again
                                    if (usage <= 94)
                                    {
                                        Console.WriteLine("***********Application or Service Is Now Running Within Safe Parameters*************");
                                         using (var client = new SmtpClient("gmail.com"))
                                         {
                                            
                                             using (var mailMessage = new MailMessage(new MailAddress("CPUDetector@gmail"), new MailAddress("foulkdw12@gmail.com")))
                                           
                                             {
                                    
                                                 mailMessage.Subject = machineName + " The Cpu is running sufficiently";
                                                 mailMessage.Body = "The Cpu is running sufficiently"; //+ "CpuThreshCount " + cpuThreshCount;
                                                 client.Send(mailMessage);
                                             }
                                         }
                                         cpuThreshCount = 0;
                                         flagSet = false;
                                         flagSet2 = false;
                                
                                    }
                                    cpuThreshCount = 0;
                                }
                            
                                
                              else
                            {
                                flagSet2 = false;
                                cpuThreshCount = 0;
                                break;
                            }
                        }
                     //send out an email if the cpu spikes for 12 times within 1 minute
                        if (cpuThreshCount == 12 && usage >= 95)
                        {
                            if (flagSet == false)
                            {
                                Console.WriteLine("***********Application or Serviadmce Exceeding Safe Operational Bounds*************");
                                using (var client = new SmtpClient("gmail.com"))
                                {

                                    using (var mailMessage = new MailMessage(new MailAddress("CPUDetector@gmail"), new MailAddress("foulkdw12@gmail.com")))
                                    {
                          
                                        mailMessage.Subject = machineName + " The Cpu is in OverUse";
                                        mailMessage.Body = "The Cpu is in OverUse"; // + "CpuThreshCount " + cpuThreshCount;
                                        client.Send(mailMessage);

                                    }

                                    //cpuThreshCount = 0;                           
                                    flagSet = true;                          
                                }
                            }
                      
                        }


                    }
                }


            }

                     }

                    }    //WriteToDataBase.SendToDataBase(result.Message, result.elapsedTime);

                };
