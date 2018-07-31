﻿using PayrollSystem.Domains.Abstract;
using PayrollSystem.Domains.Enum;

namespace PayrollSystem.Domains
{
    public class Sales : ChiefWorker
    {
        public override WorkerType WorkerType => WorkerType.Sales;
    }
}
