/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

USE [$(DatabaseName)];

-- Gestion des Users

SET IDENTITY_INSERT [dbo].[User] ON;

INSERT INTO [dbo].[User] (Id, UserName, Email, PasswordHash, LastName, FirstName)
VALUES
(1, 'admin', 'admin@admin.be', HASHBYTES('SHA2_256', 'Test1234='), 'Admin', 'Admin'),
(2, 'Oyumiro', 'quentin.geerts@bstorm.be', HASHBYTES('SHA2_256', 'Test1234='), 'Geerts', NULL);

SET IDENTITY_INSERT [dbo].[User] OFF;

-- Gestion des Tasks

SET IDENTITY_INSERT [dbo].[Task] ON;

INSERT INTO [dbo].[Task] (Id, UserId, Title, Description, Status)
VALUES
(1, 1, 'Aller travailler', 'Oui oui, il faut bien gagner sa croutte.', 'In Progress'),
(2, 1, 'Faire les courses', 'Acheter du lait et des cigarettes.', 'In Progress'),
(3, 1, 'Laver la voiture', 'Les routes sont trop sales.', 'Completed'),
(4, 1, 'Ranger la maison', 'Manger sur le bureau, tout est en bordel.', 'Pending'),
(5, 1, 'Changer la litière', 'Ca commence à puer.', 'Pending');

SET IDENTITY_INSERT [dbo].[Task] OFF;

