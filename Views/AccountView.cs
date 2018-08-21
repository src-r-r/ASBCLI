using System;
using System.Collections.Generic;
using AltSrcBank.Models;
using ASBCLI.Financial;
using ASBLib.Exceptions;

namespace ASBCLI.Views
{
	public class AccountView : MenuView
    {
		public int DepositAmount(object context = null)
		{
			var ctx = ((ViewContext)context);
            var controller = ((ViewContext)context).Controller;
			User user = ctx.GetUser();
			float amount = float.NaN;
			Deposit transaction = null;
			while (float.IsNaN(amount))
			{
				try
				{               
					amount = Util.InputFloat("Deposit Amount (e.g. 4.25):");
                    transaction = new Deposit(amount);
				}
				catch (ValidationException e)
				{
					Console.WriteLine(e);
					amount = float.NaN;               
				}
			}
			controller.AccountAddTransaction(user, transaction);
			return 1;
		}

        public int WithdrawAmount(object context = null)
		{
            var ctx = ((ViewContext)context);
            var controller = ((ViewContext)context).Controller;
            User user = ctx.GetUser();
			float amount = float.NaN;
			Withdrawal transaction = null;
            while (float.IsNaN(amount))
            {
                try
                {
					amount = Util.InputFloat("Withdraw Amount (e.g. 4.25):");
                    transaction = new Withdrawal(amount);
                }
                catch (ValidationException e)
                {
                    Console.WriteLine(e);
                    amount = float.NaN;
                }
            }
            controller.AccountAddTransaction(user, transaction);
            return 1;
		}

        public int ViewBalance(object context = null)
		{
            var ctx = ((ViewContext)context);
            var controller = ((ViewContext)context).Controller;
            User user = ctx.GetUser();
			Console.WriteLine("Your balance is {0}", user.GetBalance());
			return 1;
		}

        public int TransactionHistory(object context = null)
		{
            var ctx = ((ViewContext)context);
            var controller = ((ViewContext)context).Controller;
			LinkedList<Transaction> transactions = controller.AccountGetTransactions(ctx.GetUser());
			foreach (var transaction in transactions)
			{
				Console.WriteLine("{0:10}{1:30}{2:30}",
								  transaction.GetType().Name,
								  transaction.Timestamp,
								  transaction.Amount);
			}
            return 1;
		}

		public AccountView(ViewContext context, Menu parent) : base(context, "Account View", parent)
		{
            // Console.WriteLine("Context == null ? {0}", context == null);
            MenuEntry[] items = new MenuEntry[]{
                new MenuItem("Deposit", DepositAmount, this),
                new MenuItem("Withdraw", WithdrawAmount, this),
                new MenuItem("View Balance", ViewBalance, this),
				new MenuItem("Transaction History", TransactionHistory, this),
			};
            foreach (var i in items) { Add(i); }
        }
    }
}
