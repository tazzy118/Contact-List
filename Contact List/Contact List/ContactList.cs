using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace Contact_List
{
    internal class ContactList
    {
        //Dictionary to store contacts, with the contact number as the key
        private Dictionary<string, Contact> priv_ContactDictionary { get; set; } = new Dictionary<string, Contact>();

        //to serialize contacts to a JSON file and had help from cousin https://stackoverflow.com/questions/9110724/serializing-a-list-to-json
        public void SaveContactsToJson(string filePath)
        {
            //converts the dictionary(contacts) to a list before serializing
            List<Contact> contacts = new List<Contact>(priv_ContactDictionary.Values);
            //serializes the list of contacts to JSON format
            string json = JsonConvert.SerializeObject(contacts);
            //Writes the JSON to the specified file
            File.WriteAllText(filePath, json);
        }
        //This to deserialize contacts from a JSON file
        public void LoadContactsFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                //Reads JSON string from the file https://stackoverflow.com/questions/72967625/i-want-to-deserialize-the-json-file-and-output-its-data
                string json = File.ReadAllText(filePath);

                //Deserialize the JSON string to a list of contacts
                List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(json);

                // Clear the existing contacts dictionary and add the deserialized contacts
                priv_ContactDictionary.Clear();
                foreach (var contact in contacts)
                {
                    // Add each contact to the dictionary, using the contact number as the key
                    priv_ContactDictionary[contact.Number] = contact;
                }
            }
            else
            {
                Console.WriteLine("File not found: " + filePath);
            }
        }
        //This is where to add a new contact to the contact list
        public void AddContact(Contact contact)
        {
            try
            {
                //this checks if the contact name is null, empty, or whitespace, or if the phone number is invalid
                if (string.IsNullOrWhiteSpace(contact.Name) || !IsPhoneNumberValid(contact.Number))
                {
                    //shows a message indicating invalid contact details
                    Console.WriteLine("Invalid contact details. Contact not added.");
                    return;
                }
                //Checks if the contact number already exists https://stackoverflow.com/questions/2829873/how-can-i-detect-if-this-dictionary-key-exists-in-c
                if (priv_ContactDictionary.ContainsKey(contact.Number))
                {
                    //Display a message indicating that the contact already exists
                    Console.WriteLine("Contact with the same number already exists. Contact not added.");
                    return; // Exit the method
                }

                //adds the valid contact to the contact dictionary
                priv_ContactDictionary[contact.Number] = contact;

                //sorts the contacts alphabetically by name
                SortContactsByName();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the contact: {ex.Message}");
            }
        }
        //This is to check if a phone number is valid.
        private bool IsPhoneNumberValid(string phoneNumber)
        {
            // Check if the phone number is not null, empty, or whitespace,consists only of digits, and has a length of 10 digits. If not 10 digits it will pop up with error. https://stackoverflow.com/questions/7438957/check-if-string-is-empty-or-all-spaces-in-c-sharp
            return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.All(char.IsDigit) && phoneNumber.Length == 10;
        }
        //This to sort contacts alphabetically by name
        public void SortContactsByName()

        {
            //Sorts the dictionary inputs (contacts) by contact name
            var sortedContacts = priv_ContactDictionary.OrderBy(pair => pair.Value.Name);

            //this creates a new dictionary to hold the sorted contacts
            var sortedDictionary = sortedContacts.ToDictionary(pair => pair.Key, pair => pair.Value);

            //Updates the original dictionary with the sorted contacts
            priv_ContactDictionary = sortedDictionary;
        }
        //this is to delete a contact based on the number
        public void DeleteContact(string number)
        {
            try
            {
                //trims whitespace from the input.
                number = number.Trim();

                //if nothing is inputted it would show a output. https://stackoverflow.com/questions/7438957/check-if-string-is-empty-or-all-spaces-in-c-sharp
                if (string.IsNullOrWhiteSpace(number))
                {
                    Console.WriteLine("Invalid contact number. Contact not deleted.");
                    return;
                }
                //https://stackoverflow.com/questions/1636885/remove-item-in-dictionary-based-on-value
                if (priv_ContactDictionary.ContainsKey(number))
                {
                    priv_ContactDictionary.Remove(number);
                    Console.WriteLine("Contact deleted successfully.");
                }
                else
                {    //if number not recognised it'll output this.
                    Console.WriteLine("Contact not found with the given number. Contact not deleted.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the contact: {ex.Message}");
            }
        }
        //This is to display contact details by number
        public void ShowContact(string number)
        {
            if (priv_ContactDictionary.TryGetValue(number, out Contact contact))
            {
                Console.WriteLine($"Contact: {contact.Name}, {contact.Number}");
            }
            else
            {
                Console.WriteLine("Contact not found with the given number.");
            }
        }
        //To display all contacts
        public void ShowAllContact()
        {
            foreach (var contact in priv_ContactDictionary.Values)
            {
                Console.WriteLine($"Contact: {contact.Name}, {contact.Number}");
            }
        }
        //this is search for contacts by name or phone number and display the results.
        public void ShowMatchingContact(string searchQuery)
        {
            try
            {
                //Convert the search query to lowercase for case-insensitive search
                string query = searchQuery.ToLower();

                //Find the contacts that match the search query using parallel processing.
                var matchingContacts = priv_ContactDictionary.Values
                    .AsParallel() //enables parallel processing https://stackoverflow.com/questions/13942998/running-a-simple-linq-query-in-parallel
                    .Where(contact =>
                        contact.Name.ToLower().Contains(query) || contact.Number.ToLower().Contains(query)
                    ).ToList();
                //checks if any matching contacts were found.
                if (matchingContacts.Count == 0)
                {
                    Console.WriteLine("No contacts found matching the search query.");
                }
                else
                {
                    foreach (var contact in matchingContacts)
                    {
                        Console.WriteLine($"Contact: {contact.Name}, {contact.Number}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while searching for contacts: {ex.Message}");
            }
        }
        //This is to edit contact details
        public void EditContact(string number)
        {
            if (priv_ContactDictionary.TryGetValue(number, out Contact contact))
            {
                Console.WriteLine("Enter new name for the contact: ");
                string newName = Console.ReadLine();
                Console.WriteLine("Enter new number for the contact: ");
                string newNumber = Console.ReadLine();
                //trims whitespace from the input to make program run cleaner.
                newName = newName.Trim();
                newNumber = newNumber.Trim();

                contact.Name = newName;
                //If the number is has changed, remove the older input and add the updated contact.
                if (newNumber != number)
                {
                    priv_ContactDictionary.Remove(number);
                    priv_ContactDictionary[newNumber] = contact;
                }
                Console.WriteLine("Contact details updated successfully.");
            }
            else
            {
                Console.WriteLine("Contact not found with the given number.");
            }
        }
    }
}
