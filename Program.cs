using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//////////////////////////
using Newtonsoft.Json;//// Don't forget to install Newtonsoft.Json Package
//////////////////////////

// Define the file path for the database JSON file
string DatabaseFilePath = Environment.CurrentDirectory + "\\" + "\\[DATABASE]\\" + "database.json"; // create folder with name: [DATABASE] and file with name database.json in the same directory of program

// Create a list to store member objects
List<Member> database = new List<Member>();

// Load the database from the JSON file
LoadDatabase();

// Create a loop for user interaction
while (true)
{

    // Refresh the database
    LoadDatabase();

    // Clear the console
    Console.Clear();

    // Display menu options
    Console.WriteLine("### Select an option ###" + Environment.NewLine);
    Console.WriteLine("1. View All Records");
    Console.WriteLine("2. Add a Record");
    Console.WriteLine("3. Update a Record");
    Console.WriteLine("4. Delete a Record");
    Console.WriteLine("5. Select a Record");
    Console.WriteLine("6. Exit" + Environment.NewLine);
    Console.WriteLine("########################");
    Console.Write("#Enter your choice: ");


    // Check if user input is a valid integer
    if (int.TryParse(Console.ReadLine(), out int choice))
    {
        switch (choice)
        {
            case 1:
                ViewAllRecords();
                break;
            case 2:
                AddRecord();
                break;
            case 3:
                UpdateRecord();
                break;
            case 4:
                DeleteRecord();
                break;
            case 5:
                SelectRecord();
                break;
            case 6:
                // Save the database and exit the program
                SaveDatabase();
                Environment.Exit(0);
                return;
            default:
                Console.WriteLine("Invalid option. Try again.");
                Console.ReadLine();
                break;
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a number.");
    }

    Console.WriteLine();
}

// Function to load the database from the JSON file
void LoadDatabase()
{
    try
    {
        if (File.Exists(DatabaseFilePath))
        {
            // Read JSON data from the file and deserialize it into the database list
            string json = File.ReadAllText(DatabaseFilePath);
            database = JsonConvert.DeserializeObject<List<Member>>(json);
        }
        else
        {
            Console.WriteLine("Error");
            Console.ReadLine();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error" + $" {ex.Message}");
        Console.ReadLine();
    }
}


// Function to save the database to the JSON file
void SaveDatabase()
{
    try
    {
        // Serialize the database list into JSON format
        string json = JsonConvert.SerializeObject(database, Formatting.Indented); // Use Formatting.Indented

        // Write the JSON data to the file
        File.WriteAllText(DatabaseFilePath, json);
        Console.WriteLine("Database saved successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
    Console.ReadLine();
}

// Function to view all records in the database
void ViewAllRecords()
{
    Console.WriteLine("Database Records:");
    if (database != null)
    {
        // Iterate through each member object in the database and display its properties
        foreach (var person in database)
        {
            Console.WriteLine($"ID: {person.Id}, Name: {person.Name}, Age: {person.Age}");
        }
    }
    Console.ReadLine();
}

// Function to add a new record to the database
void AddRecord()
{

    // Read the existing JSON data from the file
    string jsonData = File.ReadAllText(DatabaseFilePath);

    // Deserialize the JSON data into a list of member objects
    List<Member> records = JsonConvert.DeserializeObject<List<Member>>(jsonData);

    // Prompt user to enter record details
    Console.Write("Enter Id: ");
    string id = Console.ReadLine();
    int Id = 0;


    bool isAgeInt = false;
    bool isIdInt = false;
    bool idExists;

    try
    {
        // Parse ID input to integer
        Id = int.Parse(id);
        isIdInt = true;

        // Check if the entered ID already exists in the database
        idExists = records.Any(record => record.Id == Id);

        if (idExists)
        {
            Console.WriteLine("ID already exists. Please enter a different ID.");
            Console.ReadLine();
        }
        else
        {
            // Prompt user to enter name and age
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Age: ");
            string age = Console.ReadLine();
            int Age = 0;

            // Parse age input to integer
            Age = int.Parse(age);
            isAgeInt = true;

            // If both ID and age are valid integers, create a new member record
            if (isAgeInt && isIdInt)
            {
                // Create a new record
                Member newRecord = new Member
                {
                    Id = Id, // Set the ID of the new record
                    Name = name,
                    Age = Age
                };

                records.Add(newRecord);

                // Serialize the updated list back to JSON
                string updatedJsonData = JsonConvert.SerializeObject(records, Formatting.Indented);

                // Write the JSON data back to the file
                File.WriteAllText(DatabaseFilePath, updatedJsonData);


                Console.WriteLine("Record added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid age. Record not added.");
            }
        }

    }
    catch (Exception ex)
    {
        isAgeInt = false;
        isIdInt = false;
    }
}


// Function to update an existing record in the database
void UpdateRecord()
{
    // Read JSON data from the file and deserialize it into a list of member objects
    string jsonData = File.ReadAllText(DatabaseFilePath);
    List<Member> records = JsonConvert.DeserializeObject<List<Member>>(jsonData);


    // Prompt user to enter the ID of the record to update
    Console.Write("Enter the ID of the record to update: ");
    if (int.TryParse(Console.ReadLine(), out int recordId))
    {
        // Find the record with the specified ID
        Member recordToUpdate = records.Find(r => r.Id == recordId);

        if (recordToUpdate != null)
        {
            // Prompt the user to enter the updated name
            Console.Write("Enter the updated name: ");
            string updatedName = Console.ReadLine();

            // Update the record
            recordToUpdate.Name = updatedName;

            // Prompt the user to enter the updated name
            Console.Write("Enter the updated Age: ");
            string StrupdatedAge = Console.ReadLine();
            int updatedAge = 0;
            bool isUpdatedAgeInt = false;

            try
            {
                // Parse updated age input to integer
                updatedAge = int.Parse(StrupdatedAge);
                isUpdatedAgeInt = true;
            }
            catch
            {
                isUpdatedAgeInt = false;
            }

            if (isUpdatedAgeInt)
            {
                // Update the record's age
                recordToUpdate.Age = updatedAge;
            }

            // Serialize the updated list back to JSON
            string updatedJsonData = JsonConvert.SerializeObject(records, Formatting.Indented);

            // Write the JSON data back to the file
            File.WriteAllText(DatabaseFilePath, updatedJsonData);

            Console.WriteLine("Record updated successfully.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Record with the specified ID not found.");
            Console.ReadLine();
        }
    }
    else
    {
        Console.WriteLine("Invalid ID.");
        Console.ReadLine();
    }
}

// Function to delete a record from the database
void DeleteRecord()
{
    // Read JSON data from the file and deserialize it into a list of member objects
    string jsonData = File.ReadAllText(DatabaseFilePath);
    List<Member> records = JsonConvert.DeserializeObject<List<Member>>(jsonData);


    // Prompt user to enter the ID of the record to delete
    Console.Write("Enter the ID of the record to delete: ");
    if (int.TryParse(Console.ReadLine(), out int recordId))
    {
        // Find the index of the record with the specified ID
        int recordToDelete = records.FindIndex(r => r.Id == recordId);

        if (recordToDelete != -1)
        {
            // Remove the record from the list
            records.RemoveAt(recordToDelete);

            // Serialize the updated list back to JSON
            string updatedJsonData = JsonConvert.SerializeObject(records, Formatting.Indented);

            // Write the JSON data back to the file
            File.WriteAllText(DatabaseFilePath, updatedJsonData);

            Console.WriteLine("Record deleted successfully.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Record with the specified ID not found.");
            Console.ReadLine();
        }
    }
    else
    {
        Console.WriteLine("Invalid ID.");
        Console.ReadLine();
    }
}

// Function to select and display a record from the database
void SelectRecord()
{
    // Read JSON data from the file and deserialize it into a list of member objects
    string jsonData = File.ReadAllText(DatabaseFilePath);
    List<Member> records = JsonConvert.DeserializeObject<List<Member>>(jsonData);


    // Prompt user to enter the ID of the record to select
    Console.Write("Enter the ID of the record you want to select: ");
    if (int.TryParse(Console.ReadLine(), out int selectedId))
    {
        // Find the record based on the specified ID
        Member selectedRecord = records.Find(record => record.Id == selectedId);

        if (selectedRecord != null)
        {
            // Display the selected record
            Console.WriteLine($"Selected Record - ID: {selectedRecord.Id}, Name: {selectedRecord.Name}, Name: {selectedRecord.Age}");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Record with the specified ID not found.");
            Console.ReadLine();
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format. Please enter a valid integer.");
        Console.ReadLine();
    }
}



// Define a class to represent a member with ID, name, and age properties
public class Member
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
