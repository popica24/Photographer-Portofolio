﻿namespace MVCCore.Options
{
    public class EmailOptions
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
