using System.Net.Mail;

namespace AddressBook
{
    // Just a simple in-memory "database" as I'm not too familiar with SQLite nor LocalDB
    public static class AddressBookStorage
    {
        private static List<AddressBookEntry> entries = new List<AddressBookEntry>();
        private static int lastID = -1;

        public static AddressBookEntry[] GetEntries()
        {
            return entries.ToArray<AddressBookEntry>();
        }

        public static InsertEntryResult InsertEntry(AddressBookEntry entry)
        {
            if (entry == null || entry.Email == null || entry.Name == null || entry.Surname == null || !IsValidEmail(entry.Email)) // entry == null probably redundant here, better safe than sorry
            {
                return InsertEntryResult.InvalidEntry;
            }
            if (entries.Any(item => item.Email == entry.Email)) // Are emails case sensitive? Probably should use .ToLower() or, better yet, convert to MailAddress before comparing? Google results inconclusive
            {
                return InsertEntryResult.EmailAlreadyExists;
            }
            entries.Add(entry);
            entry.ID = ++lastID; // could just use entries.Count, since we're not deleting entries
            return InsertEntryResult.OK;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email); // no regex, no problems!
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public enum InsertEntryResult
        {
            OK = 0,
            EmailAlreadyExists = 1,
            InvalidEntry = 2
        }
    }
}