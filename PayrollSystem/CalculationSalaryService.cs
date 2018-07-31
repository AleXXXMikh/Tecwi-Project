using System;
using System.Collections.Generic;
using System.Linq;
using PayrollSystem.Domains;
using PayrollSystem.Domains.Abstract;
using PayrollSystem.Domains.Enum;

namespace PayrollSystem
{
    public class CalculationSalaryService : ICalculationSalaryService
    {
        #region Employee constants

        /// <summary>
        /// The maximum employee surcharge is 30%
        /// </summary>
        private const decimal MAXIMUM_EMPLOYEE_SURCHARGE = 0.3m;

        /// <summary>
        /// The employee surcharge per year is 3%
        /// </summary>
        private const decimal EMPLOYEE_SURCHARGER_PER_YEAR = 0.03m;

        #endregion

        #region Manager constants

        /// <summary>
        /// The maximum manager surcharge is 40%
        /// </summary>
        private const decimal MAXIMUM_MANAGER_SURCHARGER = 0.4m;

        /// <summary>
        /// The manager surcharge per year is 5%
        /// </summary>
        private const decimal MANAGER_SURCHARGER_PER_YEAR = 0.05m;

        /// <summary>
        /// The manager surcharger from subordinates of first level is 0.5%
        /// </summary>
        private const decimal MANAGER_SURCHARGER_FROM_SUBORDINATES = 0.005m;

        #endregion 

        #region Sales constants

        /// <summary>
        /// The maximum sales surcharge is 35%
        /// </summary>
        private const decimal MAXUMUM_SALES_SURCHARGER = 0.35m;

        /// <summary>
        /// The sales surcharge per year is 1%
        /// </summary>
        private const decimal SALES_SURCHARGER_PER_YEAR = 0.01m;

        /// <summary>
        /// The sales surcharger from subordinates of all levels is 0.3%
        /// </summary>
        private const decimal SALES_SURCHARGER_FROM_SUBORDINATES = 0.003m;
        #endregion

        #region Public

        public decimal CalculateWorkerSalary(Worker worker, DateTime calculationDate)
        {
            if (worker.EmploymentDate > calculationDate)
            {
                // Worker didn't work at that time in the company
                return 0;
            }

            switch (worker.WorkerType)
            {
                case WorkerType.Employee:
                {
                    return CalculateEmployeeSalary((Employee)worker, calculationDate);
                }

                case WorkerType.Manager:
                {
                    return CalculateManagerSalary((Manager)worker, calculationDate);
                }

                case WorkerType.Sales:
                {
                    return CalculateSalesSalary((Sales)worker, calculationDate);
                }
                default:

                    throw new Exception($"Unknown WorkerType enum value {worker.WorkerType}");
            }
        }

        public decimal CalculateWorkersSalariesInSum(List<Worker> workers, DateTime calculationDate)
        {
            return workers.Sum(w => CalculateWorkerSalary(w, calculationDate));
        }

        #endregion

        #region Private

        private decimal CalculateSurchargeForWorkedYears(Worker worker, DateTime calculationDate, decimal surchargePerYear, decimal maximumSurcharge)
        {
            DateTime employmentDate = worker.EmploymentDate;

            int workedYears = 0;
            employmentDate = employmentDate.AddYears(1);
            // getting number of worked years
            while (calculationDate >= employmentDate)
            {
                workedYears++;
                employmentDate = employmentDate.AddYears(1);
            }

            var surchargeSumInPercent = workedYears * surchargePerYear;

            // Checking limit
            if (surchargeSumInPercent > maximumSurcharge)
            {
                surchargeSumInPercent = maximumSurcharge;
            }

            return worker.BasePaymentRate * surchargeSumInPercent;
        }

        private decimal CalculateEmployeeSalary(Employee employee, DateTime calculationDate)
        {
            var salary = employee.BasePaymentRate + CalculateSurchargeForWorkedYears(employee, calculationDate, EMPLOYEE_SURCHARGER_PER_YEAR, MAXIMUM_EMPLOYEE_SURCHARGE);

            return salary;
        }

        private decimal CalculateManagerSalary(Manager manager, DateTime calculationDate)
        {
            var salary = manager.BasePaymentRate + CalculateSurchargeForWorkedYears(manager, calculationDate, MANAGER_SURCHARGER_PER_YEAR, MAXIMUM_MANAGER_SURCHARGER);

            salary += CalculateWorkersSalariesInSum(manager.Subordinates, calculationDate) * MANAGER_SURCHARGER_FROM_SUBORDINATES;

            return salary;
        }

        private decimal CalculateSalesSalary(Sales sales, DateTime calculationDate)
        {
            var salary = sales.BasePaymentRate + CalculateSurchargeForWorkedYears(sales, calculationDate, SALES_SURCHARGER_PER_YEAR, MAXUMUM_SALES_SURCHARGER);

            salary += CalculateAllLevelsWorkerSalaries(sales.Subordinates, calculationDate) * SALES_SURCHARGER_FROM_SUBORDINATES;

            return salary;
        }

        private decimal CalculateAllLevelsWorkerSalaries(List<Worker> workers, DateTime calculationDate)
        {
            decimal sum = 0;
            foreach (var worker in workers)
            {
                if (worker is ChiefWorker chiefWorker && chiefWorker.Subordinates.Any())
                {
                    sum += CalculateWorkerSalary(chiefWorker, calculationDate) +
                           CalculateAllLevelsWorkerSalaries(chiefWorker.Subordinates, calculationDate);
                }
                else
                {
                    sum += CalculateWorkerSalary(worker, calculationDate);
                }
            }

            return sum;
        }

        #endregion
    }
}
