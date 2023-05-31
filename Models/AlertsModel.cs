namespace imcd_alert_sync_pro.Models
{
    public class AlertsModel
    {
        public List<AlertData>? Data { get; set; }
    }

    public class AlertData
    {
        public bool Seen { get; set; }
        public string? Id { get; set; }
        public string? TinyId { get; set; }
        public string? Alias { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
        public bool Acknowledged { get; set; }
        public bool IsSeen { get; set; }
        public List<string>? Tags { get; set; }
        public bool Snoozed { get; set; }
        public int Count { get; set; }
        public DateTime LastOccurredAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Source { get; set; }
        public string? Owner { get; set; }
        public string? Priority { get; set; }
        public List<Team>? Teams { get; set; }
        public List<Responder>? Responders { get; set; }
        public Integration? Integration { get; set; }
        public string? OwnerTeamId { get; set; }
    }

    public class Team
    {
        public string? Id { get; set; }
    }

    public class Responder
    {
        public string? Type { get; set; }
        public string? Id { get; set; }
    }

    public class Integration
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
}
