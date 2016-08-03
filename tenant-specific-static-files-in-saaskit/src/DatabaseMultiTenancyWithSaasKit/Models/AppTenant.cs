namespace DatabaseMultiTenancyWithSaasKit.Models
{
    public class AppTenant
    {
        public int AppTenantId { get; set; }
        public string Name { get; set; }
        public string Hostname { get; set; }
        public string Folder { get; set; }
    }
}