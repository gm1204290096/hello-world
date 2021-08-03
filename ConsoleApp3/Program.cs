using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static double L1 = 0.092, L3 = 1.124, L4 = 0.28, L6 = 0.65, _L6 = 1.118;//输入
        static double W1 = 3 * Math.PI, TT = 0, T1;//输入
        static double[] S3, SE, T3, T4;//输出
        static double[] VB2B3, VE, W3, W4;//输出
        static double[] AB2B3, AE, A3, A4;//输出
        static void Main(string[] args)
        {
            #region 参数值输入
            try
            {
                Console.WriteLine("请按格式输入值，否则都按默认值计算！！！");
                Console.Write("请输入w1(弧度制,例如3派请输入 3PI)：");
                string temp = Console.ReadLine().Replace("PI", "");
                W1 = Convert.ToDouble(temp) * Math.PI;

                Console.Write("请输入θ1(角度制，且小于360°，列如90°请输入 90)：");
                TT = Convert.ToDouble(Console.ReadLine());//角度
                T1 = Angle2Radian(TT);//弧度

                Console.Write("请输入l1：");
                L1 = Convert.ToDouble(Console.ReadLine());
                Console.Write("请输入l3：");
                L3 = Convert.ToDouble(Console.ReadLine());
                Console.Write("请输入l4：");
                L4 = Convert.ToDouble(Console.ReadLine());
                Console.Write("请输入l6：");
                L6 = Convert.ToDouble(Console.ReadLine());
                Console.Write("请输入l6'：");
                _L6 = Convert.ToDouble(Console.ReadLine());
            }
            catch
            {
                Console.Write("输入格式不对，默认值进行计算");
            }
            #endregion

            #region 循环次数计算
            int n = (int)(360 - T1);
            n = n / 10 + 1;
            Console.WriteLine("循环{0}次", n.ToString());
            S3 = new double[n]; SE = new double[n]; T3 = new double[n]; T4 = new double[n];
            VB2B3 = new double[n]; VE = new double[n]; W3 = new double[n]; W4 = new double[n];
            AB2B3 = new double[n]; AE = new double[n]; A3 = new double[n]; A4 = new double[n];
            #endregion
            int i = 0;
            //for (int j = 0; j < n; j++)
            //{
            //    Console.Write("{0}\t", j);
            //}
            Console.WriteLine("角度\tS3\tSE\tθ3\tθ4\tVB2B3\tVE\tW3\tW4\tAB2B3\tAE\tε3\tε4");
            for (; TT <= 360.0; TT += 10.0)
            {
                T1 = Angle2Radian(TT);
                AnalysisPosition(out S3[i], out SE[i], out T3[i], out T4[i]);
                AnalysisSpeed(S3[i], T3[i], T4[i], out VB2B3[i], out VE[i], out W3[i], out W4[i]);
                AnalysisAcceleration(S3[i], T3[i], T4[i], W3[i], W4[i], VB2B3[i], out AB2B3[i], out AE[i], out A3[i], out A4[i]);                
                Console.WriteLine(TT.ToString() + "°\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}", 
                    Math.Round(S3[i], 3), Math.Round(SE[i], 3), Math.Round(T3[i], 3), Math.Round(T4[i], 3),
                    Math.Round(VB2B3[i], 3), Math.Round(VE[i], 3), Math.Round(W3[i], 3), Math.Round(W4[i], 3),
                    Math.Round(AB2B3[i], 3), Math.Round(AE[i], 3), Math.Round(A3[i], 3), Math.Round(A4[i], 3));
                i++;
            }
            Console.ReadLine();
        }

        static void AnalysisPosition(out double s3, out double se, out double t3, out double t4) //位置分析
        {


            t3 = Math.Atan((L6 + L1 * Math.Sin(T1)) / (L1 * Math.Cos(T1)));
            s3 = (L1 * Math.Cos(T1)) / (Math.Cos(t3));
            t4 = Math.Asin((_L6 - (L3 * Math.Sin(t3))) / L4);
            se = L3 * Math.Cos(t3) + L4 * Math.Cos(t4);
        }

        static void AnalysisSpeed(double s3, double t3, double t4, out double vb2b3, out double ve, out double w3, out double w4) //速度分析
        {
            vb2b3 = -(W1 * L1 * Math.Sin(T1 - t3));
            w3 = (W1 * L1 * Math.Cos(T1 - t3)) / (s3);
            ve = -((w3 * L3 * Math.Sin(t3 - t4)) / (Math.Cos(t4)));
            w4 = -((w3 * L3 * Math.Cos(t3)) / (L4 * Math.Cos(t4)));
        }

        static void AnalysisAcceleration(double s3, double t3, double t4, double w3, double w4, double vb2b3, out double ab2b3, out double ae, out double a3, out double a4) //速度分析
        {
            ab2b3 = (w3 * w3 * s3) - (W1 * W1 * L1 * Math.Cos(T1 - t3));
            a3 = (W1 * W1 * L1 * Math.Sin(t3 - T1) - 2 * w3 * vb2b3) / s3;
            ae = -((a3 * L3 * Math.Sin(t3 - t4) + w3 * w3 * L3 * Math.Cos(t3 - t4) - w4 * w4 * L4) / (Math.Cos(t4)));
            a4 = (w3 * w3 * L3 * Math.Sin(t3) + w4 * w4 * L4 * Math.Sin(t4) - a3 * L3 * Math.Cos(t3)) / (L4 * Math.Cos(t4));
        }

        static double Angle2Radian(double angle)
        {
            double radian;
            radian = angle * Math.PI / 180;
            return radian;
        }
        static double Radian2Angle(double radian)
        {
            double angle;
            angle = radian * 180 / Math.PI;
            return angle;
        }
    }
}
