// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	9.11.2011. 11:56:20
// Description:	GEM2 Generated class for table SSI.dbo.STRUCT_REPRES_ATTACHMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "STRUCT_REPRES_ATTACHMENT")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Struct_repres_attach_PK
	{
		private Int32? _struct_repres_attach_PK;
		private Guid? _id;
		private Byte[] _disk_file;
		private String _attachmentname;
		private String _filetype;
		private Int32? _userID; 

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? struct_repres_attach_PK
		{
			get { return _struct_repres_attach_PK; }
			set { _struct_repres_attach_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Guid)]
		public Guid? Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Binary)]
		public Byte[] disk_file
		{
			get { return _disk_file; }
			set { _disk_file = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String attachmentname
		{
			get { return _attachmentname; }
			set { _attachmentname = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String filetype
		{
			get { return _filetype; }
			set { _filetype = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? userID
		{
			get { return _userID; }
			set { _userID = value; }
		}

		#endregion

		public Struct_repres_attach_PK() { }
		public Struct_repres_attach_PK(Int32? struct_repres_attach_PK, Guid? id, Byte[] disk_file, String attachmentname, String filetype, Int32? userID)
		{
			this.struct_repres_attach_PK = struct_repres_attach_PK;
			this.Id = id;
			this.disk_file = disk_file;
			this.attachmentname = attachmentname;
			this.filetype = filetype;
			this.userID = userID;
		}
	}

	public interface IStruct_repres_attach_PKOperations : ICRUDOperations<Struct_repres_attach_PK>
	{
        void DeleteNULLByUserID(Int32? userID);
	}
}
