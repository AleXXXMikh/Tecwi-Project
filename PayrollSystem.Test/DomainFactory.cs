using System;
using System.Collections.Generic;
using PayrollSystem.Domains;
using PayrollSystem.Domains.Abstract;

namespace PayrollSystem.Test
{
    public class DomainFactory
    {
        public static Employee CreateEmployee(int id, DateTime dateTime, string fullName = "Test name", ChiefWorker chief = null)
        {
            var employee = new Employee
            {
                Id = id,
                EmploymentDate = dateTime,
                FullName = fullName + id
            };

            if (chief != null)
            {
                employee.SetChief(chief);
            }

            return employee;
        }

        public static Manager CreateManager(int id, DateTime dateTime, string fullName = "Test name", List<Worker> workers = null, ChiefWorker chief = null)
        {
            var manager = new Manager
            {
                Id = id,
                EmploymentDate = dateTime,
                FullName = fullName + id
            };

            if (chief != null)
            {
                manager.SetChief(chief);
            }

            if (workers != null)
            {
                foreach (var worker in workers)
                {
                    manager.AddSubordinates(worker);
                }
            }

            return manager;
        }

        public static Sales CreateSales(int id, DateTime dateTime, string fullName = "Test name", List<Worker> workers = null, ChiefWorker chief = null)
        {
            var sales = new Sales
            {
                Id = id,
                EmploymentDate = dateTime,
                FullName = fullName + id
            };

            if (chief != null)
            {
                sales.SetChief(chief);
            }

            if (workers != null)
            {
                foreach (var worker in workers)
                {
                    sales.AddSubordinates(worker);
                }
            }

            return sales;
        }
    }
}
