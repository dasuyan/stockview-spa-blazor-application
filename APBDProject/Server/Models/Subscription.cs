namespace APBDProject.Server.Models
{
    public class Subscription
    {
        public int IdSubscription { get; set; }
        public string IdUser { get; set; }
        public string CompanyName { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Company Company { get; set; }
    }
}
