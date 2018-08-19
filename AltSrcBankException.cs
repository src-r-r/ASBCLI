using System;
namespace ASBCLI
{
	public class AltSrcBankException : Exception
    {
		private String mReason;
        public AltSrcBankException(String reason)
        {
			mReason = reason;
        }
    }
}
