using System;
using System.Collections.Generic;
using PayrollSystem.Domains.Abstract;

namespace PayrollSystem
{
    /// <summary>
    /// Service for calculation salary
    /// </summary>
    public interface ICalculationSalaryService
    {
        /// <summary>
        /// Calculates the worker salary.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <param name="calculationDate">The calculation date.</param>
        /// <returns></returns>
        decimal CalculateWorkerSalary(Worker worker, DateTime calculationDate);

        /// <summary>
        /// Calculates the workers salaries in sum.
        /// </summary>
        /// <param name="workers">The workers.</param>
        /// <param name="calculationDate">The calculation date.</param>
        /// <returns></returns>
        decimal CalculateWorkersSalariesInSum(List<Worker> workers, DateTime calculationDate);
    }
}
