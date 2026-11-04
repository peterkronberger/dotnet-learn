// File GlobalUsings.cs: valid for every file of the program
global using System.Text;
global using Alerts.Mail;

// File Program.cs: needs no using directive for StringBuilder or Notice
StringBuilder builder = new StringBuilder("Disk full");
Notice notice = new Notice(builder.ToString());
