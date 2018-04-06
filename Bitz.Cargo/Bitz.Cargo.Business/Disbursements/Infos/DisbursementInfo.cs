using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Core.Constants;
using Bitz.Core.Data;
using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.Business.Disbursements.Infos
{
  #region DisbursementInfo

  [Serializable]
  [TableInfo(TableName = "disbursement", KeyColumn = "disbursement.disbursement")]
  public class DisbursementInfo : ReadOnlyBase<DisbursementInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region Type

    public static readonly PropertyInfo<VoucherTypeInfo> _Type = RegisterProperty<VoucherTypeInfo>(c => c.Type, "Type");
    public VoucherTypeInfo Type
    {
      get { return GetProperty(_Type); }
    }

    #endregion

    #region DocumentNo

    public static readonly PropertyInfo<string> _DocumentNo = RegisterProperty<string>(c => c.DocumentNo);
    public string DocumentNo
    {
      get { return GetProperty(_DocumentNo); }
    }

    #endregion

    #region DocumentDate

    public static readonly PropertyInfo<SmartDate> _DocumentDate = RegisterProperty<SmartDate>(c => c.DocumentDate);
    public SmartDate DocumentDate
    {
      get { return GetProperty(_DocumentDate); }
    }

    #endregion

    #region ControlNumber

    public static readonly PropertyInfo<int> _ControlNumber = RegisterProperty<int>(c => c.ControlNumber);
    public int ControlNumber
    {
      get { return GetProperty(_ControlNumber); }
    }

    #endregion

    #region Recipient

    public static readonly PropertyInfo<string> _Recipient = RegisterProperty<string>(c => c.Recipient);
    public string Recipient
    {
      get { return GetProperty(_Recipient); }
    }

    #endregion

    #region Status

    public static readonly PropertyInfo<int> _Status = RegisterProperty<int>(c => c.Status);
    public CoreConstants.IdValue Status
    {
      get
      {
        return CargoConstants.StandardStatus.Items.SingleOrDefault(x => x.Id == GetProperty(_Status));
      }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static DisbursementInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<DisbursementInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<DisbursementInfo>> completed)
    {
      DataPortal<DisbursementInfo> dp = new DataPortal<DisbursementInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("disbursement"));
      LoadProperty(_Type, VoucherTypeInfo.Get(dr, _Type.Name));
      LoadProperty(_DocumentNo, dr.GetString("documentno"));
      LoadProperty(_DocumentDate, dr.GetSmartDate("documentdate"));
      LoadProperty(_Recipient, dr.GetString("recipient"));
      LoadProperty(_ControlNumber, dr.GetInt32("controlnumber"));
      LoadProperty(_Status, dr.GetInt32("status"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM disbursement WHERE disbursement = @id";
          cm.Parameters.AddWithValue("@id", id.Value);
          cm.ExecuteNonQuery();
        }
      }
    }

    #endregion

    #endregion

    #endregion
  }

  #endregion

  #region DisbursementInfos

  [Serializable]
  public class DisbursementInfos : ReadOnlyListBase<DisbursementInfos, DisbursementInfo>
  {
    #region Criteria

    [Serializable]
    public class Criteria : PageCriteriaBase<Criteria>
    {
      #region SearchText
      private static PropertyInfo<string> _SearchText = RegisterProperty<string>(c => c.SearchText);

      public string SearchText
      {
        get { return ReadProperty(_SearchText); }
        set { LoadProperty(_SearchText, value); }
      }
      #endregion //SearchText

      #region Status

      public static readonly PropertyInfo<int?> _Status = RegisterProperty<int?>(c => c.Status);
      public int? Status
      {
        get { return ReadProperty(_Status); }
        set { LoadProperty(_Status, value); }
      }

      #endregion

      #region Type

      public static readonly PropertyInfo<int?> _Type = RegisterProperty<int?>(c => c.Type);
      public int? Type
      {
        get { return ReadProperty(_Type); }
        set { LoadProperty(_Type, value); }
      }

      #endregion

      #region StartDate

      public static readonly PropertyInfo<SmartDate> _StartDate = RegisterProperty<SmartDate>(c => c.StartDate);
      public SmartDate StartDate
      {
        get { return ReadProperty(_StartDate); }
        set { LoadProperty(_StartDate, value); }
      }

      #endregion

      #region EndDate

      public static readonly PropertyInfo<SmartDate> _EndDate = RegisterProperty<SmartDate>(c => c.EndDate);
      public SmartDate EndDate
      {
        get { return ReadProperty(_EndDate); }
        set { LoadProperty(_EndDate, value); }
      }

      #endregion

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<DisbursementInfos>> completed)
    {
      DataPortal<DisbursementInfos> dp = new DataPortal<DisbursementInfos>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(criteria);
    }

    #endregion

    #region Data Access

    protected void DataPortal_Fetch(Criteria criteria)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = string.Format(@"
                                  SELECT d.disbursement,d.type,d.documentno,d.documentdate,d.status,
                                    vt.vouchertype AS {0}vouchertype,vt.type AS {0}type,vt.name AS {0}name
                                  FROM disbursement d
                                  LEFT JOIN vouchertype vt ON d.type = vt.vouchertype
                                  WHERE 1 = 1 ", "Type");
          if (!string.IsNullOrEmpty(criteria.SearchText))
          {
            cmd.CommandText += @" AND (d.preparedby LIKE @SearchText 
                                  OR d.approvedby LIKE @SearchText 
                                  OR d.receivedby LIKE @SearchText 
                                  OR d.documentno LIKE @SearchText)";

            cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");
          }

          if (criteria.Type != null)
            cmd.Parameters.AddWithValue("@type", criteria.Type);
          else
            cmd.Parameters.AddWithValue("@type", DBNull.Value);

          if (criteria.StartDate != null && criteria.EndDate != null)
          {
            cmd.CommandText += @" AND (d.documentdate >= @fromDate AND d.documentdate < @toDate)";
            cmd.Parameters.AddWithValue("@fromDate", criteria.StartDate.Date);
            cmd.Parameters.AddWithValue("@toDate", criteria.EndDate.Date.AddDays(1).Date);
          }

          //Apply paging
          if (criteria.PageSize > 0)
          {
            var sortby = "d.documentdate DESC";
            SQLHelper.AddSQLPaging(criteria, sortby, cmd);
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(DisbursementInfo.Get(dr));
            }
            IsReadOnly = true;
          }
        }
      }
    }

    #endregion

  }

  #endregion
}
