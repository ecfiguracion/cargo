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

namespace Bitz.Cargo.Business.Billing.Infos
{
  #region BillingItemInfo

  [Serializable]
  [TableInfo(TableName = "billingitem", KeyColumn = "billingitem.billingitem")]
  public class BillingItemInfo : ReadOnlyBase<BillingItemInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region BillingItemType

    public static readonly PropertyInfo<int> _BillingItemType = RegisterProperty<int>(c => c.BillingItemType);
    public int BillingItemType
    {
      get { return GetProperty(_BillingItemType); }
    }

    #endregion

    #region BillingItemTypeName

    public static readonly PropertyInfo<string> _BillingItemTypeName = RegisterProperty<string>(c => c.BillingItemTypeName);
    public string BillingItemTypeName
    {
      get { return GetProperty(_BillingItemTypeName); }
    }

    #endregion

    #region ReferenceNo

    public static readonly PropertyInfo<string> _ReferenceNo = RegisterProperty<string>(c => c.ReferenceNo);
    public string ReferenceNo
    {
      get { return GetProperty(_ReferenceNo); }
    }

    #endregion

    #region BillingDate

    public static readonly PropertyInfo<SmartDate> _BillingDate = RegisterProperty<SmartDate>(c => c.BillingDate);
    public SmartDate BillingDate
    {
      get { return GetProperty(_BillingDate); }
    }

    #endregion

    #region IsWithTax

    public static readonly PropertyInfo<bool> _IsWithTax = RegisterProperty<bool>(c => c.IsWithTax);
    public bool IsWithTax
    {
      get { return GetProperty(_IsWithTax); }
    }

    #endregion

    #region BillLadingNo

    public static readonly PropertyInfo<string> _BillLadingNo = RegisterProperty<string>(c => c.BillLadingNo);
    public string BillLadingNo
    {
      get { return GetProperty(_BillLadingNo); }
    }

    #endregion

    #region ConsigneeName

    public static readonly PropertyInfo<string> _ConsigneeName = RegisterProperty<string>(c => c.ConsigneeName);
    public string ConsigneeName
    {
      get { return GetProperty(_ConsigneeName); }
    }

    #endregion

    #region ConsigneeAddress

    public static readonly PropertyInfo<string> _ConsigneeAddress = RegisterProperty<string>(c => c.ConsigneeAddress);
    public string ConsigneeAddress
    {
      get { return GetProperty(_ConsigneeAddress); }
    }

    #endregion

    #region VesselName

    public static readonly PropertyInfo<string> _VesselName = RegisterProperty<string>(c => c.VesselName);
    public string VesselName
    {
      get { return GetProperty(_VesselName); }
    }

    #endregion

    #region VoyageNo

    public static readonly PropertyInfo<string> _VoyageNo = RegisterProperty<string>(c => c.VoyageNo);
    public string VoyageNo
    {
      get { return GetProperty(_VoyageNo); }
    }

    #endregion

    #region ItemName

    public static readonly PropertyInfo<string> _ItemName = RegisterProperty<string>(c => c.ItemName);
    public string ItemName
    {
      get { return GetProperty(_ItemName); }
    }

    #endregion

    #region ItemCount

    public static readonly PropertyInfo<decimal> _ItemCount = RegisterProperty<decimal>(c => c.ItemCount);
    public decimal ItemCount
    {
      get { return GetProperty(_ItemCount); }
    }

    #endregion

    #region ItemUnitName

    public static readonly PropertyInfo<string> _ItemUnitName = RegisterProperty<string>(c => c.ItemUnitName);
    public string ItemUnitName
    {
      get { return GetProperty(_ItemUnitName); }
    }

    #endregion

    #region Remarks

    public static readonly PropertyInfo<string> _Remarks = RegisterProperty<string>(c => c.Remarks);
    public string Remarks
    {
      get { return GetProperty(_Remarks); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static BillingItemInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BillingItemInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<BillingItemInfo>> completed)
    {
      DataPortal<BillingItemInfo> dp = new DataPortal<BillingItemInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("billingitem"));
      LoadProperty(_BillingItemType, dr.GetInt16("type"));
      LoadProperty(_BillingItemTypeName, dr.GetString("typename"));
      LoadProperty(_ReferenceNo, dr.GetString("referenceno"));
      LoadProperty(_BillingDate, dr.GetSmartDate("billingdate"));
      LoadProperty(_BillLadingNo, dr.GetString("billladingno"));
      LoadProperty(_ConsigneeName, dr.GetString("consigneename"));
      LoadProperty(_ConsigneeAddress, dr.GetString("custpreferredaddress"));
      LoadProperty(_VesselName, dr.GetString("vesselname"));
      LoadProperty(_VoyageNo, dr.GetString("voyageno"));
      LoadProperty(_ItemName, dr.GetString(("itemname")));
      LoadProperty(_ItemCount, dr.GetDecimal("itemcount"));
      LoadProperty(_ItemUnitName, dr.GetString(("itemunitname")));
      LoadProperty(_Remarks, dr.GetString(("remarks")));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM billingitem WHERE billingitem = @id";
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

  #region BillingItemInfos

  [Serializable]
  public class BillingItemInfos : ReadOnlyListBase<BillingItemInfos, BillingItemInfo>
  {
    #region Criteria

    [Serializable]
    public class Criteria : CriteriaBase<Criteria>
    {
      #region SearchText
      private static PropertyInfo<string> _SearchText = RegisterProperty<string>(c => c.SearchText);

      public string SearchText
      {
        get { return ReadProperty(_SearchText); }
        set { LoadProperty(_SearchText, value); }
      }
      #endregion //SearchText

      #region BillingItemType
      private static PropertyInfo<int?> _BillingItemType = RegisterProperty<int?>(c => c.BillingItemType);

      public int? BillingItemType
      {
        get { return ReadProperty(_BillingItemType); }
        set { LoadProperty(_BillingItemType, value); }
      }
      #endregion //ItemType

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<BillingItemInfos>> completed)
    {
      DataPortal<BillingItemInfos> dp = new DataPortal<BillingItemInfos>();
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
          cmd.CommandText = @"SELECT i.billingitem,i.referenceno,i.billingdate,i.iswithtax,i.billladingno,consignee.name as consigneename,
                                i.custpreferredaddress,vessel.name as vesselname,i.voyageno,item.itemname,i.itemcount,uom.code as itemunitname,i.type,
                                CASE WHEN i.type = 1 THEN 'Foreign'
                                  WHEN i.type = 2 THEN 'Domestic'
                                  WHEN i.type = 3 THEN 'RORO' END as typename, i.remarks
                              FROM dbo.billingitem i
	                            INNER JOIN contact consignee
		                            ON consignee.contact = i.customer
	                            INNER JOIN contact vessel
		                            ON vessel.contact = i.vessel
	                            INNER JOIN uom
		                            ON uom.uom = i.preferreduom
	                            INNER JOIN item
		                            ON item.item = i.item
                              WHERE (i.referenceno LIKE @SearchText 
                                  OR i.billladingno LIKE @SearchText 
                                  OR consignee.name LIKE @SearchText 
                                  OR i.custpreferredaddress LIKE @SearchText 
                                  OR vessel.name LIKE @SearchText 
                                  OR i.voyageno LIKE @SearchText 
                                  OR item.itemname LIKE @SearchText)
                                AND (i.type = @billingitemtype OR @billingitemtype IS NULL)";

          cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          if (criteria.BillingItemType != null)
            cmd.Parameters.AddWithValue("@billingitemtype", criteria.BillingItemType);
          else
            cmd.Parameters.AddWithValue("@billingitemtype", DBNull.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(BillingItemInfo.Get(dr));
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
