﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FTPTransfer
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	public partial class enterpriseDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertAuditDetail(AuditDetail instance);
    partial void UpdateAuditDetail(AuditDetail instance);
    partial void DeleteAuditDetail(AuditDetail instance);
    partial void InsertClientDetail(ClientDetail instance);
    partial void UpdateClientDetail(ClientDetail instance);
    partial void DeleteClientDetail(ClientDetail instance);
    partial void InsertAUDITRK(AUDITRK instance);
    partial void UpdateAUDITRK(AUDITRK instance);
    partial void DeleteAUDITRK(AUDITRK instance);
    partial void InsertVendor(Vendor instance);
    partial void UpdateVendor(Vendor instance);
    partial void DeleteVendor(Vendor instance);
    partial void InsertService(Service instance);
    partial void UpdateService(Service instance);
    partial void DeleteService(Service instance);
    partial void InsertLoan_Service(Loan_Service instance);
    partial void UpdateLoan_Service(Loan_Service instance);
    partial void DeleteLoan_Service(Loan_Service instance);
    #endregion
		
		public enterpriseDBDataContext() : 
				base(global::FTPTransfer.Properties.Settings.Default.enterpriseDBConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public enterpriseDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public enterpriseDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public enterpriseDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public enterpriseDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<AuditDetail> AuditDetails
		{
			get
			{
				return this.GetTable<AuditDetail>();
			}
		}
		
		public System.Data.Linq.Table<ClientDetail> ClientDetails
		{
			get
			{
				return this.GetTable<ClientDetail>();
			}
		}
		
		public System.Data.Linq.Table<AUDITRK> AUDITRKs
		{
			get
			{
				return this.GetTable<AUDITRK>();
			}
		}
		
		public System.Data.Linq.Table<LoanDetail> LoanDetails
		{
			get
			{
				return this.GetTable<LoanDetail>();
			}
		}
		
		public System.Data.Linq.Table<Vendor> Vendors
		{
			get
			{
				return this.GetTable<Vendor>();
			}
		}
		
		public System.Data.Linq.Table<Service> Services
		{
			get
			{
				return this.GetTable<Service>();
			}
		}
		
		public System.Data.Linq.Table<Loan_Service> Loan_Services
		{
			get
			{
				return this.GetTable<Loan_Service>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AuditDetail")]
	public partial class AuditDetail : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ClientID;
		
		private string _AuditID;
		
		private string _AuditDesc;
		
		private System.Nullable<System.DateTime> _ListReceivedDate;
		
		private int _StatusID;
		
		private System.Nullable<System.DateTime> _FileReceivedDate;
		
		private bool _IsArchived;
		
		private EntityRef<ClientDetail> _ClientDetail;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnClientIDChanging(int value);
    partial void OnClientIDChanged();
    partial void OnAuditIDChanging(string value);
    partial void OnAuditIDChanged();
    partial void OnAuditDescChanging(string value);
    partial void OnAuditDescChanged();
    partial void OnListReceivedDateChanging(System.Nullable<System.DateTime> value);
    partial void OnListReceivedDateChanged();
    partial void OnStatusIDChanging(int value);
    partial void OnStatusIDChanged();
    partial void OnFileReceivedDateChanging(System.Nullable<System.DateTime> value);
    partial void OnFileReceivedDateChanged();
    partial void OnIsArchivedChanging(bool value);
    partial void OnIsArchivedChanged();
    #endregion
		
		public AuditDetail()
		{
			this._ClientDetail = default(EntityRef<ClientDetail>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ClientID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int ClientID
		{
			get
			{
				return this._ClientID;
			}
			set
			{
				if ((this._ClientID != value))
				{
					if (this._ClientDetail.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnClientIDChanging(value);
					this.SendPropertyChanging();
					this._ClientID = value;
					this.SendPropertyChanged("ClientID");
					this.OnClientIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AuditID", DbType="VarChar(5) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string AuditID
		{
			get
			{
				return this._AuditID;
			}
			set
			{
				if ((this._AuditID != value))
				{
					this.OnAuditIDChanging(value);
					this.SendPropertyChanging();
					this._AuditID = value;
					this.SendPropertyChanged("AuditID");
					this.OnAuditIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AuditDesc", DbType="VarChar(50)")]
		public string AuditDesc
		{
			get
			{
				return this._AuditDesc;
			}
			set
			{
				if ((this._AuditDesc != value))
				{
					this.OnAuditDescChanging(value);
					this.SendPropertyChanging();
					this._AuditDesc = value;
					this.SendPropertyChanged("AuditDesc");
					this.OnAuditDescChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ListReceivedDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> ListReceivedDate
		{
			get
			{
				return this._ListReceivedDate;
			}
			set
			{
				if ((this._ListReceivedDate != value))
				{
					this.OnListReceivedDateChanging(value);
					this.SendPropertyChanging();
					this._ListReceivedDate = value;
					this.SendPropertyChanged("ListReceivedDate");
					this.OnListReceivedDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StatusID", DbType="int NOT NULL")]
		public int StatusID
		{
			get
			{
				return this._StatusID;
			}
			set
			{
				if ((this._StatusID != value))
				{
					this.OnStatusIDChanging(value);
					this.SendPropertyChanging();
					this._StatusID = value;
					this.SendPropertyChanged("StatusID");
					this.OnStatusIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FileReceivedDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> FileReceivedDate
		{
			get
			{
				return this._FileReceivedDate;
			}
			set
			{
				if ((this._FileReceivedDate != value))
				{
					this.OnFileReceivedDateChanging(value);
					this.SendPropertyChanging();
					this._FileReceivedDate = value;
					this.SendPropertyChanged("FileReceivedDate");
					this.OnFileReceivedDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsArchived", DbType="bit")]
		public bool IsArchived
		{
			get
			{
				return this._IsArchived;
			}
			set
			{
				if ((this._IsArchived != value))
				{
					this.OnIsArchivedChanging(value);
					this.SendPropertyChanging();
					this._IsArchived = value;
					this.SendPropertyChanged("IsArchived");
					this.OnIsArchivedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="ClientDetail_AuditDetail", Storage="_ClientDetail", ThisKey="ClientID", OtherKey="ClientID", IsForeignKey=true)]
		public ClientDetail ClientDetail
		{
			get
			{
				return this._ClientDetail.Entity;
			}
			set
			{
				ClientDetail previousValue = this._ClientDetail.Entity;
				if (((previousValue != value) 
							|| (this._ClientDetail.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._ClientDetail.Entity = null;
						previousValue.AuditDetails.Remove(this);
					}
					this._ClientDetail.Entity = value;
					if ((value != null))
					{
						value.AuditDetails.Add(this);
						this._ClientID = value.ClientID;
					}
					else
					{
						this._ClientID = default(int);
					}
					this.SendPropertyChanged("ClientDetail");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ClientDetail")]
	public partial class ClientDetail : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ClientID;
		
		private string _ClientNm;
		
		private System.Nullable<bool> _IsActive;
		
		private EntitySet<AuditDetail> _AuditDetails;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnClientIDChanging(int value);
    partial void OnClientIDChanged();
    partial void OnClientNmChanging(string value);
    partial void OnClientNmChanged();
    partial void OnIsActiveChanging(System.Nullable<bool> value);
    partial void OnIsActiveChanged();
    #endregion
		
		public ClientDetail()
		{
			this._AuditDetails = new EntitySet<AuditDetail>(new Action<AuditDetail>(this.attach_AuditDetails), new Action<AuditDetail>(this.detach_AuditDetails));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ClientID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int ClientID
		{
			get
			{
				return this._ClientID;
			}
			set
			{
				if ((this._ClientID != value))
				{
					this.OnClientIDChanging(value);
					this.SendPropertyChanging();
					this._ClientID = value;
					this.SendPropertyChanged("ClientID");
					this.OnClientIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ClientNm", DbType="VarChar(30)")]
		public string ClientNm
		{
			get
			{
				return this._ClientNm;
			}
			set
			{
				if ((this._ClientNm != value))
				{
					this.OnClientNmChanging(value);
					this.SendPropertyChanging();
					this._ClientNm = value;
					this.SendPropertyChanged("ClientNm");
					this.OnClientNmChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsActive", DbType="Bit")]
		public System.Nullable<bool> IsActive
		{
			get
			{
				return this._IsActive;
			}
			set
			{
				if ((this._IsActive != value))
				{
					this.OnIsActiveChanging(value);
					this.SendPropertyChanging();
					this._IsActive = value;
					this.SendPropertyChanged("IsActive");
					this.OnIsActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="ClientDetail_AuditDetail", Storage="_AuditDetails", ThisKey="ClientID", OtherKey="ClientID")]
		public EntitySet<AuditDetail> AuditDetails
		{
			get
			{
				return this._AuditDetails;
			}
			set
			{
				this._AuditDetails.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_AuditDetails(AuditDetail entity)
		{
			this.SendPropertyChanging();
			entity.ClientDetail = this;
		}
		
		private void detach_AuditDetails(AuditDetail entity)
		{
			this.SendPropertyChanging();
			entity.ClientDetail = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="AcesSQL.acesdata.AUDITRK")]
	public partial class AUDITRK : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _CUSTOMERNBR;
		
		private string _AUDITID;
		
		private string _LOANNBR;
		
		private string _LASTNAME;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCUSTOMERNBRChanging(int value);
    partial void OnCUSTOMERNBRChanged();
    partial void OnAUDITIDChanging(string value);
    partial void OnAUDITIDChanged();
    partial void OnLOANNBRChanging(string value);
    partial void OnLOANNBRChanged();
    partial void OnLASTNAMEChanging(string value);
    partial void OnLASTNAMEChanged();
    #endregion
		
		public AUDITRK()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CUSTOMERNBR", DbType="SmallInt NOT NULL", IsPrimaryKey=true)]
		public int CUSTOMERNBR
		{
			get
			{
				return this._CUSTOMERNBR;
			}
			set
			{
				if ((this._CUSTOMERNBR != value))
				{
					this.OnCUSTOMERNBRChanging(value);
					this.SendPropertyChanging();
					this._CUSTOMERNBR = value;
					this.SendPropertyChanged("CUSTOMERNBR");
					this.OnCUSTOMERNBRChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AUDITID", DbType="Char(5) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string AUDITID
		{
			get
			{
				return this._AUDITID;
			}
			set
			{
				if ((this._AUDITID != value))
				{
					this.OnAUDITIDChanging(value);
					this.SendPropertyChanging();
					this._AUDITID = value;
					this.SendPropertyChanged("AUDITID");
					this.OnAUDITIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LOANNBR", DbType="Char(18) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string LOANNBR
		{
			get
			{
				return this._LOANNBR;
			}
			set
			{
				if ((this._LOANNBR != value))
				{
					this.OnLOANNBRChanging(value);
					this.SendPropertyChanging();
					this._LOANNBR = value;
					this.SendPropertyChanged("LOANNBR");
					this.OnLOANNBRChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LASTNAME", DbType="Char(15)")]
		public string LASTNAME
		{
			get
			{
				return this._LASTNAME;
			}
			set
			{
				if ((this._LASTNAME != value))
				{
					this.OnLASTNAMEChanging(value);
					this.SendPropertyChanging();
					this._LASTNAME = value;
					this.SendPropertyChanged("LASTNAME");
					this.OnLASTNAMEChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class LoanDetail
	{
		
		private string _LastName;
		
		private string _LoanNbr;
		
		private string _AuditID;
		
		private string _AuditName;
		
		private string _LoanPath;
		
		private int _CompanyID;
		
		private System.Nullable<bool> _IsFulfillment;
		
		private System.Nullable<int> _LoanFileAuditID;
		
		public LoanDetail()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastName", CanBeNull=false)]
		public string LastName
		{
			get
			{
				return this._LastName;
			}
			set
			{
				if ((this._LastName != value))
				{
					this._LastName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoanNbr", CanBeNull=false)]
		public string LoanNbr
		{
			get
			{
				return this._LoanNbr;
			}
			set
			{
				if ((this._LoanNbr != value))
				{
					this._LoanNbr = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AuditID", CanBeNull=false)]
		public string AuditID
		{
			get
			{
				return this._AuditID;
			}
			set
			{
				if ((this._AuditID != value))
				{
					this._AuditID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AuditName", CanBeNull=false)]
		public string AuditName
		{
			get
			{
				return this._AuditName;
			}
			set
			{
				if ((this._AuditName != value))
				{
					this._AuditName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoanPath", CanBeNull=false)]
		public string LoanPath
		{
			get
			{
				return this._LoanPath;
			}
			set
			{
				if ((this._LoanPath != value))
				{
					this._LoanPath = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CompanyID")]
		public int CompanyID
		{
			get
			{
				return this._CompanyID;
			}
			set
			{
				if ((this._CompanyID != value))
				{
					this._CompanyID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsFulfillment")]
		public System.Nullable<bool> IsFulfillment
		{
			get
			{
				return this._IsFulfillment;
			}
			set
			{
				if ((this._IsFulfillment != value))
				{
					this._IsFulfillment = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoanFileAuditID")]
		public System.Nullable<int> LoanFileAuditID
		{
			get
			{
				return this._LoanFileAuditID;
			}
			set
			{
				if ((this._LoanFileAuditID != value))
				{
					this._LoanFileAuditID = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute()]
	public partial class Vendor : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Nullable<int> _VendorID;
		
		private string _VendorNm;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnVendorIDChanging(System.Nullable<int> value);
    partial void OnVendorIDChanged();
    partial void OnVendorNmChanging(string value);
    partial void OnVendorNmChanged();
    #endregion
		
		public Vendor()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VendorID", DbType="int", IsPrimaryKey=true)]
		public System.Nullable<int> VendorID
		{
			get
			{
				return this._VendorID;
			}
			set
			{
				if ((this._VendorID != value))
				{
					this.OnVendorIDChanging(value);
					this.SendPropertyChanging();
					this._VendorID = value;
					this.SendPropertyChanged("VendorID");
					this.OnVendorIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VendorNm", CanBeNull=false)]
		public string VendorNm
		{
			get
			{
				return this._VendorNm;
			}
			set
			{
				if ((this._VendorNm != value))
				{
					this.OnVendorNmChanging(value);
					this.SendPropertyChanging();
					this._VendorNm = value;
					this.SendPropertyChanged("VendorNm");
					this.OnVendorNmChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute()]
	public partial class Service : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ServiceID;
		
		private int _VendorID;
		
		private string _ServiceNm;
		
		private decimal _StandardFeeAmt;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnServiceIDChanging(int value);
    partial void OnServiceIDChanged();
    partial void OnVendorIDChanging(int value);
    partial void OnVendorIDChanged();
    partial void OnServiceNmChanging(string value);
    partial void OnServiceNmChanged();
    partial void OnStandardFeeAmtChanging(decimal value);
    partial void OnStandardFeeAmtChanged();
    #endregion
		
		public Service()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServiceID", DbType="int", IsPrimaryKey=true)]
		public int ServiceID
		{
			get
			{
				return this._ServiceID;
			}
			set
			{
				if ((this._ServiceID != value))
				{
					this.OnServiceIDChanging(value);
					this.SendPropertyChanging();
					this._ServiceID = value;
					this.SendPropertyChanged("ServiceID");
					this.OnServiceIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VendorID", DbType="int")]
		public int VendorID
		{
			get
			{
				return this._VendorID;
			}
			set
			{
				if ((this._VendorID != value))
				{
					this.OnVendorIDChanging(value);
					this.SendPropertyChanging();
					this._VendorID = value;
					this.SendPropertyChanged("VendorID");
					this.OnVendorIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServiceNm", DbType="varchar(50)", CanBeNull=false)]
		public string ServiceNm
		{
			get
			{
				return this._ServiceNm;
			}
			set
			{
				if ((this._ServiceNm != value))
				{
					this.OnServiceNmChanging(value);
					this.SendPropertyChanging();
					this._ServiceNm = value;
					this.SendPropertyChanged("ServiceNm");
					this.OnServiceNmChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StandardFeeAmt", DbType="money")]
		public decimal StandardFeeAmt
		{
			get
			{
				return this._StandardFeeAmt;
			}
			set
			{
				if ((this._StandardFeeAmt != value))
				{
					this.OnStandardFeeAmtChanging(value);
					this.SendPropertyChanging();
					this._StandardFeeAmt = value;
					this.SendPropertyChanged("StandardFeeAmt");
					this.OnStandardFeeAmtChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute()]
	public partial class Loan_Service : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _LoanServiceID;
		
		private int _ClientID;
		
		private string _AuditID;
		
		private string _LoanID;
		
		private int _ServiceID;
		
		private System.DateTime _ServiceStatusDate;
		
		private decimal _FeeChargedAmt;
		
		private string _CreatedBy;
		
		private string _UpdatedBy;
		
		private System.DateTime _CreatedDate;
		
		private int _StatusID;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnLoanServiceIDChanging(int value);
    partial void OnLoanServiceIDChanged();
    partial void OnClientIDChanging(int value);
    partial void OnClientIDChanged();
    partial void OnAuditIDChanging(string value);
    partial void OnAuditIDChanged();
    partial void OnLoanIDChanging(string value);
    partial void OnLoanIDChanged();
    partial void OnServiceIDChanging(int value);
    partial void OnServiceIDChanged();
    partial void OnServiceStatusDateChanging(System.DateTime value);
    partial void OnServiceStatusDateChanged();
    partial void OnFeeChargedAmtChanging(decimal value);
    partial void OnFeeChargedAmtChanged();
    partial void OnCreatedByChanging(string value);
    partial void OnCreatedByChanged();
    partial void OnUpdatedByChanging(string value);
    partial void OnUpdatedByChanged();
    partial void OnCreatedDateChanging(System.DateTime value);
    partial void OnCreatedDateChanged();
    partial void OnStatusIDChanging(int value);
    partial void OnStatusIDChanged();
    #endregion
		
		public Loan_Service()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoanServiceID", AutoSync=AutoSync.OnInsert, DbType="int", IsPrimaryKey=true, IsDbGenerated=true)]
		public int LoanServiceID
		{
			get
			{
				return this._LoanServiceID;
			}
			set
			{
				if ((this._LoanServiceID != value))
				{
					this.OnLoanServiceIDChanging(value);
					this.SendPropertyChanging();
					this._LoanServiceID = value;
					this.SendPropertyChanged("LoanServiceID");
					this.OnLoanServiceIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ClientID", DbType="int")]
		public int ClientID
		{
			get
			{
				return this._ClientID;
			}
			set
			{
				if ((this._ClientID != value))
				{
					this.OnClientIDChanging(value);
					this.SendPropertyChanging();
					this._ClientID = value;
					this.SendPropertyChanged("ClientID");
					this.OnClientIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AuditID", DbType="varchar(5)", CanBeNull=false)]
		public string AuditID
		{
			get
			{
				return this._AuditID;
			}
			set
			{
				if ((this._AuditID != value))
				{
					this.OnAuditIDChanging(value);
					this.SendPropertyChanging();
					this._AuditID = value;
					this.SendPropertyChanged("AuditID");
					this.OnAuditIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoanID", DbType="varchar(18)", CanBeNull=false)]
		public string LoanID
		{
			get
			{
				return this._LoanID;
			}
			set
			{
				if ((this._LoanID != value))
				{
					this.OnLoanIDChanging(value);
					this.SendPropertyChanging();
					this._LoanID = value;
					this.SendPropertyChanged("LoanID");
					this.OnLoanIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServiceID", DbType="int")]
		public int ServiceID
		{
			get
			{
				return this._ServiceID;
			}
			set
			{
				if ((this._ServiceID != value))
				{
					this.OnServiceIDChanging(value);
					this.SendPropertyChanging();
					this._ServiceID = value;
					this.SendPropertyChanged("ServiceID");
					this.OnServiceIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServiceStatusDate", DbType="datetime")]
		public System.DateTime ServiceStatusDate
		{
			get
			{
				return this._ServiceStatusDate;
			}
			set
			{
				if ((this._ServiceStatusDate != value))
				{
					this.OnServiceStatusDateChanging(value);
					this.SendPropertyChanging();
					this._ServiceStatusDate = value;
					this.SendPropertyChanged("ServiceStatusDate");
					this.OnServiceStatusDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeeChargedAmt", DbType="money")]
		public decimal FeeChargedAmt
		{
			get
			{
				return this._FeeChargedAmt;
			}
			set
			{
				if ((this._FeeChargedAmt != value))
				{
					this.OnFeeChargedAmtChanging(value);
					this.SendPropertyChanging();
					this._FeeChargedAmt = value;
					this.SendPropertyChanged("FeeChargedAmt");
					this.OnFeeChargedAmtChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedBy", DbType="varchar(50)", CanBeNull=false)]
		public string CreatedBy
		{
			get
			{
				return this._CreatedBy;
			}
			set
			{
				if ((this._CreatedBy != value))
				{
					this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdatedBy", DbType="varchar(50)", CanBeNull=false)]
		public string UpdatedBy
		{
			get
			{
				return this._UpdatedBy;
			}
			set
			{
				if ((this._UpdatedBy != value))
				{
					this.OnUpdatedByChanging(value);
					this.SendPropertyChanging();
					this._UpdatedBy = value;
					this.SendPropertyChanged("UpdatedBy");
					this.OnUpdatedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedDate", DbType="datetime")]
		public System.DateTime CreatedDate
		{
			get
			{
				return this._CreatedDate;
			}
			set
			{
				if ((this._CreatedDate != value))
				{
					this.OnCreatedDateChanging(value);
					this.SendPropertyChanging();
					this._CreatedDate = value;
					this.SendPropertyChanged("CreatedDate");
					this.OnCreatedDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StatusID", DbType="int")]
		public int StatusID
		{
			get
			{
				return this._StatusID;
			}
			set
			{
				if ((this._StatusID != value))
				{
					this.OnStatusIDChanging(value);
					this.SendPropertyChanging();
					this._StatusID = value;
					this.SendPropertyChanged("StatusID");
					this.OnStatusIDChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
