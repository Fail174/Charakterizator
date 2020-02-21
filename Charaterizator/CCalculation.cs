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

        public Matrix<double> CalculationCoef(Matrix<double> Rmtx, Matrix<double> Umtx, Matrix<double> Pmtx)
        {
            //-----------------------------------------------------------------------------------------------
            Matrix<double> resultBmtx;              // Возвращаемое значение           

            int rowRmtx = Rmtx.RowCount;            // Количество строк матрицы Rmtx
            int colsRmtx = Rmtx.ColumnCount;        // Количество столбцов матрицы Rmtx

            int rowUmtx = Umtx.RowCount;            // Количество строк матрицы Umtx
            int colsUmtx = Umtx.ColumnCount;        // Количество столбцов матрицы Umtx

            int rowPmtx = Pmtx.RowCount;            // Количество строк матрицы Pmtx
            int colsPmtx = Pmtx.ColumnCount;        // Количество столбцов матрицы Pmtx


            //-----------------------------------------------------------------------------------------------
            // ШАГ-1 - ПРОВЕРКА
            // Проверка на совпадение размерностей матриц 
            if ((rowRmtx != rowUmtx) || (rowRmtx != rowPmtx) || (colsRmtx != colsUmtx) || (colsRmtx != colsPmtx))
            {
                resultBmtx = DenseMatrix.Create(1, 1, -1);       // если размерности не совпадают возвращаем -1
                return resultBmtx;
            }
            


            //-----------------------------------------------------------------------------------------------
            // ШАГ-2 - УСРЕДНЕНИЕ (ПОДГОТОВКА ДАННЫХ)
            // Усредняем матрицу R           
            Matrix<double> MeanRmtx = Rmtx.ColumnAbsoluteSums().Divide(Rmtx.RowCount).ToRowMatrix();           

            // Усредняем матрицу U               
            Matrix<double> MeanUmtx = DenseMatrix.Create(rowUmtx / 2, colsUmtx, 0);
            for (int j = 0; j < colsUmtx; j++)
            {
                for (int i = 0; i < rowUmtx / 2; i++)
                {
                    MeanUmtx[i, j] = (Umtx.At(i, j) + Umtx.At(rowUmtx - 1 - i, j)) / 2;
                }
            }
           
            // Формируем матрицу P             
            Matrix<double> MeanPmtx = DenseMatrix.Create(rowPmtx / 2, colsPmtx, 0);
            for (int j = 0; j < colsPmtx; ++j)
            {
                for (int i = 0; i < rowPmtx / 2; ++i)
                {
                    MeanPmtx[i, j] = Pmtx.At(i, j);
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
            for (int n = 0; n < row; n++)
            {
                for (int k = 0; k < cols; k++)
                {
                    aj = 0;
                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {                            
                            Amtx[ai, aj] = (Math.Pow(MeanUmtx.At(n, k), i)* Math.Pow(MeanRmtx.At(0, k), j));
                            aj = aj + 1;
                        }
                    }
                    ai = ai + 1;
                }
            }
           
            // Формируем матрицу C 
            int rowMP = MeanPmtx.RowCount;
            int colsMP = MeanPmtx.ColumnCount;
            int ci = 0;
            int N = Amtx.RowCount/ rowMP;
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
            resultBmtx = DenseMatrix.Create(row * N, 1, 0);
            resultBmtx = Amtx.Solve(Сmtx);                             
                      

            return resultBmtx;
        }
    }
}
