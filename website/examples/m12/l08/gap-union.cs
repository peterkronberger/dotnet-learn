// Choose the text by the case type of a contact
Contact contact = new Phone("555-0100");

string text = contact switch
{
    Email email => $"mail to {email.Address}",
    Phone phone => $"call {phone.Number}",
};
Console.WriteLine(text);

record Email(string Address);
record Phone(string Number);
union Contact(Email, Phone);
