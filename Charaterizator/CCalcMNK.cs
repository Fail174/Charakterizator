﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;


namespace Charaterizator
{
    class CCalcMNK
    {
        
        // Для датчика ЭНИ-12
        // Табличные значения
        // Пределы допускаемой основной приведенной погрешности Коэффициенты а
        public Matrix<double> gammaPa = DenseMatrix.OfArray(new double[,] {
            {  0,   4,      10,    25  },
            {  7,   0.075,  0.1,   0.14},
            { 10,   0.1,    0.1,   0.2 },
            { 15,   0.15,   0.2,   0.3 },
            { 20,   0.2,    0.2,   0.4 },
            { 25,   0.25,   0.3,   0.4 },
            { 50,   0.5,    0.5,   0.8 }});


        // Коэффициенты b
        public Matrix<double> gammaPb = DenseMatrix.OfArray(new double[,] {
            { 0,    4,   10,      25   },
            { 7,    0,   0.014,   0.01 },
            { 10,   0,   0.02,   0.01  },
            { 15,   0,   0.03,    0.02 },
            { 20,   0,   0.03,    0.02 },
            { 25,   0,   0.04,    0.03 },
            { 50,   0,   0.08,    0.05 }});

        // Для датчика ЭНИ-12
        // Табличные значения
        // Дополнительная температурная погрешность
        public Matrix<double> gammaTa = DenseMatrix.OfArray(new double[,] {
            { 0,    1,       2},
            { 7,    0.04,    0.05},
            {10,    0.04,    0.05},
            {15,    0.04,    0.05},
            {20,    0.05,    0.07},
            {25,    0.05,    0.07},
            {50,    0.1,     0.1 }});


        // Коэффициенты b
        public Matrix<double> gammaTb = DenseMatrix.OfArray(new double[,] {
            { 0,     1,       2  },
            { 7,     0.03,    0.07},
            { 10,    0.03,    0.07},
            { 15,    0.03,    0.07},
            { 20,    0.04,    0.08},
            { 25,    0.04,    0.08},
            { 50,    0.05,    0.1 }});


        int nm = 0;


        //-----------------------------------------------------------------------------------
        // НАСТРОЙКИ РАСЧЕТА - вынести на форму       
        // Kf - коэффициент для формирования расчетной границы
        public int Kf = 3;
        //Kpmax_dop - максимально допускаемое значение коэффициента перенастройки
        public int Kpmax_dop = 10;
        //Код погрешности
        public int code = 7;
        // Максимальное кол-во циклов поиска
        public int Amax = 300;
        // Максимальное значение веса
        public int Mmax = 300;
        // Температура НКУ
        public double Tnku = 23;       
        // Коэффициент регулировки при расчете шага веса 
        public double KdM = 0.33;        
        // Допускаемый минимальный шаг расчетной границы
        public double deltaFdop_min = 0.0001;
        // Минимальная расчетная граница
        public double Fr_min = 0.01;

        //-----------------------------------------------------------------------------------
        public double deltaFkn_min = 100;

        




        // ----------------------------------------------------------------------------------
        // -------- ОСНОВНАЯ ФУНКЦИЯ РАСЧЕТА ------------------------------------------------                                      
        // весовых коэффициентов калибровки датчика с помощью МНК
        // ----------------------------------------------------------------------------------
        public Matrix<double> CalcCalibrCoef(Matrix<double> Rmtx, Matrix<double> Umtx, Matrix<double> Pmtx, Matrix<double> Tmtx, double Pmax, int sens)
        {
           
            Matrix<double> resultBmtx;           // Возвращаемое значение - Матрица калибровочных коэффициентов 
                                                 // -1 если решения не найдено 
                                                 // -2 если не верные входные данные (не согласованый матрицы)
                                                 // -3 если ошибка возникла в процессе решения (исключение)
                                                 // -4 если в массиве температур нет точки НКУ 23 градуса
                                                 // -5 если отсутствует точка ноль по давлению, используемая для определения класса точности (обработка результатов) 
                                                 // -6 если после расчетов не попали ни в какой код точности (БРАК)

            
            Matrix<double> Kp;      // Матрица коэффициентов пернастройки(стартовая)  
            
            Matrix<double> Fdop;    // Матрица допустимых отклонений Fdop (стартовая)                 
            Matrix<double> Fr;      // Матрица допустимых отклонений Fdop (расчетная)   
                                              
            Matrix<double> Fkn1;    // Матрица фактических отклонений Fkn1 (стартовая с единичными весами)
            Matrix<double> Fkn;     //Рассчитываем фактические отклонения Fkn (расчетная)

            Matrix<double> M1;      // Матрица весов (стартовая, единичная)
            Matrix<double> M;       // Матрица весов (расчетная)
            Matrix<double> M_opt;   // Матрица весов (оптимальная)
            double maxValueM;       // максимальное значение веса матрицы M

            Matrix<double> B1;      // Матрица весовых коэффициентов (стартовая, для единичных весов)
            Matrix<double> B;       // Матрица весовых коэффициентов (расчетная, промежуточная)
    
            int Ndop;               // Количество точек в допуске                                                      
            int Nmax;               // Общее количество точек (число элементов в матрице)

            int Kf_opt;             // Оптимальный Kf    
            double Kmult_opt;       // Оптимальный Kmult                 

            int Tdw;                // Счетчик всех циклов do -while
            int Tdw_opt;             // Количество найденных не оптимальных решений подряд
            int Tdw_opt_max;        // Макс.кол - во решений в допуске но НЕ оптимальных
            int Tdw_out_res;        // Промежуточный счетчик циклов подряд без решения
            int Tdw_out_res_max;    // Предельное значение промежуточного счетчика циклов подряд без решения
            int Tdw_out_res_sum;    // Общий счетчик циклов в сумме без решения
            int Tdw_out_res_sum_max;// Предельное количество циклов в сумме без решения
            int Res_count;          // Счетчик найденных решений
            int Res_count_max;      // Максимальное количество найденных решений
            bool Exit_dw;           // Критерий выхода из цикла do -while

            int curCode;


            // ---------- ЭТАП-1 ------------------------------------------------------------
            // ПОДГОТОВКА ДАННЫХ ДЛЯ РАСЧЕТА
            // ------------------------------------------------------------------------------

            // Определяем количество строк и столбцов входных матриц         
            int rowP = Pmtx.RowCount;           // Количество строк матрицы Pmtx
            int colP = Pmtx.ColumnCount;        // Количество столбцов матрицы Pmtx

            int rowU = Umtx.RowCount;           // Количество строк матрицы Umtx
            int colU = Umtx.ColumnCount;        // Количество столбцов матрицы Umtx

            int rowR = Rmtx.RowCount;           // Количество строк матрицы Rmtx
            int colR = Rmtx.ColumnCount;        // Количество столбцов матрицы Rmtx

            int colT = Tmtx.ColumnCount;        // Количество столбцов матрицы Rmtx

            // Проверяем на соответствие размерности матриц
            if ((rowP != rowU) || (rowU != rowR) || (colP != colU) || (colU != colR) || (colR != colT))
            {
                resultBmtx = DenseMatrix.Create(1, 1, -2);  // возвращаем: -2 не верные входные данные
                return resultBmtx;
            }



            // Если размерности входных данных согласованы
            // Определяем общее количество точек (число элементов в матрице)
            Nmax = rowP * colP;

            // Нормируем матрицу P    
            //Это можно перенести пораньше, сразу после формирования матрицы P и определения Pmax
            Matrix<double> Pn = DenseMatrix.Create(rowP, colP, 0);

            for (int i = 0; i < rowP; i++)
            {
                for (int j = 0; j < colP; j++)
                {
                    Pn[i, j] = Pmtx.At(i, j) / Pmax;
                }
            }

            // Формируем единичную матрицу весов M          
            M1 = DenseMatrix.Create(rowP, colP, 1);
            M_opt = M1;

            // Рассчитываем матрицу коэффициентов B1(при единичной матрице весов М)
            B1 = CalcB(rowP, colP, M1, Pn, Umtx, Rmtx);
            if ((B1.RowCount == 1) && (B1.At(0, 0) == -1))
            {
                resultBmtx = DenseMatrix.Create(1, 1, -1);  // возвращаем: -1 решения нет
                return resultBmtx;
            }

            // ФОРМИРОВАНИЕ СТАРТОВОЙ РАСЧЕТНОЙ ГРАНИЦЫ
            // Составляем матрицу коэффициентов пернастройки, где все Kp = 0           
            Kp = DenseMatrix.Create(rowP, colP, 0);
            double Kpmax = 0;

            // Составляем матрицу коэффициентов пернастройки Kp, кроме точек, где P(i, j) == 0
            for (int i = 0; i < rowP; i++)
            {
                for (int j = 0; j < colP; j++)
                {
                    if (Pmtx.At(i, j) != 0)
                    {
                        Kp[i, j] = Pmax / Math.Abs(Pmtx.At(i, j));        //abs(P(i, j)), чтобы обеспечить универсальность формулы относителньо вида давления(ДД, ДИВ, ДА и пр)
                        {
                            if (Kp[i, j] > Kpmax)
                            {
                                Kpmax = Kp.At(i, j);
                            }
                        }
                    }
                }
            }

            // Ограничение на Kpmax
            if (Kpmax > Kpmax_dop)
            {
                Kpmax = Kpmax_dop;
            }

            // Окончательное формирование матрицы коэффициентов пернастройки Kp
            for (int i = 0; i < rowP; i++)
            {
                for (int j = 0; j < colP; j++)
                {
                    if ((Kp.At(i, j) == 0) || (Kp.At(i, j) > Kpmax))
                    {
                        Kp[i, j] = Kpmax;
                    }
                }
            }
            
            // Рассчитываем матрицу допустимых отклонений Fdop(размерностью N, K)
            // Внутри функции должно быть определение какой тип датчика (ЭнИ - 100 или ЭнИ - 12, или может быть другой новый тип)                        
            Fdop = CalcFdop(rowP, colP, Kf, Kp, code, sens, Tmtx, Tnku, gammaPa, gammaPb);

            // Проверка рассчитанной матрицы допустимых отклонений Fdop на отсутствие нулевых элементов
            // Если нулевые элементы есть, следовательно нет решения, выходим из функции (возвращаем -1)
            int ind = 0;
            bool flag = true;

            while ((ind < rowP)&&(flag))
            {
                for (int j = 0; j < colP; j++)
                {
                    if (Fdop.At(ind, j) == 0)
                    {                        
                        flag = false;
                        break;
                    }
                }
                ind = ind + 1;
            }

            if (flag == false)
            {
                resultBmtx = DenseMatrix.Create(1, 1, -2);  // возвращаем: -2 не верные входные данные
                return resultBmtx;
            }

            //Рассчитываем фактические отклонения Fkn(формула 5, стр. 10)
            Fkn1 = CalcFkn(rowP, colP, B1, Rmtx, Umtx, Pn, Kp);




            // ---------- ЭТАП - 2 --------------------------------------------------------
            // ОСНОВНОЙ АЛГОРИТМ РАСЧЕТА
            // ПОИСК Kf
            // ----------------------------------------------------------------------------

            // Флаг решение false - не найдено, true - найдено
            bool flagFindR = false;
            bool flagExitCalc = false;
            double dM = 0;
            maxValueM = 0;

            for (int Kf = 1; Kf < Kpmax_dop; Kf++)  // Изменил предельное значение "10" на "Kpmax_dop"
            {
                // Рассчитываем матрицу допустимых отклонений Fdop для заданного Kf
                Fdop = CalcFdop(rowP, colP, Kf, Kp, code, sens, Tmtx, Tnku, gammaPa, gammaPb);

                // Формируем расчетную матрицу Fr(на старте она равна допускаемой Fdop)
                Fr = Fdop;

                // Формируем матрицу единичных весовов М (на старте она равна единичной матрице)
                M = M1;
                M = DenseMatrix.Create(rowP, colP, 1);

                // Вычисляем матрицу фактических отклонений Fkn(на старте она равна фактическим отклонениям при единичных весовых коэффициентах)                
                Fkn = Fkn1;

                // Определяем кол - во точек в допуске
                Ndop = CalcNdop(rowP, colP, Fkn1, Fr);

                // Если не все точки в допуске
                if (Ndop < Nmax)
                {
                    //maxValueM = 0;
                    // Организуем цикл по а до Amax
                    for (int a = 0; a < Amax; a++)
                    {
                        flagExitCalc = false;

                        // Перерассчитываем веса матрицы М
                        for (int N = 0; N < rowP; N++)
                        {
                            for (int K = 0; K < colP; K++)
                            {
                                // нужна проверка для Fr(N, K) не равно нулю, если равно, то выходим из расчета
                                if (Fr.At(N, K) != 0)
                                {
                                    dM = KdM * (Fkn.At(N, K) - Fr.At(N, K)) * M.At(N, K) / Fr.At(N, K);
                                    M[N, K] = Math.Round(M.At(N, K) + dM, 4);                                    
                                    //M[N, K] = M.At(N, K) + dM;
                                }
                                else
                                {
                                    flagExitCalc = true;
                                    break;
                                }
                                
                            }
                            if (flagExitCalc)
                            {
                                break;
                            }
                        }
                        
                        // Перерассчитываем коэф.полинома В для новой матрицы весом М
                        B = CalcB(rowP, colP, M, Pn, Umtx, Rmtx);

                        // Перерассчитываем фактические отклонения Fkn для новых коэф В
                        Fkn = CalcFkn(rowP, colP, B, Rmtx, Umtx, Pn, Kp);

                        // Определяем кол - во точек в допуске
                        Ndop = CalcNdop(rowP, colP, Fkn, Fr);

                        // Если все точки в допуске решение найдено
                        if (Ndop == Nmax)
                        {
                            flagFindR = true;
                            break;
                        }
                        // если нет
                        else
                        {

                            // Находим максимальное значение эл.матрицы М
                            maxValueM = maxValMatrix(M);

                            // Проверяем превышают ли веса М максимально допустимое Mmax
                            if (maxValueM > Mmax)
                            {
                                // если да, то выходим из цикла по Amax
                                break;
                            }
                        } // для  if (Ndop == Nmax)
                    } //  for (int a = 0; a < Amax; a++)
                }
                else
                {
                    // Решение найдено, устанавливаем флаг
                    flagFindR = true;
                } // для if (Ndop ~= Nmax)

                //  Если решение найдено(флаг = 1) выходим из цикла
                if (flagFindR)
                {
                    Kf_opt = Kf;
                    break;
                }              
               
            } // для for Kf = 1 : 1 : 10

            if ((Kf == Kpmax_dop)&&(flagFindR == false))
            {
                // Изменил в условие "Kf == 10" на условие "Kf == Kpmax_dop" ПЕРЕИМЕНОВАТЬ Kf в Kf_opt до конца кода
                Kf_opt = 6;                
            }



            // ---------- ЭТАП - 3 -----------------------------------------------------
            //  ОСНОВНОЙ АЛГОРИТМ РАСЧЕТА
            // ПОИСК ОПТИМАЛЬНОГО РЕШЕНИЯ
            // -------------------------------------------------------------------------

           
            //  Рассчет r_opt
            double r = 0;
            double r_opt;
            maxValueM = 0;

            for (int N = 0; N < rowP; N++)
            {
                for (int K = 0; K < colP; K++)
                {
                    r = r + Math.Abs(Fkn1.At(N, K) / Fdop.At(N, K));
                }
            }           
            r_opt = r/Nmax;
        

            double Kmult = 1;           
            while (Kmult >= 0.1)
            {
                // Задаем, обновляем переменные
                Tdw = 0;                    // счетчик всех циклов do -while
                Tdw_opt = 0;                 // количество найденных не оптимальных решений подряд
                Tdw_opt_max = 50;           // макс.кол - во решений в допуске но НЕ оптимальных
                Tdw_out_res = 0;            // промежуточный счетчик циклов подряд без решения
                Tdw_out_res_max = 50;       // предельное значение промежуточного счетчика циклов подряд без решения
                Tdw_out_res_sum = 0;        // общий счетчик циклов в сумме без решения
                Tdw_out_res_sum_max = 100;  // предельное количество циклов в сумме без решения
                Res_count = 0;              // счетчик найденных решений
                Res_count_max = 100;        // максимальное количество найденных решений
                Exit_dw = false;            // критерий выхода из цикла do -while

                // Формируем матрицу единичных весовов М
                M = M1;  
                M = DenseMatrix.Create(rowP, colP, 1);

                // Вычисляем матрицу фактических отклонеий Fkn
                Fkn = Fkn1;

                // Формируем стартовую рассчетную границу(для заданного Kmult)
                Fr = Fdop.Multiply(Kmult);

                while (Exit_dw == false)
                {
                    flagExitCalc = false;
                    Tdw = Tdw + 1;   // Считаем количество всех циклов do -while
                    Tdw_out_res_sum = Tdw_out_res_sum + 1;     // Считаем количество всех циклов do -while в сумме без решения, независимо от того было ли найдено решение или нет

                    // Расчет коэффициентов B
                    B = CalcB(rowP, colP, M, Pn, Umtx, Rmtx);

                    // Рассчитываем фактические отклонения Fkn(формула 5, стр. 10)
                    Fkn = CalcFkn(rowP, colP, B, Rmtx, Umtx, Pn, Kp);
                  

                    // Определяем кол - во точек в допуске
                    Ndop = CalcNdop(rowP, colP, Fkn, Fr);

                    // Если все точки в допуске решение найдено
                    if (Ndop == Nmax)
                    {
                        Tdw_opt = Tdw_opt + 1;    // Считаем количество найденных решений подряд, как только решение отсутствует, параметр обнуляется
                        // Расчет среднего значения относительного отклонения по модулю
                        r = 0;

                        for (int N = 0; N < rowP; N++)
                        {
                            for (int K = 0; K < colP; K++)
                            {
                                r = Math.Round(r + Math.Abs(Fkn.At(N, K)/Fdop.At(N, K)), 4);
                            }
                        }
                        r = r / Nmax;

                        // Сравниваем текущую r с r_opt, если r меньше, то перезаписываем r_opt
                        if (r < r_opt)
                        {
                            // Оптимальное решение найдено
                            r_opt = r;
                            M_opt = M;
                            Kmult_opt = Kmult;
                            Tdw_opt = 0;
                        }

                        // Считаем общее количество найденных решений, независимо от того, было ли отсутствие решения
                        Res_count = Res_count + 1;
                        Tdw_out_res = 0;   // обнуляем промежуточный счетчик циклов без решения, в связи с найденным решением
                        // Понижение расчетной границы Fr
                        Fr = CalcFr_cur(rowP, colP, Fr, Fkn, deltaFdop_min, Fr_min, deltaFkn_min);
            
                    //если не все точки в допуске(решение не найдено)
                    }
                    else
                    {
                         // увеличиваем промежуточный счетчик циклов без решения
                        Tdw_out_res = Tdw_out_res + 1;
                    }

                    // Находим максимальное значение элементов матрицы М                    
                    maxValueM = maxValMatrix(M);

                    // Проверка условий выхода их цикла do -while
                    // 1.Если общее количество найденных решений Res_count, независимо от того, было ли отсутствие решения, больше, чем максимальное
                    // значение этого параметра Res_count_max, то выход из do -while
                    // 2.Если максимальное значение элементов матрицы М maxVal больше, чем Mmax, то выходим
                    // 3.Если общий счетчик циклов в сумме без решения
                    // Tdw_out_res_sum больше, чем допускаемое количество Tdw_out_res_sum_max, то выходим
                    // 4.Если количество найденных не оптимальных решений подряд Tdw_opt больше, чем допускаемое количество Tdw_opt_max, то выходим. 

                    if ((Res_count > Res_count_max) || (maxValueM > Mmax) || (Tdw_out_res_sum > Tdw_out_res_sum_max) || (Tdw_opt > Tdw_opt_max))
                    {
                        Exit_dw = true;  // критерий выхода из цикла do -while
                    }


                    // Перерассчитываем веса матрицы М
                    for (int N = 0; N < rowP; N++)
                    {
                        for (int K = 0; K < colP; K++)
                        {
                            // нужна проверка для Fr(N, K) не равно нулю, если равно, то выходим из расчета
                            if (Fr.At(N, K) != 0)
                            {
                                dM = KdM * (Fkn.At(N, K) - Fr.At(N, K)) * M.At(N, K) / Fr.At(N, K);                              
                                M[N, K] = M.At(N, K) + dM;
                                nm++;
                            }
                            else
                            {
                                flagExitCalc = true;
                                break;
                            }

                        }
                        if (flagExitCalc)
                        {
                            break;
                        }
                    }


                } // для while(Exit_dw == 0)  

                Kmult = Kmult - 0.1;
            } // для for Kmult = 1 : -0.1 : 0.1


        

        // ----------ЭТАП - 4--------------------
        // ОБРАБОТКА РЕЗУЛЬТАТОВ
        // --------------------------------------
        nm = nm + 0;
            // Расчет коэффициентов B
            Matrix<double> BmtxRes = DenseMatrix.Create(24, 1, 0);
            // Расчет коэффициентов B
            BmtxRes = CalcB(rowP, colP, M_opt, Pn, Umtx, Rmtx);
/*
            BmtxRes[0,0] = 0.527342609543531;
            BmtxRes[1, 0] = -3.865293182609505e-04;
            BmtxRes[2, 0] = 1.026300362063800e-07;
            BmtxRes[3, 0] = -9.333675118220151e-12;
            BmtxRes[4, 0] = -0.123656171426227;
            BmtxRes[5, 0] = 1.216973142593132e-04;
            BmtxRes[6, 0] = -3.311124723859655e-08;
            BmtxRes[7, 0] = 2.963912621442306e-12;
            BmtxRes[8, 0] = -0.012695803795669;
            BmtxRes[9, 0] = 1.127025347700269e-05;
            BmtxRes[10, 0] = -3.319588952698123e-09;
            BmtxRes[11, 0] = 3.244175414820060e-13;
            BmtxRes[12, 0] = 0.001335065164757;
            BmtxRes[13, 0] = -1.176518217828281e-06;
            BmtxRes[14, 0] = 3.442472834860238e-10;
            BmtxRes[15, 0] = -3.344181719312444e-14;
            BmtxRes[16, 0] = -4.494610686899630e-05;
            BmtxRes[17, 0] = 3.950749426694677e-08;
            BmtxRes[18, 0] = -1.153243046044599e-11;
            BmtxRes[19, 0] = 1.117859453310178e-15;
            BmtxRes[20, 0] = 4.844398720691186e-07;
            BmtxRes[21, 0] = -4.252267011775080e-10;
            BmtxRes[22, 0] = 1.239627318210713e-13;
            BmtxRes[23, 0] = -1.200112421064509e-17;
          */


             // Рассчитываем фактические отклонения Fkn(формула 5, стр. 10)
             Fkn = CalcFkn(rowP, colP, BmtxRes, Rmtx, Umtx, Pn, Kp);

            // ФУНКЦИЯ ОБНУЛЕНИЯ ДАТЧИКА
            //-------------------------------------------------------------------------
            // В этой функции серьезные проблемы с универсальностью. Буду думать уже в
            // отпуске. Не успел подумать. Точка по давлению 0 может отсутствовать! И
            // как быть в этом случае. Обнулить ближайшую к нулю точку по давлению? Буду
            // думать.А можно точку ноль аппроксимировать и потом обнулять ?
            // Пока это работает для ДД, ДИ, ДВ.
            //-------------------------------------------------------------------------

            // Ищем координаты точки с нулевым давлением и на НКУ.
            int col = -1;
            for (int i = 0; i < Tmtx.ColumnCount; i++)
            {
                if (Tmtx.At(0, i) == 23)
                {
                    col = i+1;
                    break;
                }                 
            }
            if (col == -1)
            {
                resultBmtx = DenseMatrix.Create(1, 1, -4);
                return resultBmtx;
            }


            int row = -1;
            for (int i = 0; i < rowP; i++)
            {
                if (Pmtx.At(i, col) == 0)
                {
                    row = i+1;
                    break;
                }                    
            }
            if (row == -1)
            {
                resultBmtx = DenseMatrix.Create(1, 1, -5);
                return resultBmtx;
            }

            // Ищем фактические отклонения, но с учетом знака(не по модулю как в CalcFkn)
            Matrix<double> Fkn_sign = DenseMatrix.Create(rowP, colP, 0);

            // цикл по N(строкам матриц M, P, R, U)
            for (int N = 0; N < rowP; N++)
            {
                // цикл по K(столбцам матриц M, P, R, U)
                for (int K = 0; K < colP; K++)
                {
                    double Fi = 0;
                    int m = 0;
                    // цикл по j
                    for (int j = 0; j < 6; j++)
                    {
                        // цикл по i
                        for (int i = 0; i < 4; i++)
                        {
                            Fi = Fi + BmtxRes.At(m,0) * Math.Pow(Rmtx.At(N, K), i) * Math.Pow(Umtx.At(N, K), j); 
                            m = m + 1;                           
                        }
                    }       
                    Fkn_sign[N, K] = (Fi - Pn.At(N, K)) * 100 * Kp.At(N, K);
                }
            }


            // Обнуляем фактические отклонения по значению на НКУ и в 0 точке
            for (int N = 0; N < rowP; N++)
                {
                for (int K = 0; K < colP; K++)
                {
                    Fkn[N, K] = Math.Abs(Fkn_sign.At(N, K) / Kp.At(N, K) - Fkn_sign.At(row, col) / Kp.At(row, col)) * Kp.At(N, K);
                }   
            }

            // Тип датчика

            Vector<double> code_acc = DenseVector.Create(gammaTa.RowCount, 0);
            code_acc = gammaTa.Column(0, 0, gammaTa.RowCount);
                        
            int code_opt = -1;

            // окончательная допускаемая граница по которой определяем код точности с учетом коэф. запаса
            Matrix<double> Fdop_res = DenseMatrix.Create(rowP, colP, 0);
            Matrix<double> gamma_P = DenseMatrix.Create(1, 2, 0);
            Matrix<double> gamma_T = DenseMatrix.Create(1, 2, 0);

            for (int cc = 1; cc < code_acc.Count; cc++)
            {
                curCode = Convert.ToInt32(gammaPa.At(cc, 0));
                for (int i = 0; i < rowP; i++)
                {
                    for (int j = 0; j < colP; j++)
                    {
                        if (Kp.At(i, j) < Kf)
                        {
                            // Получаем коэффициенты а и b осн.приведенной погрешности
                            gamma_P = CalcGammaP(Kp.At(i, j), curCode, gammaPa, gammaPb);
                            // Получаем коэффициенты а и b доп.температурной погрешности
                            gamma_T = CalcGammaT(curCode, sens, gammaTa, gammaTb);
                            // Расчет Fdop
                            Fdop_res[i, j] = ((gamma_P.At(0, 0) + gamma_P.At(0, 1) * Kp.At(i, j)) + (gamma_T.At(0, 0) + gamma_T.At(0, 1) * Kp.At(i, j)) * (Math.Abs(Tmtx.At(0, j) - Tnku) / 10))*(0.5 + 0.05 * (Math.Abs(Tmtx.At(0, j) - Tnku) / 10)) * 0.9;
                        }

                        else
                        {
                            // Получаем коэффициенты а и b осн.приведенной погрешности
                            gamma_P = CalcGammaP(Kf, curCode, gammaPa, gammaPb);
                            // Получаем коэффициенты а и b доп.температурной погрешности
                            gamma_T = CalcGammaT(curCode, sens, gammaTa, gammaTb);
                            // Расчет Fdop
                            Fdop_res[i, j] = ((gamma_P.At(0, 0) + gamma_P.At(0, 1) * Kf) + (gamma_T.At(0, 0) + gamma_T.At(0, 1) * Kf) * (Math.Abs(Tmtx.At(0, j) - Tnku) / 10)) *(0.5 + 0.05 * (Math.Abs(Tmtx.At(0, j) - Tnku) / 10)) * 0.9;
                        }
                    }
                }

                Ndop = CalcNdop(rowP, colP, Fkn, Fdop_res);


                if (Ndop == Nmax)
                {
                    code_opt = curCode;
                    break;
                }
            }

            if (code_opt == -1)
            {
                resultBmtx = DenseMatrix.Create(1, 1, -1);  // возвращаем: -6 не попали ни в какой код точности
                return resultBmtx;
            }

            resultBmtx = DenseMatrix.Create(24, 1, 0);            
            resultBmtx = BmtxRes;            
            return resultBmtx;

        }





        // ----------------------------------------------------------------------------------
        // --------ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ --------------------------------------------------                                    
        // ----------------------------------------------------------------------------------



        // ----------------------------------------------------------------------------------
        // ФУНКЦИЯ для расчета весовых коэффициентов B - полинома
        // ----------------------------------------------------------------------------------
        public Matrix<double> CalcB(int rowP, int colP, Matrix<double> M, Matrix<double> Pn, Matrix<double> Umtx, Matrix<double> Rmtx)
        {
            Matrix<double> Bmtx;  // Формируем матрицу - столбец Bmtx размером 24 строки
            Matrix<double> Cmtx = DenseMatrix.Create(24, 1, 0);     // Формируем матрицу - столбец С размером 24 строки
            Matrix<double> Amtx = DenseMatrix.Create(24, 24, 0);    // Формируем матрицу - столбец Bmtx размером 24 строки

            for (int q = 0; q < 6; q++)
            {
                for (int p = 0; p < 4; p++)
                {
                    double Cn = 0;
                    // цикл по K (столбцам матриц M, P, R, U)
                    for (int K = 0; K < colP; K++)
                    {
                        // цикл по N (строкам матриц M, P, R, U)
                        for (int N = 0; N < rowP; N++)
                        {
                            //результат: сумма по строкам                          
                            Cn = Cn + M.At(N, K) * Pn.At(N, K) * Math.Pow(Rmtx.At(N, K), p) * Math.Pow(Umtx.At(N, K), q);
                        } // для N
                    } // для K
                    Cmtx[p + 4 * q, 0] = Cn;
                } // для p
            } // для q


            // Формируем матрицу - Amtx размером 24x24            
            for (int q = 0; q < 6; q++)
            {
                for (int p = 0; p < 4; p++)
                {
                    // цикл по j
                    for (int j = 0; j < 6; j++)
                    {
                        // цикл по i
                        for (int i = 0; i < 4; i++)
                        {
                            double An = 0;
                            // цикл по K(столбцам матриц M, P, R, U)
                            for (int K = 0; K < colP; K++)
                            {
                                // цикл по N(строкам матриц M, P, R, U)
                                for (int N = 0; N < rowP; N++)
                                {
                                    // результат: сумма по строкам
                                    An = An + M.At(N, K) * Math.Pow(Rmtx.At(N, K), i) * Math.Pow(Umtx.At(N, K), j) * Math.Pow(Rmtx.At(N, K), p) * Math.Pow(Umtx.At(N, K), q);
                                } // для N
                            } // для K
                            Amtx[p + 4 * q, i + 4 * j] = An;
                        } // по i
                    } // для j
                } // по p
            } // по q


            // РЕШЕНИЕ
            // Проверяем, если определитель матрицы Amtx равен нулю - то нет решения,
            if (Amtx.Determinant() == 0)
            {
                Bmtx = DenseMatrix.Create(1, 1, -1);       // если решения нет возвращаем -2
                return Bmtx;
            }

            // если определитель не равен нулю находим коэффиценты B
            Bmtx = DenseMatrix.Create(24, 1, 0);
            Bmtx = Amtx.Solve(Cmtx);

            // другой метод
            //Bmtx = Cmtx.Multiply(Amtx.Inverse());


            return Bmtx;
        }

      
        // ----------------------------------------------------------------------------------
        // ФУНКЦИЯ для расчета допустимых отклонений(допускаемой погрешности)
        // ----------------------------------------------------------------------------------
        public Matrix<double> CalcFdop(int rowP, int colP, int Kf, Matrix<double> Kp, int code, int sens, Matrix<double> Tmtx, double Tnku, Matrix<double> gammaPa, Matrix<double> gammaPb)
        {
            // Создаем матрицу Fdop(размерностью N, K)
            Matrix<double> Fdop = DenseMatrix.Create(rowP, colP, 0);
            Matrix<double> gamma_P = DenseMatrix.Create(1, 2, 0);
            Matrix<double> gamma_T = DenseMatrix.Create(1, 2, 0);


            for (int i = 0; i < rowP; i++)
            {
                for (int j = 0; j < colP; j++)
                {
                    if (Kp.At(i, j) < Kf)
                    {
                        // Получаем коэффициенты а и b осн.приведенной погрешности
                        gamma_P = CalcGammaP(Kp.At(i, j), code, gammaPa, gammaPb);
                        // Получаем коэффициенты а и b доп.температурной погрешности
                        gamma_T = CalcGammaT(code, sens, gammaTa, gammaTb);
                        // Расчет Fdop
                        Fdop[i, j] = (gamma_P.At(0,0) + gamma_P.At(0, 1) * Kp.At(i, j)) + (gamma_T.At(0,0) + gamma_T.At(0, 1) * Kp.At(i, j)) * (Math.Abs(Tmtx.At(0,j) - Tnku)/10);
           
                    }

                    else
                    {
                        // Получаем коэффициенты а и b осн.приведенной погрешности
                        gamma_P = CalcGammaP(Kf, code, gammaPa, gammaPb);
                        // Получаем коэффициенты а и b доп.температурной погрешности
                        gamma_T = CalcGammaT(code, sens, gammaTa, gammaTb);
                        // Расчет Fdop
                        Fdop[i, j] = (gamma_P.At(0, 0) + gamma_P.At(0, 1) * Kf) + (gamma_T.At(0, 0) + gamma_T.At(0, 1) * Kf) * (Math.Abs(Tmtx.At(0,j) - Tnku)/10);
                    }
                }
            }

            return Fdop;
        }




        // ----------------------------------------------------------------------------------
        // Необходимо реализовать загрузку из файла данных с учетом типа датчика
        // давления(ЭнИ-100, ЭнИ-12, или может появиться другой тип)
        
          
        //----------------------------------------------------------------------------------    
        // ФУНКЦИЯ для расчета коэффициентов a и b
        // пределов допускаемой основной приведенной погрешности
        // Входные данные: 
        // code - код погрешности 
        // Kp - диапазон перенастройки
        // Выходные данные:
        // gamma_Pa - коэф.а
        // gamma_Pb - клэф.b

        public Matrix<double> CalcGammaP(double Kp, int code, Matrix<double> gammaPa, Matrix<double> gammaPb)
        {

            Matrix<double> gamma_P = DenseMatrix.Create(1, 2, -1);
            int ii = -1;
            int jj = -1;
                      

            int rowPa = gammaPa.RowCount;       // кол-во строк
            int colPa = gammaPa.ColumnCount;    // кол-во столбцов


            // По коду ошибки определяем индекс строки
            for (int i = 0; i < rowPa; i++)
            {
                if (gammaPa.At(i, 0) == code)
                {
                    ii = i;
                    break;
                }               
            }


            // По коду сравнению с Kp определяем номер столбца
            for (int j = 0; j < colPa-1; j++)
            {
                if ((Kp > gammaPa.At(0, j)) &&(Kp <= gammaPa.At(0, j + 1)))
                {
                    jj = j + 1;
                    break;
                }
            }


            // Проверка
            if((ii != -1)&&(ii <= rowPa)&&(jj != -1)&&(jj <= colPa))
            {
                gamma_P[0, 0] = gammaPa.At(ii, jj);
                gamma_P[0, 1] = gammaPb.At(ii, jj);
            }

            return gamma_P;

        }






        //----------------------------------------------------------------------------------    
        // ФУНКЦИЯ для расчета коэффициентов a и b
        // пределов дополнительной температурной погрешности
        // Вхоные данные: 
        // code - код погрешности 
        // sens - номер столбца(в зависимости от вида давления (типа датчика) 1 или 2)
        // Выходные данные:
        // gamma_Ta - коэф.а
        // gamma_Tb - клэф.b

        public Matrix<double> CalcGammaT(int code, int sens, Matrix<double> gammaTa, Matrix<double> gammaTb)
        {

            Matrix<double> gamma_T = DenseMatrix.Create(1, 2, -1);
            int ii = -1;
           

            int rowTa = gammaTa.RowCount;       // кол-во строк
            int colTa = gammaTa.ColumnCount;    // кол-во столбцов


            // По коду ошибки определяем индекс строки
            for (int i = 0; i < rowTa; i++)
            {
                if (gammaTa.At(i, 0) == code)
                {
                    ii = i;
                    break;
                }
            }

            // Проверка
            if ((ii != -1)&&(ii <= rowTa)&&(sens <= colTa))
            {
                gamma_T[0, 0] = gammaTa.At(ii, sens);
                gamma_T[0, 1] = gammaTb.At(ii, sens);
            }

            return gamma_T;
        }


        //----------------------------------------------------------------------------------    
        // ФУНКЦИЯ для расчета фактических отклонений        
     
        public Matrix<double> CalcFkn(int rowP, int colP, Matrix<double> B, Matrix<double> Rmtx, Matrix<double> Umtx, Matrix<double> Pn, Matrix<double> Kp)
        {
            Matrix<double> Fkn = DenseMatrix.Create(rowP, colP, -1);
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
                            Fi = Fi + B.At(m,0) * Math.Pow(Rmtx.At(N, K), i) * Math.Pow(Umtx.At(N, K), j);                            
                            m = m + 1;
                        }
                    }
                    Fkn[N, K] = Math.Abs((Fi - Pn.At(N, K)) * 100 * Kp.At(N, K));
                }
            }            
            return Fkn;
        }


        //----------------------------------------------------------------------------------    
        // ФУНКЦИЯ для расчета фактических отклонений        

        public int CalcNdop(int rowP, int colP, Matrix<double> Fkn, Matrix<double> Fr)
        {
            int Ndop = 0;

            // Определяем кол - во точек в допуске
            for (int N = 0; N < rowP; N++)
            {
                //цикл по K(столбцам матриц M, P, R, U)
                for (int K = 0; K < colP; K++)
                {
                    if (Fkn.At(N, K) < Fr.At(N, K))
                    {
                        Ndop = Ndop + 1;
                    }
                }
            }
            return Ndop;
        }




        //----------------------------------------------------------------------------------    
        // ФУНКЦИЯ для расчета отклонений для формирования новой границы        

        public Matrix<double> CalcFr_cur(int rowP, int colP, Matrix<double> Fr, Matrix<double> Fkn, double deltaFdop_min, double Fr_min, double deltaFkn_min)
        {
            Matrix<double> Fr_cur = Fr;
            double deltaFkn;

            for (int N = 0; N < rowP; N++)
            {
                for (int K = 0; K < colP; K++)
                {
                    deltaFkn = Fr.At(N, K) - Math.Abs(Fkn.At(N, K));
                    // Поиск минимального шага
                    if (deltaFkn < deltaFkn_min)
                    {
                        deltaFkn_min = deltaFkn;
                    }
                }
            }

            // Проверка что мин.шаг не меньше допускаемого
            if (deltaFkn_min < deltaFdop_min)
            {
                deltaFkn_min = deltaFdop_min;
            }

            // Проверка
            if (deltaFkn_min == 100)
            {
                deltaFkn_min = deltaFdop_min;
            }

            // Опускаем расчетную границу
            for (int N = 0; N < rowP; N++)
            {
                for (int K = 0; K < colP; K++)
                {
                    Fr_cur[N, K] = Fr.At(N, K) - deltaFkn_min;

                    if (Fr_cur.At(N, K) < Fr_min)
                    {
                        Fr_cur[N, K] = Fr_min;
                    }
                }
            }
            
            return Fr_cur;
        }




        //----------------------------------------------------------------------------------    
        // ФУНКЦИЯ определения максимального значения элемента матрицы   
        public double maxValMatrix(Matrix<double> Mtx)
        {
            double MaxValue = -1;

            for (int N = 0; N < Mtx.RowCount; N++)
            {
                for (int K = 0; K < Mtx.ColumnCount; K++)
                {                    

                    if (Mtx.At(N, K) > MaxValue)
                    {
                        MaxValue = Mtx.At(N, K);
                    }
                }
            }
            return MaxValue;
        }















        }
}
