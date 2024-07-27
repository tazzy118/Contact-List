using Contact_List;

class Program
{
    static void Main(string[] args)
    {
        //check if any command-line arguments are provided
        if (args.Length == 0)
        {
            //if no command-line arguments are provided, run the interactive menu
            RunInteractiveMenu();
        }
        else
        {
            //parses command-line arguments and does other arguments
            HandleCommandLineArguments(args);
        }
    }

    //This is to run the interactive menu
    static void RunInteractiveMenu()
    {
        //displays the interactive menu for the Contact List.
        Console.WriteLine("This is the Contact List Application");
        Console.WriteLine("");

        Console.WriteLine("Please choose an option");
        Console.WriteLine("______________________________________");
        Console.WriteLine("|        1: Add Person               |");
        Console.WriteLine("| 2: Show me the name of the contact |");
        Console.WriteLine("|  3: Show me all the contacts       |");
        Console.WriteLine("| 4: Search contact by their name    |");
        Console.WriteLine("|     5: Edit contact details        |");
        Console.WriteLine("|  6: Delete Contact from list       |");
        Console.WriteLine("| To Leave the application type '0'  |");
        Console.WriteLine("|____________________________________|");

        //create an instance of the ContactList class
        var contactList = new ContactList();
        string activeUserinput;
        //handles user input and execute corresponding actions https://stackoverflow.com/questions/2552501/switch-statement-in-c-sharp
        while ((activeUserinput = Console.ReadLine()) != "0")
        {
            switch (activeUserinput)
            {
                case "1":
                    //tells user to enter details and add a new contact.
                    Console.WriteLine("Enter the person's Name: ");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter their Number: ");
                    string number = Console.ReadLine();
                    var newContact = new Contact(name, number);
                    contactList.AddContact(newContact);
                    break;
                case "2":
                    //tells user to enter a contact number and display the contact details.
                    Console.WriteLine("Enter their Number: ");
                    string searchNumber = Console.ReadLine();
                    contactList.ShowContact(searchNumber);
                    break;
                case "3":
                    //shows details of all contacts
                    contactList.ShowAllContact();
                    break;
                case "4":
                    //Tells user to enter a contact name and display matching contacts https://stackoverflow.com/questions/65919930/find-nearly-matching-phrase-from-a-list-of-string
                    Console.WriteLine("Enter the name of the contact: ");
                    string searchPhrase = Console.ReadLine();
                    contactList.ShowMatchingContact(searchPhrase);
                    break;
                case "5":
                    //Tells user to enter a contact name/number and edit contact details.
                    Console.WriteLine("Enter the number of the contact to edit: ");
                    string editPhrase = Console.ReadLine();
                    contactList.EditContact(editPhrase);
                    break;
                case "6":
                    //Tells user to enter a contact name/number and delete the contact.
                    Console.WriteLine("Enter the number of the contact to delete: ");
                    string deletePhrase = Console.ReadLine();
                    contactList.DeleteContact(deletePhrase);
                    break;
                default:
                    //not valid option, tells user to re-enter
                    Console.WriteLine("Please re-enter an option");
                    break;
            }
            //tells user another option
            Console.WriteLine("Choose another option");
        }
    }
    //This is to handle command-line arguments
    static void HandleCommandLineArguments(string[] args)
    {
        //This checks if any arguments are provided 
        if (args.Length == 0)
        {
            Console.WriteLine("No command-line arguments provided.");
            return;
        }
        //parses the first argument to determine the action had some help too https://stackoverflow.com/questions/34438589/parsing-and-acting-on-commands
        string action = args[0].ToLower(); 
        //Creates an instance of the ContactList class.
        var contactList = new ContactList();
        //This does actions based on the command-line arguments.
        switch (action)
        {
            case "add":
                //handles add actions.
                if (args.Length < 3)
                {
                    Console.WriteLine("Invalid number of arguments for 'add' action.");
                    return;
                }
                string nameToAdd = args[1];
                string numberToAdd = args[2];
                var newContact = new Contact(nameToAdd, numberToAdd);
                contactList.AddContact(newContact);
                Console.WriteLine("Contact added successfully.");
                break;
            case "delete":
                //handles delete actions I had some help and advice from my cousin.
                if (args.Length < 2)
                {
                    Console.WriteLine("Invalid number of arguments for 'delete' action.");
                    return;
                }
                string numberToDelete = args[1];
                contactList.DeleteContact(numberToDelete);
                break;
            case "show":
                //handles show actions
                if (args.Length < 2)
                {
                    Console.WriteLine("Invalid number of arguments for 'show' action.");
                    return;
                }
                string numberToShow = args[1];
                contactList.ShowContact(numberToShow);
                break;
            default:
                //handle unrecognized actions.
                Console.WriteLine($"Unrecognized action: {action}");
                break;
        }
    }
}


