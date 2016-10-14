using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.Business.Billing
{
  #region PaymentDetail
  [Serializable]
  public class PaymentDetail : BusinessBase<PaymentDetail>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
      set { SetProperty(_Id, value); }
    }

    #endregion

    #region Payment

    public static readonly PropertyInfo<int> _Payment = RegisterProperty<int>(c => c.Payment, "PaymentDetail");
    public int Payment
    {
      get { return GetProperty(_Payment); }
      set { SetProperty(_Payment, value); }
    }

    #endregion

    #region Bill

    public static readonly PropertyInfo<BillPaymentInfo> _Bill = RegisterProperty<BillPaymentInfo>(c => c.Bill, "Bill");
    public BillPaymentInfo Bill
    {
      get { return GetProperty(_Bill); }
      set { SetProperty(_Bill, value); }
    }

    #endregion

    #region PartialPayment

    public static readonly PropertyInfo<decimal> _PartialPayment = RegisterProperty<decimal>(c => c.PartialPayment);
    public decimal PartialPayment
    {
      get { return GetProperty(_PartialPayment); }
      set { SetProperty(_PartialPayment, value); }
    }

    #endregion

    #region AmountDue

    public static readonly PropertyInfo<decimal> _AmountDue = RegisterProperty<decimal>(c => c.AmountDue);
    public decimal AmountDue
    {
      get { return GetProperty(_AmountDue); }
      set { SetProperty(_AmountDue, value); }
    }

    #endregion

    #region AmountPaid

    public static readonly PropertyInfo<decimal?> _AmountPaid = RegisterProperty<decimal?>(c => c.AmountPaid,"Amount Paid");
    public decimal? AmountPaid
    {
      get { return GetProperty(_AmountPaid); }
      set { SetProperty(_AmountPaid, value); }
    }

    #endregion

    #endregion

    #region Derived Properties

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new AmountPaidRule { PrimaryProperty = _AmountPaid });
    }

    #region AmountPaidRule
    private class AmountPaidRule : Csla.Rules.BusinessRule
    {
      protected override void Execute(Csla.Rules.RuleContext context)
      {
        var target = (PaymentDetail)context.Target;
        if (target.AmountPaid == null)
        {
          context.AddErrorResult("Amount paid is required");
        }
        else
        {
          if (target.AmountPaid > target.AmountDue)
          {
            context.AddErrorResult("Amount paid should not exceed amount due.");
          }
        }
      }
    }
    #endregion

    #endregion

    #region Factory Methods

    public static PaymentDetail New()
    {
      return Csla.DataPortal.CreateChild<PaymentDetail>();
    }

    public static PaymentDetail Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<PaymentDetail>(dr);
    }

    #endregion

    #region Data Access

    #region Fetch

    protected override void Child_Create()
    {
      base.Child_Create();
      BusinessRules.CheckRules();
    }

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("paymentdetail"));
      LoadProperty(_Payment, dr.GetInt32("Payment"));
      LoadProperty(_Bill,BillPaymentInfo.Get(dr,_Bill.Name));
      LoadProperty(_PartialPayment,dr.GetDecimal("partialpayment"));
      LoadProperty(_AmountDue,dr.GetDecimal("amountdue"));
      LoadProperty(_AmountPaid,dr.GetDecimal("amountpaid"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO paymentdetail(payment,bill,partialpayment,amountdue,amountpaid)
                              VALUES (@payment,@bill,@partialpayment,@amountdue,@amountpaid)
                            SELECT SCOPE_IDENTITY()";

          cmd.Parameters.AddWithValue("@payment", parentId.Value);
          cmd.Parameters.AddWithValue("@bill", Bill.Id);
          cmd.Parameters.AddWithValue("@partialpayment", PartialPayment);
          cmd.Parameters.AddWithValue("@amountdue", AmountDue);
          cmd.Parameters.AddWithValue("@amountpaid", AmountPaid);

          try
          {
            int identity = Convert.ToInt32(cmd.ExecuteScalar());
            LoadProperty(_Id, identity);
          }
          catch (Exception)
          {
            throw;
          }
        }
      }

    }

    #endregion

    #region Child Update

    protected void Child_Update(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE paymentetail SET 
                                bill = @bill,
                                partialpayment = @partialpayment,
                                amountdue = @amountdue,
                                amountpaid = @amountpaid
                              WHERE paymentetail = @id";
          cmd.Parameters.AddWithValue("@bill", Bill.Id);
          cmd.Parameters.AddWithValue("@partialpayment", PartialPayment);
          cmd.Parameters.AddWithValue("@amountdue", AmountDue);
          cmd.Parameters.AddWithValue("@amountpaid", AmountPaid);
          cmd.Parameters.AddWithValue("@id", this.Id);
          try
          {
            cmd.ExecuteNonQuery();
          }
          catch (Exception)
          {
            throw;
          }
        }
      }

    }

    #endregion

    #region Child Delete

    protected void Child_DeleteSelf()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"DELETE FROM paymentdetail WHERE paymentdetail = @id";
          cmd.Parameters.AddWithValue("@id", this.Id);

          try
          {
            cmd.ExecuteNonQuery();
          }
          catch (Exception)
          {
            throw;
          }
        }
      }

    }

    #endregion

    #endregion

    #region Methods

    #endregion
  }
  #endregion

  #region PaymentDetails
  [Serializable]
  public class PaymentDetails : BusinessListBase<PaymentDetails, PaymentDetail>
  {
    #region Factory Methods

    public static PaymentDetails New()
    {
      return new PaymentDetails();
    }

    public static PaymentDetails Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<PaymentDetails>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = string.Format(@"
                              SELECT pd.paymentdetail,pd.payment,pd.bill,pd.partialpayment,pd.amountdue,pd.amountpaid,
                                  b.bill as {0}bill,b.billno AS {0}billno,b.billdate as {0}billdate
                              FROM paymentdetail pd
                              INNER JOIN bill b ON pd.bill = b.bill
                              WHERE pd.payment = @id", "Bill");
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(PaymentDetail.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
