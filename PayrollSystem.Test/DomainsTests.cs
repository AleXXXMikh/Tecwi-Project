using NUnit.Framework;
using System;

namespace PayrollSystem.Test
{
    public class DomainsTests
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void AddSubordinates_CheckThatAfterAddingSubordinatesChiefWillBeSet_AddSubordinates()
        {
            var sales = DomainFactory.CreateSales(1, new DateTime(1991, 2, 28));

            var manager = DomainFactory.CreateManager(2, new DateTime(1991, 2, 28));
            sales.AddSubordinates(manager);

            Assert.AreEqual(1, manager.Chief.Id);
        }

        [Test]
        public void SetChief_CheckThatAfterSettingChiefSubordinatesWillBeAdded_AddSubordinates()
        {
            var sales = DomainFactory.CreateSales(1, new DateTime(1991, 2, 28));

            var manager = DomainFactory.CreateManager(2, new DateTime(1991, 2, 28));
            sales.SetChief(manager);

            Assert.AreEqual(1, manager.Subordinates.Count);
        }

        [Test]
        public void SetChief_CheckCycleDependencyForSales_CannotBeCycleDependency()
        {
            var sales1 = DomainFactory.CreateSales(1, new DateTime(1991, 2, 28));

            var manager = DomainFactory.CreateManager(2, new DateTime(1991, 2, 28));
            sales1.AddSubordinates(manager);

            var sales3 = DomainFactory.CreateSales(4, new DateTime(1991, 2, 28));
            manager.AddSubordinates(sales3);

           sales1.SetChief(sales3);

            Assert.IsNull(sales1.Chief);
            Assert.AreEqual(0, sales3.Subordinates.Count);
        }
    }
}
