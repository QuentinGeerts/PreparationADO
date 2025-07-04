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
 *  Utile pour parcourir de gros volumes de données / données à jour
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
        command.CommandText = "SELECT [Id], [UserName] FROM [dbo].[User]";

        // La connexion reste active tant que nous utilisons SqlDataReader
        using (SqlDataReader reader = command.ExecuteReader())
        {
            // Permet de lire les enregistrements un par un
            while (reader.Read())
            {
                int id = (int)reader["Id"];
                string userName = (string)reader["UserName"];

                // Affiche les valeurs des colonnes Id, UserName
                Console.WriteLine($"Id: {id}, UserName: {userName}");
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
 *  La connexion s'ouvre le temps de charger les données
 *  Se ferme automatiquement une fois la réception terminée
 *  Utilisation des données en local
 *  Utile pour travailler hors connexion ou sur une "copie"
 */

// Utilise SqlDataAdapter pour remplir un DataSet avec les données de la base de données
Console.WriteLine($"\n6. Utilise SqlDataAdapter pour remplir un DataSet avec les données de la base de données\n");

DataSet dataSet = new DataSet();

using (SqlConnection connection = new SqlConnection(connectionString))
{

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT [Id], [UserName] FROM [dbo].[User]";

        SqlDataAdapter adapter = new SqlDataAdapter(command);

        adapter.Fill(dataSet); // Remplit le DataSet → connexion active 
    } // Fermeture de la connexion automatiquement
}

// Parcourt du DataSet hors connexion
if (dataSet.Tables.Count > 0)
{
    foreach (DataRow row in dataSet.Tables[0].Rows)
    {
        Console.WriteLine($"Id: {row["Id"]}, Prenom: {row["UserName"]}");
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
        command.CommandText = "SELECT [Id], [UserName] FROM [dbo].[User] WHERE Id = @idParam";

        // Utilise un paramètre pour éviter les injections SQL
        command.Parameters.AddWithValue("@idParam", idParam);

        using (SqlDataReader reader = command.ExecuteReader())
        {
            // Lit les enregistrements retournés par la requête
            while (reader.Read())
            {
                int id = (int)reader["Id"];
                string userName = (string)reader["UserName"];

                // Affiche les valeurs des colonnes Id et FirstName
                Console.WriteLine($"Id: {id}, UserName: {userName}");
            }
        }
    }

    connection.Close();
}

// Valeur "NULL" vs DBNull
Console.WriteLine($"\n8. Valeur \"NULL\" vs DBNull");

/*
 * C#       : null
 * SQL      : NULL
 * ADO.NET  : DBNull
 */

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT [Id], [FirstName] FROM [dbo].[User]";

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                int id = (int)reader["Id"];
                // <!> null != DBNull
                string? firstName = (reader["FirstName"] is DBNull) ? null : (string)reader["FirstName"];

                if (firstName is null || reader.IsDBNull("FirstName"))
                {
                    Console.WriteLine($"L'utilisateur avec l'id {id} n'a pas de prénom.");
                }
                else
                {
                    Console.WriteLine($"L'utilisateur avec l'id {id} s'appelle {firstName}");
                }

            }
        }
    }
}

// Procédures stockées
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "AddUser";
        command.CommandType = CommandType.StoredProcedure;

        // Paramètres d'entrée
        command.Parameters.AddWithValue("@UserName", "NomUtilisateur");
        command.Parameters.AddWithValue("@Email", "mon-email@gmail.com");
        command.Parameters.AddWithValue("@Password", "Test1234=");
        command.Parameters.AddWithValue("@LastName", "NomDeFamille");
        command.Parameters.AddWithValue("@FirstName", "Prénom");

        // Paramètres de sortie
        var userId = new SqlParameter("@UserId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        var errorMessage = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000)
        {
            Direction = ParameterDirection.Output
        };

        command.Parameters.Add(userId);
        command.Parameters.Add(errorMessage);

        command.ExecuteScalar();

        object userIdObject = userId.Value;
        string message = errorMessage.Value as string;

        if (!string.IsNullOrEmpty(message))
        {
            Console.WriteLine($"Erreur lors de l'insertion: {message}");
        }
        else
        {
            int id = (userIdObject != DBNull.Value) ? Convert.ToInt32(userIdObject) : -1;
            Console.WriteLine($"Utilisateur créé: {id}");
        }

    }
}