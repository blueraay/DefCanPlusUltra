using DefCan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CUPS.Services
{


	public class CheckingAccountServices
	{
		private IApplicationDbContext db;

		public void UpdateBalance(int id)
		{
			var checkingAccount = db.CheckingAccounts.Where(c => c.Id == checkingAccountId).First();
			checkingAccount.Balance = db.Transactions.Where(c => c.CheckingAccountId == checkingAccountId).Sum(c => c.Amount);
			db.SaveChanges();
		}
	}



}