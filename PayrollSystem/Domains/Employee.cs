using PayrollSystem.Domains.Abstract;
using PayrollSystem.Domains.Enum;

namespace PayrollSystem.Domains
{
    public class Employee : Worker
    {
        public override WorkerType WorkerType => WorkerType.Employee;
    }
}
