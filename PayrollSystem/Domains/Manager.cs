using PayrollSystem.Domains.Abstract;
using PayrollSystem.Domains.Enum;

namespace PayrollSystem.Domains
{
    public class Manager : ChiefWorker
    {
        public override WorkerType WorkerType => WorkerType.Manager;
    }
}
