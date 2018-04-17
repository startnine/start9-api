using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System;

namespace Start9.Api.Contracts
{
    [AddInContract]
    public interface ICalc1Contract : IContract
    {
        Double Add(Double a, Double b);
        Double Subtract(Double a, Double b);
        Double Multiply(Double a, Double b);
        Double Divide(Double a, Double b);  
    }
}
