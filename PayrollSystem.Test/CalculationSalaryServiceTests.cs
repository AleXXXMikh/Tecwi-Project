using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PayrollSystem.Domains;
using PayrollSystem.Domains.Abstract;

namespace PayrollSystem.Test
{
    public class CalculationSalaryServiceTests
    {
        private ICalculationSalaryService _calculationSalaryService;
        private List<Worker> _workers;
        private DateTime _claculationDateTime;

        [SetUp]
        public void SetUp()
        {
            SeedTestData();

            _calculationSalaryService = new CalculationSalaryService();
        }

        #region Emploee

        [Test]
        public void CalculateWorkerSalary_CalculateEmploeeSalary_ReturnsSalary()
        {
            var emplyee = _workers.Single(w => w.Id == 1);

            var salary = _calculationSalaryService.CalculateWorkerSalary(emplyee, _claculationDateTime);

            // 200 + (200 * (0.03 * 5)) = 230
            Assert.AreEqual(230, salary);
        }

        [Test]
        public void CalculateWorkerSalary_CalculateEmploeeSalaryWhenMaxSurcharger_ReturnsSalary()
        {
            var emplyee = _workers.Single(w => w.Id == 2);

            var salary = _calculationSalaryService.CalculateWorkerSalary(emplyee, _claculationDateTime);

            // 11 years
            // 200 + (200 * (0.3)) = 260
            Assert.AreEqual(260, salary);
        }

        #endregion

        #region Manager

        [Test]
        public void CalculateWorkerSalary_CalculateManagerSalaryWithoutSubordinates_ReturnsSalary()
        {
            var manager = _workers.Single(w => w.Id == 3);

            var salary = _calculationSalaryService.CalculateWorkerSalary(manager, _claculationDateTime);

            // 200 + (200 * (0.05 * 5)) = 250
            Assert.AreEqual(250, salary);
        }

        [Test]
        public void CalculateWorkerSalary_CalculateManagerSalaryWithoutSubordinatesWhenMaxSurcharger_ReturnsSalary()
        {
            var manager = _workers.Single(w => w.Id == 4);

            var salary = _calculationSalaryService.CalculateWorkerSalary(manager, _claculationDateTime);
            // 9 years
            // 200 + (200 * (0.4)) = 280
            Assert.AreEqual(280, salary);
        }

        [Test]
        public void CalculateWorkerSalary_CalculateManagerSalaryWitSubordinates_ReturnsSalary()
        {
            var manager = _workers.Single(w => w.Id == 5);

            var salary = _calculationSalaryService.CalculateWorkerSalary(manager, _claculationDateTime);

            // (200 + (200 * (0.05 * 4)) = 240) + ((230 + 260 + 250 + 280) * 0.005 = 5.1) = 245.1
            Assert.AreEqual(245.1, salary);
        }

        #endregion

        #region Sales
        [Test]
        public void CalculateWorkerSalary_CalculateSalesSalaryWithoutSubordinates_ReturnsSalary()
        {
            var sales = _workers.Single(w => w.Id == 6);

            var salary = _calculationSalaryService.CalculateWorkerSalary(sales, _claculationDateTime);

            // (200 + ((200 * 0,2) = 40)) = 240
            Assert.AreEqual(240, salary);
        }

        [Test]
        public void CalculateWorkerSalary_CalculateSalesSalaryWithoutSubordinatesMaxSurcharger_ReturnsSalary()
        {
            var sales = _workers.Single(w => w.Id == 7);

            var salary = _calculationSalaryService.CalculateWorkerSalary(sales, _claculationDateTime);

            // 40 years
            // (200 + ((200 * 0,35) = 70)) = 270
            Assert.AreEqual(270, salary);
        }

        [Test]
        public void CalculateWorkerSalary_CalculateSalesSalaryWitSubordinates_ReturnsSalary()
        {
            var sales = _workers.Single(w => w.Id == 8);
            var salary = _calculationSalaryService.CalculateWorkerSalary(sales, _claculationDateTime);

            // (2 lvl) = 230 + 260 + 250 + 280 = 1020
            // (1 lvl) = 245.1 + 240 + 270 = 755.1
            // base with years = (200 + ((200 * 0,3) = 60)) = 260
            // (0.003 * (1lvl + 2lvl = 1775.1) = 5.3253)  + base with years = 265.3253
            Assert.AreEqual(265.3253, salary);
        }

        #endregion

        [Test]
        public void CalculateWorkersSalariesInSum_CalculateWorkersSalariesSum_ReturnsSalariesInSum()
        {
            var salary = _calculationSalaryService.CalculateWorkersSalariesInSum(_workers, _claculationDateTime);

            // 230 + 260 + 250 + 280 + 245.1 + 240 + 270 + 265.3253 = 2040.4253
            Assert.AreEqual(2040.4253, salary);
        }

        private void SeedTestData()
        {
            _claculationDateTime = new DateTime(2021, 2, 28);

            _workers = new List<Worker>
            {
                //230
                DomainFactory.CreateEmployee(1, new DateTime(2016, 2, 29)),
                //260
                DomainFactory.CreateEmployee(2, new DateTime(2010, 2, 28)),
                //250
                DomainFactory.CreateManager(3, new DateTime(2016, 2, 29)),
                //280
                DomainFactory.CreateManager(4, new DateTime(2012, 2, 29))
            };

            // (200 + (200 * (0.05 * 4)) = 240) + ((230 + 260 + 250 + 280) * 0.005 = 5.1) = 245.1
            var manager5 = DomainFactory.CreateManager(5, new DateTime(2017, 2, 28));

            foreach (var worker in _workers)
            {
                manager5.AddSubordinates(worker);
            }

            _workers.Add(manager5);
            // (200 + ((200 * 0,2) = 40)) = 240
            var sales6 = DomainFactory.CreateSales(6, new DateTime(2001, 2, 28));

            _workers.Add(sales6);
            // 270
            var sales7 = DomainFactory.CreateSales(7, new DateTime(1981, 2, 28));

            _workers.Add(sales7);
            // 265.3253
            var sales8 = DomainFactory.CreateSales(8, new DateTime(1991, 2, 28));

            sales8.AddSubordinates(manager5);
            sales8.AddSubordinates(sales6);
            sales8.AddSubordinates(sales7);

            _workers.Add(sales8);
        }
    }
}
