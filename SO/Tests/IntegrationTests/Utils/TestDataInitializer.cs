using Logic.Utils.Db;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Utils
{
    internal class TestDataInitializer
    {
        internal static void Seed(DatabaseContext context)
        {
            string truncateStatement = @"
DELETE FROM  [dbo].[Users]
DBCC CHECKIDENT ([Users], RESEED, 0)
DELETE FROM [dbo].[Comments]
DBCC CHECKIDENT ([Comments], RESEED, 0)
DELETE FROM [dbo].[Posts]
DBCC CHECKIDENT ([Posts], RESEED, 0)
DELETE FROM  [dbo].[PostTypes]
DBCC CHECKIDENT ([Posts], RESEED, 0)
";

            context.Database.ExecuteSqlRaw(truncateStatement);


            string insertStatement = @"
SET IDENTITY_INSERT [dbo].[PostTypes] ON 
INSERT [dbo].[PostTypes] ([Id], [Type], [IsDeleted], [CreateDate], [LastUpdateDate], [DeleteDate]) VALUES
(1, N'Question', 0, '2022-01-09', NULL, NULL),
(2, N'Answer', 0, '2022-01-09', NULL, NULL)
SET IDENTITY_INSERT [dbo].[PostTypes] OFF

SET IDENTITY_INSERT [dbo].[Posts] ON 
INSERT [dbo].[Posts] ([Id],[AcceptedAnswerId],[AnswerCount],[Body],[ClosedDate],[CommentCount],[CommunityOwnedDate],[CreateDate],[FavoriteCount],[LastActivityDate],[LastUpdateDate],[LastEditorDisplayName],[LastEditorUserId],[OwnerUserId],[PostTypeId],[Score],[Tags],[Title],[ViewCount],[IsDeleted],[DeleteDate]) VALUES 
(1, 0, 0, 'test loooooooooooooong booooodyyyyyyyyyyyyyyyyyyyyyy', null, 0, '2022-01-09', '2022-01-09', 0, '2022-01-09', '2022-01-09', 'Test user', 1, 1, 1, 0, null, 'Test title 1', 10, 0, null),
(2, 0, 0, 'test loooooooooooooong booooodyyyyyyyyyyyyyyyyyyyyyy', null, 0, '2022-01-09', '2022-01-09', 0, '2022-01-09', '2022-01-09', 'Test user', 1, 1, 1, 0, null, 'Test title 2', 10, 0, null),
(3, 0, 0, 'test loooooooooooooong booooodyyyyyyyyyyyyyyyyyyyyyy', null, 0, '2022-01-09', '2022-01-09', 0, '2022-01-09', '2022-01-09', 'Test user', 1, 1, 1, 0, null, 'Test title 3', 10, 0, null),
(4, 0, 0, 'test loooooooooooooong booooodyyyyyyyyyyyyyyyyyyyyyy', null, 0, '2022-01-09', '2022-01-09', 0, '2022-01-09', '2022-01-09', 'Test user', 1, 1, 1, 0, null, 'Test title 4', 10, 0, null)
SET IDENTITY_INSERT [dbo].[Posts] OFF

SET IDENTITY_INSERT [dbo].[Users] ON 
INSERT INTO [dbo].[Users] ([Id],[CreateDate],[DisplayName],[DownVotes],[LastAccessDate],[Reputation],[UpVotes],[Views],[IsDeleted]) VALUES 
(1,'2022-01-09','Test User 1',0,'2022-01-09',0,0,0,0),
(2,'2022-01-09','Test User 2',0,'2022-01-09',0,0,0,0),
(3,'2022-01-09','Test User 3',0,'2022-01-09',0,0,0,0),
(4,'2022-01-09','Test User 4',0,'2022-01-09',0,0,0,0)
SET IDENTITY_INSERT [dbo].[Users] OFF
";

            context.Database.ExecuteSqlRaw(insertStatement);
        }
    }
}
