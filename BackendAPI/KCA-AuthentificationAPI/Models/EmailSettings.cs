﻿public class EmailSettings
{
    public string FromName { get; set; }
    public string FromEmail { get; set; }
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public bool UseSSL { get; set; }
    public string SmtpUser { get; set; }
    public string SmtpPass { get; set; }
}
