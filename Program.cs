using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tryoutcodes
{

    internal class Program
    {
        public static void Main(string[] args)
        {
            Calculation();
        }
        public static void Calculation(double v = 0.01, double rho = 1000, double tolerance = 1e-6)
        {
            //input variables
            Console.Write("Enter number of rows: ");
            int num_passes = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter inlet diameter in mm: ");
            double in_dia = Convert.ToDouble(Console.ReadLine()) / 1000;

            Console.Write("Enter the distance from center of inlet to end of distribution geometry in mm: ");
            double distri_length = Convert.ToDouble(Console.ReadLine()) / 1000;

            Console.Write("Enter nearest/desired press depth you need to calculate in mm: ");
            double init_press_depth = Convert.ToDouble(Console.ReadLine()) / 1000;

            double startAngle = -20; // Starting angle in degrees
            double endAngle = 50; // Ending angle in degrees
            double angleIncrement = (endAngle - startAngle) / (num_passes - 1);// Calculate the angle increment
            

            double[] length_list = new double[num_passes];
            
            double[] initial_guess = new double[num_passes];
            for (int i = 0; i < initial_guess.Length; i++)
            {
                initial_guess[i] = init_press_depth;
            }


            double[] press_depth_list = new double[num_passes];
            double[] fric_f = new double[num_passes];
            bool converged = false;

            for (int i = 0; i < num_passes; i++)
            {
                double angle = startAngle + (angleIncrement * i);
                double angleInRadians = angle * Math.PI / 180;
                length_list[i] = (distri_length / Math.Cos(angleInRadians)) - in_dia/2;
                Console.WriteLine($"The length of the passes from bottom is {length_list[i] * 1000}");
                press_depth_list[i] = initial_guess[i];

            }

            double mu = 0.001d;
            double[] Re_nu = new double[num_passes];

            while (!converged)
            {
                // Calculate pressure drop for each pipe using current diameters
                double[] dp = new double[num_passes];
                for (int i = 0; i < num_passes; i++)
                {
                    Re_nu[i] = rho * v * press_depth_list[i] / mu;
                    fric_f[i] = 0.316 / Math.Pow(Re_nu[i],0.25);
                    dp[i] = (fric_f[i] * length_list[i] * Math.Pow(v, 2) * rho) / (2 * press_depth_list[i]);
                }

                // Calculate the average pressure drop
                double avg_dp = dp.Sum() / num_passes;


                for (int i = 0; i < num_passes; i++)
                {
                    press_depth_list[i] = (fric_f[i] * length_list[i] * Math.Pow(v, 2) * rho) / (2 * avg_dp);

                }

                //if (dp.All(dp0 => Math.Abs(dp0 - avg_dp) > tolerance))
                //{
                //    for (int i = 0; i < num_passes; i++)
                //    {
                //        press_depth_list[i] = (fric_f[i] * length_list[i] * Math.Pow(v, 2) * rho) / (2 * avg_dp);

                //    }
                //    Console.WriteLine("if condtion running");
                //}
                //else //(press_depth_list.All(press_depth => press_depth <= 0.001 && press_depth >= init_press_depth))
                //{
                //    for (int i = 0; i < num_passes; i++)
                //    {
                //        press_depth_list[i] = press_depth_list[i] - (press_depth_list[i]*0.1);
                //        Console.WriteLine($"list pd {press_depth_list[i]}");
                //    }
                //    Console.WriteLine("else condition running");

                //}


                // Check for convergence
                converged = dp.All(dp0 => Math.Abs(dp0 - avg_dp) < tolerance) && press_depth_list.All(press_depth => press_depth >= 0.001 && press_depth <= 0.01);
                Console.WriteLine(string.Join(", ", dp) + ", " + avg_dp);
            }

            for (int i = 0; i < num_passes; i++)
            {
                Console.WriteLine($"Row {i + 1} length: {length_list[i] * 1000} mm ----> Press depth: {press_depth_list[i] * 1000} mm");
            }

            Console.ReadLine();
        }
    }
}
