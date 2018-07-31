using System;
using PayrollSystem.Domains.Enum;

namespace PayrollSystem.Domains.Abstract
{
    public abstract class Worker
    {
        public int Id { get; set; }

        public ChiefWorker Chief { get; private set; }

        public string FullName { get; set; }

        public DateTime EmploymentDate { get; set; }

        public decimal BasePaymentRate => 200;

        public abstract WorkerType WorkerType { get; }

        public void SetChief(ChiefWorker chief)
        {
            // Checking if chief is already set and permission for adding this chief
            if ((Chief == null || Chief.Id != chief?.Id) && CanWorkerBeSubordinated(chief, this))
            {
                Chief = chief;
                // Adding current worker to subordinates if needed
                if (chief != null && !chief.Subordinates.Contains(this))
                {
                    chief.AddSubordinates(this);
                }
            }
        }

        /// <summary>
        /// Determines whether worker can be subordinated by the specified chief.
        /// </summary>
        /// <param name="chief">The chief.</param>
        /// <param name="worker">The worker.</param>
        /// <returns>
        ///   <c>true</c> Determines whether worker can be subordinated by the specified chief. <c>false</c>.
        /// </returns>
        protected bool CanWorkerBeSubordinated(ChiefWorker chief, Worker worker)
        {
            if (!(worker is ChiefWorker chiefWorker))
            {
                return true;
            }

            foreach (var subordinate in chiefWorker.Subordinates)
            {
                if (chief.Id == subordinate.Id)
                {
                    return false;
                }

                return CanWorkerBeSubordinated(chief, subordinate);
            }

            return true;
        }
    }
}
