using System;
using System.Collections.Generic;
using AltSrcBank.Models;
using ASBCLI.Financial;
using ASBLib.Exceptions;

namespace ASBCLI.Views
{
	public class AccountView : MenuView
    {
		const float EPSILON = 0.0F;
		/**
		 * Create a general transaction.
		 * @param name="context" Context of the application
		 * @param name="controller" Application's controller instance.
		 * @param name="type" Type of the transaction (Deposit, Withdrawal)
		 * @returns 1 on success, 0 otherwise.
		 **/
		public int HandleTransaction(ViewContext context,
		                             Controller controller,
		                             Type type)
		{
            User user = context.GetUser();
            float amount = float.NaN;
			Transaction transaction = null;
			string tName = type.Name;
            while (float.IsNaN(amount))
            {
                try
                {
					amount = Util.InputFloat(tName + " Amount (e.g. 4.25): ");
                    if (Math.Abs(amount) < EPSILON)
					{
						Console.WriteLine("Nothing added.");
						return 0;
					}
                    transaction = (Transaction)Activator.CreateInstance(type, amount);
                }
                catch (ValidationException e)
                {
                    Console.WriteLine(e);
                    amount = float.NaN;
				}
				catch (FormatException e2)
				{
                    Console.WriteLine(e2);
                    amount = float.NaN;
				}
			}
            controller.AccountAddTransaction(user, transaction);
            return 1;
		}

        /**
         * Show a menu allowing the current user to deposit a certain amount
         * to user's account.
         * @param name="context" Application context
         * @returns 1 on success, 0 otherwise.
         **/
		public int DepositAmount(object context = null)
		{
            var ctx = ((ViewContext)context);
            var controller = ((ViewContext)context).Controller;
			return HandleTransaction(ctx, controller, typeof(Deposit));
		}

        /**
         * Show a menu allowing the current user to withdraw a certain amount
         * from user's account.
         * @param name="context" Application context
         * @returns 1 on success, 0 otherwise.
         **/
		public int WithdrawAmount(object context = null)
		{
            var ctx = ((ViewContext)context);
            var controller = ((ViewContext)context).Controller;
			return HandleTransaction(ctx, controller, typeof(Withdrawal));
		}

        /**
         * Display the account balance to the user.
         * @param name="context" Application context
         * @returns 1 on success, 0 otherwise.
         **/
        public int ViewBalance(object context = null)
		{
            var ctx = ((ViewContext)context);
            var controller = ((ViewContext)context).Controller;
            User user = ctx.GetUser();
			Console.WriteLine("Your balance is {0}", user.GetBalance());
			return 1;
		}


        /**
         * Display the transaction history of the currently logged in user.
         * @param name="context" Application context
         * @returns 1 on success, 0 otherwise.
         **/
        public int TransactionHistory(object context = null)
		{
            var ctx = ((ViewContext)context);
            var controller = ((ViewContext)context).Controller;
			LinkedList<Transaction> transactions = controller.AccountGetTransactions(ctx.GetUser());
			foreach (var transaction in transactions)
			{
				Console.WriteLine("{0,-20}{1,30}{2,30}",
								  transaction.GetType().Name,
				                  transaction.Timestamp.ToLocalTime(),
								  transaction.Amount);
			}
            return 1;
		}



        /**
         * Construct an Account menu.
         * @param name="context" Application context
         * @param name="parent" Parent menu.
         **/
		public AccountView(ViewContext context, Menu parent) : base(context, "Account Menu", parent)
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
