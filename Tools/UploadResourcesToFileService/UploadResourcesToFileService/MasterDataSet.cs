namespace Micajah.Common.Tools.UploadResourcesToFileService.MasterDataSetTableAdapters
{
    partial class InstanceTableAdapter
    {
        public InstanceTableAdapter(string connectionString)
            : base()
        {
            this.InitConnection();
            this._connection.ConnectionString = connectionString;
        }
    }
}