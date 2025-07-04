using Microsoft.Data.SqlClient;
using System.Data;

string connectionString = @"Data Source=DESKTOP-0S9BUVM;Database=PreparationADO;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True;";


// Affiche les informations de la connexion à la base de données
Console.WriteLine($"\n1. Affiche les informations de la connexion à la base de données\n");
using (SqlConnection connection = new SqlConnection(connectionString))
{
    Console.WriteLine($"ConnectionString: {connection.ConnectionString}");
    Console.WriteLine($"State: {connection.State}");

    connection.Open();

    Console.WriteLine($"State: {connection.State}");

    Console.WriteLine($"Data Source: {connection.DataSource}");
    Console.WriteLine($"Database: {connection.Database}");

    connection.Close();

    Console.WriteLine($"State: {connection.State}");
}

/*
 *  MODE CONNECTÉ
 *  La connexion reste ouverte le temps que la requête soit exécutée et retourne le résultat
 */

// Exécute une commande SQL sur la base de données
Console.WriteLine($"\n2. Exécute une commande SQL sur la base de données\n");
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM [dbo].[User]";

        // Permet de récupérer plusieurs enregistrements
        //command.ExecuteReader();

        // Permet de récupérer un seul enregistrement
        //command.ExecuteScalar();

        // Permet d'exécuter une commande qui ne retourne pas de données (INSERT, UPDATE, DELETE)
        //command.ExecuteNonQuery();
    }

    connection.Close();
}


// ExecuteReader permet de lire les enregistrements retournés par une requête SQL
Console.WriteLine($"\n3. ExecuteReader permet de lire les enregistrements retournés par une requête SQL\n");
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT [Id], [FirstName], [LastName] FROM [dbo].[User]";

        using (SqlDataReader reader = command.ExecuteReader())
        {
            // Permet de lire les enregistrements un par un
            while (reader.Read())
            {
                int id = (int)reader["Id"];
                string firstName = (string)reader["FirstName"];
                string lastName = (string)reader["LastName"];

                // Affiche les valeurs des colonnes Id, FirstName et LastName
                Console.WriteLine($"Id: {id}, FirstName: {firstName}, LastName: {lastName}");
            }
        }
    }

    connection.Close();
}


// ExecuteScalar permet de récupérer une seule valeur d'une requête SQL
Console.WriteLine($"\n4. ExecuteScalar permet de récupérer une seule valeur d'une requête SQL\n");
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT COUNT(*) FROM [dbo].[User]";

        // Récupère le nombre d'enregistrements dans la table User
        int count = (int)command.ExecuteScalar();

        // Affiche le nombre d'enregistrements
        Console.WriteLine($"Nombre d'enregistrements dans la table User: {count}");
    }

    connection.Close();
}


// ExecuteNonQuery permet d'exécuter une commande SQL qui ne retourne pas de données
Console.WriteLine($"\n5. ExecuteNonQuery permet d'exécuter une commande SQL qui ne retourne pas de données\n");
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    using (SqlCommand command = connection.CreateCommand())
    {
        // Update une colonne dans la table User
        command.CommandText = "UPDATE [dbo].[User] SET [UserName] = 'Quentin' WHERE Id = 2";

        // Exécute la commande et récupère le nombre de lignes affectées
        int rowsAffected = command.ExecuteNonQuery();

        // Affiche le nombre de lignes affectées
        Console.WriteLine($"Nombre de lignes affectées: {rowsAffected}");
    }

    connection.Close();
}


/*
 *  MODE DÉCONNECTÉ
 */

// Utilise SqlDataAdapter pour remplir un DataSet avec les données de la base de données
Console.WriteLine($"\n6. Utilise SqlDataAdapter pour remplir un DataSet avec les données de la base de données\n");
using (SqlConnection connection = new SqlConnection(connectionString))
{

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT [Id], [FirstName] FROM [dbo].[User]";

        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet dataSet = new DataSet();

        adapter.Fill(dataSet);

        if (dataSet.Tables.Count > 0)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                Console.WriteLine($"Id: {row["Id"]}, Prenom: {row["FirstName"]}");
            }
        }
    }
}

// Requête avec un paramètre
Console.WriteLine($"\n7. Requête avec un paramètre\n");
int idParam = 2;
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    using (SqlCommand command = connection.CreateCommand())
    {
        // Ajoute un paramètre à la commande SQL
        command.CommandText = "SELECT [Id], [FirstName], [LastName] FROM [dbo].[User] WHERE Id = @idParam";

        // Utilise un paramètre pour éviter les injections SQL
        command.Parameters.AddWithValue("@idParam", idParam);

        using (SqlDataReader reader = command.ExecuteReader())
        {
            // Lit les enregistrements retournés par la requête
            while (reader.Read())
            {
                int id = (int)reader["Id"];
                string firstName = (string)reader["FirstName"];
                string lastName = (string)reader["LastName"];

                // Affiche les valeurs des colonnes Id et FirstName
                Console.WriteLine($"Id: {id}, FirstName: {firstName}, LastName: {lastName}");
            }
        }
    }

    connection.Close();
}
