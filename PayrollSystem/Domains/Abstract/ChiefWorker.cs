using System.Collections.Generic;

namespace PayrollSystem.Domains.Abstract
{
    public abstract class ChiefWorker : Worker
    {
        protected ChiefWorker()
        {
            Subordinates = new List<Worker>();
        }

        public List<Worker> Subordinates { get; }

        public void AddSubordinates(Worker worker)
        {
            // Checking if worker is already subordinated and permission for adding this worker
            if (!Subordinates.Contains(worker) && CanWorkerBeSubordinated(this, worker))
            {
                Subordinates.Add(worker);
                // Set current chief for worker if needed
                if (worker.Chief == null || worker.Chief.Id != this.Id)
                {
                    worker.SetChief(this);
                }
            }
        }
    }
}
