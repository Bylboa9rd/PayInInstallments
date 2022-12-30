using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UpdatedPayInInstallmentsApp.Enums;

namespace UpdatedPayInInstallmentsApp
{
    public class Debtor
    {
        public Debtor() { }
        public Debtor(int id, string email, string password, decimal? debitCardNumber, short? pin, decimal? originalDebt, Plan? plan, decimal? remainingDebt, decimal? amountPerDeposit) 
        {
            Id = id;
            Email = email;
            Password = password;
            DebitCardNumber = debitCardNumber;
            Pin = pin;
            OriginalDebt = originalDebt;
            Plan = (Plan)plan;
            RemainingDebt = remainingDebt;
            AmountPerDeposit = amountPerDeposit;
            
        }  

        public int Id { get; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal? DebitCardNumber { get; set; }
        public short? Pin { get; set; }
        public decimal? OriginalDebt { get; }
        public decimal? RemainingDebt { get; private set; }
        public Plan Plan { get; }
        public decimal? AmountPerDeposit { get; }
        public DateTime NextExpectedPayment { get; private set; }
        public override bool Equals(object? obj) => Email == (obj as Debtor).Email;


        public void SetRemainingDebt()
        {
            RemainingDebt -= AmountPerDeposit;
        }

        public void ClearDebt()
        {
            RemainingDebt -= RemainingDebt;
        }

        public void SetNextExpectedPayment()
        {
            DateTime today = DateTime.Now;
            switch(Plan)
            {
                case Plan.Daily:
                    NextExpectedPayment = today.AddDays(1);
                    break;
                case Plan.Weekly:
                    NextExpectedPayment = today.AddDays(7);
                    break;
                case Plan.Bi_Weekly:
                    NextExpectedPayment = today.AddDays(14);
                    break;
                case Plan.Monthly:
                    NextExpectedPayment = today.AddMonths(1);
                    break;
                case Plan.Six_Monthly:
                    NextExpectedPayment = today.AddMonths(6);
                    break;
                case Plan.Yearly:
                    NextExpectedPayment = today.AddYears(1);
                    break;
                default:
                    break;
            }
        }

        public Plan? GetPlan(int plan)
        {
            switch (plan)
            {
                case 1:
                    return Plan.Daily;
                case 2:
                    return Plan.Weekly;
                case 3:
                    return Plan.Bi_Weekly;
                case 4:
                    return Plan.Monthly;
                case 5:
                    return Plan.Six_Monthly;
                case 6:
                    return Plan.Yearly;
                default:                 
                    return null;
            }
        }
    }
}
