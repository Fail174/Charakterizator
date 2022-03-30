using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;


namespace Charaterizator
{
    class CCalculation
    {
        public static bool flag_ObrHod;  // 
        public static bool flag_MeanR;   // true - усреднять матрицу сопротивлений false - не усреднять


        public Matrix<double> CalculationCoef(Matrix<double> Rmtx, Matrix<double> Umtx, Matrix<double> Pmtx, double Pmax, bool sensor_DV)
        {
            //-----------------------------------------------------------------------------------------------
            Matrix<double> resultBmtx;              // Возвращаемое значение           

            int rowRmtx = Rmtx.RowCount;            // Количество строк матрицы Rmtx
            int colsRmtx = Rmtx.ColumnCount;        // Количество столбцов матрицы Rmtx

            int rowUmtx = Umtx.RowCount;            // Количество строк матрицы Umtx
            int colsUmtx = Umtx.ColumnCount;        // Количество столбцов матрицы Umtx

            int rowPmtx = Pmtx.RowCount;            // Количество строк матрицы Pmtx
            int colsPmtx = Pmtx.ColumnCount;        // Количество столбцов матрицы Pmtx

            int D; // D = 2 прямой и обратный ход, D = 1 только прямой ход


            //-----------------------------------------------------------------------------------------------
            // ШАГ-1 - ПРОВЕРКА
            // Проверка на совпадение размерностей матриц 
            if ((rowRmtx != rowUmtx) || (rowRmtx != rowPmtx) || (colsRmtx != colsUmtx) || (colsRmtx != colsPmtx))
            {
                resultBmtx = DenseMatrix.Create(1, 1, -1);       // если размерности не совпадают возвращаем -1
                return resultBmtx;               
            }

            // Проверяем установлен или нет флаг не учитывать обратный ход
            if (flag_ObrHod) // если да 
            {
                rowRmtx = 6;
                rowUmtx = 6;
                rowPmtx = 6;
                D = 1;
            }
            else // если нет
            {
                if (rowRmtx == 6)
                {
                    D = 1;
                }
                else
                {
                    D = 2;
                }
               
            }

            //-----------------------------------------------------------------------------------------------
            // ШАГ-2 - УСРЕДНЕНИЕ (ПОДГОТОВКА ДАННЫХ)
            // Если флаг true - Усредняем матрицу R        
            Matrix<double> MeanRmtx = DenseMatrix.Create(1, colsRmtx, 0);
            if (flag_MeanR)
            {
               
                double res;
                for (int j = 0; j < colsRmtx; j++)
                {
                    res = 0;
                    for (int i = 0; i < Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(rowRmtx))); i++)
                    {
                        res = res + Rmtx.At(i, j);
                    }
                    res = res / rowRmtx;
                    MeanRmtx[0, j] = res;
                }
            }         




            // Усредняем матрицу U               
            Matrix<double> MeanUmtx = DenseMatrix.Create(Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(rowUmtx) / D)), colsUmtx, 0);
            if (D == 1)
            {
                for (int j = 0; j < colsUmtx; j++)
                {
                    for (int i = 0; i < Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(rowUmtx) / D)); i++)
                    {
                        MeanUmtx[i, j] = Umtx.At(i, j);
                    }
                }
            }
            else if (D == 2)
            {
                for (int j = 0; j < colsUmtx; j++)
                {
                    for (int i = 0; i < Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(rowUmtx) / D)); i++)
                    {
                        MeanUmtx[i, j] = (Umtx.At(i, j) + Umtx.At(rowUmtx - 1 - i, j)) / 2;
                    }
                }
            }
            

            // Формируем матрицу P             
            Matrix<double> MeanPmtx = DenseMatrix.Create(Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(rowPmtx) / D)), colsPmtx, 0);
            for (int j = 0; j < colsPmtx; ++j)
            {
                for (int i = 0; i < Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(rowPmtx) / D)); ++i)
                {
                    MeanPmtx[i, j] = Pmtx.At(i, j);
                }
            }


            // Нормируем матрицу P    
            //Это можно перенести пораньше, сразу после формирования матрицы P и определения Pmax
            //Matrix<double> Pn = DenseMatrix.Create(rowP, colP, 0);
            if (sensor_DV)
            {
                for (int i = 0; i < MeanPmtx.RowCount; i++)
                {
                    for (int j = 0; j < MeanPmtx.ColumnCount; j++)
                    {
                        MeanPmtx[i, j] = Math.Abs(MeanPmtx.At(i, j) / Pmax);
                    }
                }
            }
            else
            {
                for (int i = 0; i < MeanPmtx.RowCount; i++)
                {
                    for (int j = 0; j < MeanPmtx.ColumnCount; j++)
                    {
                        MeanPmtx[i, j] = MeanPmtx.At(i, j) / Pmax;
                    }
                }
            }


            //-----------------------------------------------------------------------------------------------
            // ШАГ - 3 - ФОРМИРОВАНИЕ
            // Формируем матрицу A          
            int row = MeanUmtx.RowCount;
            int cols = MeanRmtx.ColumnCount;
            Matrix<double> Amtx = DenseMatrix.Create(row * cols, row * cols, 0);
            int ai = 0;
            int aj = 0;

            if (flag_MeanR)
            {
                for (int n = 0; n < row; n++)
                {
                    for (int k = 0; k < cols; k++)
                    {
                        aj = 0;
                        for (int i = 0; i < row; i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                Amtx[ai, aj] = (Math.Pow(MeanUmtx.At(n, k), i) * Math.Pow(MeanRmtx.At(0, k), j));
                                aj = aj + 1;
                            }
                        }
                        ai = ai + 1;
                    }
                }

            }

            else
            {
                for (int n = 0; n < row; n++)
                {
                    for (int k = 0; k < cols; k++)
                    {
                        aj = 0;
                        for (int i = 0; i < row; i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                Amtx[ai, aj] = (Math.Pow(MeanUmtx.At(n, k), i) * Math.Pow(Rmtx.At(n, k), j));
                                aj = aj + 1;
                            }
                        }
                        ai = ai + 1;
                    }
                }
            }


             
           

            // Формируем матрицу C 
            int rowMP = MeanPmtx.RowCount;
            int colsMP = MeanPmtx.ColumnCount;
            int ci = 0;
            int N = Amtx.RowCount/rowMP;
            Matrix<double> Сmtx = DenseMatrix.Create(row * N, 1, 0);
            for (int i = 0; i < rowMP; i++)
            {
                for (int n = 0; n < N; n++)
                {
                    Сmtx[ci, 0] = MeanPmtx.At(i, 0);
                    ci++;
                }
            }



            //-----------------------------------------------------------------------------------------------
            // ШАГ - 4 - РЕШЕНИЕ
            // Проверяем, 
            // если определитель матрицы Amtx равен нулю - то нет решения,
            if (Amtx.Determinant() == 0)
            {
                resultBmtx = DenseMatrix.Create(1, 1, -2);       // если решения нет возвращаем -2
                return resultBmtx;
            }

            // если определител не равен нулю
            // находим коэффиценты B
            Matrix<double> Bmtx = DenseMatrix.Create(row * N, 1, 0);
            Bmtx = Amtx.Solve(Сmtx);


            resultBmtx = DenseMatrix.Create(Bmtx.RowCount + 1, 1, 0);

            for (int i = 0; i < resultBmtx.RowCount - 1; i++)
            {
                resultBmtx[i, 0] = Bmtx.At(i, 0);
            }


            // Расчет R^2
            //Matrix<double> R2 = DenseMatrix.Create(1, 1, -1);
            Matrix<double> R2 = CalcR2(row, cols, Bmtx, Rmtx, Umtx, MeanPmtx);
            resultBmtx[Bmtx.RowCount, 0] = R2.At(0, 0);

            return resultBmtx;
            
        }

        // ФУНКЦИЯ для R^2        

        public Matrix<double> CalcR2(int rowP, int colP, Matrix<double> B, Matrix<double> Rmtx, Matrix<double> Umtx, Matrix<double> Pn)
        {
            Matrix<double> R2 = DenseMatrix.Create(1, 1, 0);
            double Fi;
            int m;

            // цикл по N(строкам матриц M, P, R, U)
            for (int N = 0; N < rowP; N++)
            {
                // цикл по K(столбцам матриц M, P, R, U)
                for (int K = 0; K < colP; K++)
                {
                    Fi = 0;
                    m = 0;
                    // цикл по j
                    for (int j = 0; j < 6; j++)
                    {
                        // цикл по i
                        for (int i = 0; i < 4; i++)
                        {
                            Fi = Fi + B.At(m, 0) * Math.Pow(Rmtx.At(N, K), i) * Math.Pow(Umtx.At(N, K), j);
                            m = m + 1;
                        }
                    }
                    //Fkn[N, K] = Math.Abs((Fi - Pn.At(N, K)) * 100 * Kp.At(N, K));
                    R2 = R2 + ((Fi - Pn.At(N, K)) * (Fi - Pn.At(N, K)));
                }
            }
            return R2;
        }



    }
}
